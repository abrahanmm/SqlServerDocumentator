using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SqlServerDocumentator.DocumentedDatabaseObjects;
using Microsoft.SqlServer.Management.Smo;
using SqlServerDocumentator.Infraestructure;
using Microsoft.Extensions.Options;

namespace SqlServerDocumentator
{
	class Documentator : IDocumentator
	{
		SqlDocumentatorConfiguration _configuration;

		public Documentator(IOptionsSnapshot<SqlDocumentatorConfiguration> configuration)
		{
			_configuration = configuration.Value;
		}

		public IEnumerable<DocumentedServer> GetServers()
		{
			return this._configuration.DocumentedServers;
		}

		public IEnumerable<DocumentedDatabase> GetDatabases(string serverName)
		{
            Server server = this.GetSMOServer(serverName);
			foreach (Database database in server.Databases)
			{
				if (!database.IsSystemObject)
					yield return new DocumentedDatabase()
					{
						Name = database.Name,
						ServerName = serverName,
					};
			}
		}

        public DocumentedDatabase GetDatabase(string serverName, string databaseName)
        {
            Database database = this.GetSMODatabase(serverName, databaseName);
            if (!database.IsSystemObject)
                return new DocumentedDatabase()
                {
                    Name = database.Name,
                    ServerName = serverName,
                    Description = (database.ExtendedProperties.Contains(this._configuration.Prefix)) ? database.ExtendedProperties[this._configuration.Prefix].Value.ToString() : null
                };
            else
                return null;
            
        }

        public IEnumerable<DocumentedSimpleObject> GetTables(string serverName, string databaseName)
		{
			return this.GetSimpleObject(serverName, databaseName
				, "SELECT t.name, SCHEMA_NAME(t.schema_id) as [schema], p.value FROM sys.tables t left join sys.extended_properties p on t.object_id = p.major_id and p.minor_id = 0 and p.name = @description ORDER BY t.name"
				, TypeDocumentedObject.Table);
		}

		public IEnumerable<DocumentedSimpleObject> GetViews(string serverName, string databaseName)
		{
			return this.GetSimpleObject(serverName, databaseName
				, "SELECT t.name, SCHEMA_NAME(t.schema_id) as [schema], p.value FROM sys.views t left join sys.extended_properties p on t.object_id = p.major_id and p.minor_id = 0 and p.name = @description ORDER BY t.name"
                , TypeDocumentedObject.View);
		}

		public IEnumerable<DocumentedSimpleObject> GetStoredProcedures(string serverName, string databaseName)
		{
			return this.GetSimpleObject(serverName, databaseName
				, "SELECT t.name, SCHEMA_NAME(t.schema_id) as [schema], p.value FROM sys.procedures t left join sys.extended_properties p on t.object_id = p.major_id and p.minor_id = 0 and p.name = @description ORDER BY t.name"
                , TypeDocumentedObject.StoredProcedure);
		}

		private IEnumerable<DocumentedSimpleObject> GetSimpleObject(string serverName, string databaseName, string query, TypeDocumentedObject type)
		{
			using (SqlConnection conn = new SqlConnection($"Server={serverName};Database={databaseName};Trusted_Connection=True;"))
			{
				using (SqlCommand command = new SqlCommand(query, conn))
				{
					command.Parameters.Add(new SqlParameter("@description", _configuration.Prefix));
					conn.Open();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							yield return new DocumentedSimpleObject(serverName, databaseName
								, reader.GetString(0)
								, reader.GetString(1)
								, (reader.IsDBNull(2)) ? null : reader.GetString(2)
								, type);
						}
					}
				}
			}
		}

        public DocumentedTable GetTable(string serverName, string databaseName, string schema, string tableName)
        {
            Table table = this.GetSMOTable(serverName, databaseName, schema, tableName);
            string description = (table.ExtendedProperties.Contains(_configuration.Prefix)) ?
                table.ExtendedProperties[_configuration.Prefix].Value.ToString() : string.Empty;
            DocumentedTable documentedTable = new DocumentedTable(serverName, databaseName, tableName, schema, description);
            foreach (Column col in table.Columns)
            {
                documentedTable.Columns.Add(
                    new DocumentedTableColumn()
                    {
                        inPrimaryKey = col.InPrimaryKey,
                        isForeignKey = col.IsForeignKey,
                        DataType = col.DataType.Name,
                        Name = col.Name,
                        Description = (col.ExtendedProperties.Contains(_configuration.Prefix)) ? col.ExtendedProperties[_configuration.Prefix].Value.ToString() : string.Empty
                    });
            }

            foreach (ForeignKey fk in table.ForeignKeys)
            {
                DocumentedForeignKey key = new DocumentedForeignKey()
                {
                    Name = fk.Name
                };
                foreach (ForeignKeyColumn fkCol in fk.Columns)
                {
                    key.Columns.Add(fkCol.Name);
                }

                documentedTable.ForeignKeys.Add(key);
            }

            return documentedTable;
        }

        public DocumentedView GetView(string serverName, string databaseName, string schema, string viewName)
        {
            View view = this.GetSMOView(serverName, databaseName, schema, viewName);
            string description = (view.ExtendedProperties.Contains(_configuration.Prefix)) ?
                view.ExtendedProperties[_configuration.Prefix].Value.ToString() : string.Empty;
            DocumentedView documentedView = new DocumentedView(serverName, databaseName, viewName, schema, description);

            foreach (Column col in view.Columns)
            {
                documentedView.Columns.Add(new DocumentedViewColumn()
                {
                    Name = col.Name,
                    DataType = col.DataType.Name
                });
            }

            return documentedView;
        }

        public DocumentedStoredProcedure GetStoredProcedure(string serverName, string databaseName, string schema, string storedProcedureName)
        {
            StoredProcedure procedure = this.GetSMOProcedure(serverName, databaseName, schema, storedProcedureName);
            string description = (procedure.ExtendedProperties.Contains(_configuration.Prefix)) ?
                procedure.ExtendedProperties[_configuration.Prefix].Value.ToString() : string.Empty;
            return new DocumentedStoredProcedure(serverName, databaseName, storedProcedureName, schema, description);
        }

        public DocumentedDatabase SaveDatabase(DocumentedDatabase database)
        {
            Database smoDatabase = this.GetSMODatabase(database.ServerName, database.Name);
            if (!smoDatabase.ExtendedProperties.Contains(this._configuration.Prefix))
            {
                smoDatabase.ExtendedProperties.Add(new ExtendedProperty(smoDatabase, this._configuration.Prefix, database.Description));
            }
            else
            {
                smoDatabase.ExtendedProperties[this._configuration.Prefix].Value = database.Description;
            }
            smoDatabase.Alter();
            return this.GetDatabase(database.ServerName, database.Name);
        }

        public DocumentedTable SaveTable(DocumentedTable table)
        {
            Table smoTable = this.GetSMOTable(table.ServerName, table.DatabaseName, table.Schema, table.Name);
            foreach (DocumentedTableColumn col in table.Columns)
            {
                if (!smoTable.Columns.Contains(col.Name))
                    throw new KeyNotFoundException("Not exist Column with the name: " + col.Name);
            }

                if (!smoTable.ExtendedProperties.Contains(this._configuration.Prefix))
            {
                smoTable.ExtendedProperties.Add(new ExtendedProperty(smoTable, this._configuration.Prefix, table.Description));
            }
            else
            {
                smoTable.ExtendedProperties[this._configuration.Prefix].Value = table.Description;
            }
            foreach (DocumentedTableColumn col in table.Columns)
            {
                Column smoCol = smoTable.Columns[col.Name];
                if (!smoCol.ExtendedProperties.Contains(this._configuration.Prefix))
                {
                    smoCol.ExtendedProperties.Add(new ExtendedProperty(smoCol, this._configuration.Prefix, col.Description));
                }
                else
                {
                    smoCol.ExtendedProperties[this._configuration.Prefix].Value = col.Description;
                }
            }
            smoTable.Alter();
            return this.GetTable(table.ServerName, table.DatabaseName, table.Schema, table.Name);
        }

        public DocumentedView SaveView(DocumentedView view)
        {
            View smoView = this.GetSMOView(view.ServerName, view.DatabaseName, view.Schema, view.Name);
            if (!smoView.ExtendedProperties.Contains(this._configuration.Prefix))
            {
                smoView.ExtendedProperties.Add(new ExtendedProperty(smoView, this._configuration.Prefix, view.Description));
            }
            else
            {
                smoView.ExtendedProperties[this._configuration.Prefix].Value = view.Description;
            }
            smoView.Alter();
            return this.GetView(view.ServerName, view.DatabaseName, view.Schema, view.Name);
        }

        public DocumentedStoredProcedure SaveStoredProcedure(DocumentedStoredProcedure procedure)
        {
            StoredProcedure smoProcedure = this.GetSMOProcedure(procedure.ServerName, procedure.DatabaseName, procedure.Schema, procedure.Name);
            if (!smoProcedure.ExtendedProperties.Contains(this._configuration.Prefix))
            {
                smoProcedure.ExtendedProperties.Add(new ExtendedProperty(smoProcedure, this._configuration.Prefix, procedure.Description));
            }
            else
            {
                smoProcedure.ExtendedProperties[this._configuration.Prefix].Value = procedure.Description;
            }
            smoProcedure.Alter();
            return procedure;
        }

        private Server GetSMOServer(string serverName)
        {
            if (this._configuration.Servers.Any(x => x.Name.Equals(serverName)))
                return new Server(serverName);
            else
                throw new KeyNotFoundException("Not exist server with the name: " + serverName);
        }

        private Database GetSMODatabase(string serverName, string databaseName)
        {
            Server server = this.GetSMOServer(serverName);
            if (server.Databases.Contains(databaseName))
                return server.Databases[databaseName];
            else
                throw new KeyNotFoundException("Not exist database with the name: " + databaseName);
        }

        private Table GetSMOTable(string serverName, string databaseName, string schema, string tableName)
        {
            Database database = this.GetSMODatabase(serverName, databaseName);
            if (database.Tables.Contains(tableName, schema))
                return database.Tables[tableName, schema];
            else
                throw new KeyNotFoundException($"Not exist Table with the name: {schema}.{tableName}");
        }

        private View GetSMOView(string serverName, string databaseName, string schema, string viewName)
        {
            Database database = this.GetSMODatabase(serverName, databaseName);
            if (database.Views.Contains(viewName, schema))
                return database.Views[viewName, schema];
            else
                throw new KeyNotFoundException($"Not exist View with the name: {schema}.{viewName}");
        }

        private StoredProcedure GetSMOProcedure(string serverName, string databaseName, string schema, string procedureName)
        {
            Database database = this.GetSMODatabase(serverName, databaseName);
            if (database.StoredProcedures.Contains(procedureName, schema))
                return database.StoredProcedures[procedureName, schema];
            else
                throw new KeyNotFoundException($"Not exist Stored Procedure with the name: {schema}.{procedureName}");
        }
    }
}
