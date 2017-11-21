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
			Server server = new Server(serverName);
			foreach (Database database in server.Databases)
			{
				if (!database.IsSystemObject)
					yield return new DocumentedDatabase()
					{
						Name = database.Name,
						ServerName = serverName
					};
			}
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
            Server server = new Server(serverName);
            string description = (server.Databases[databaseName].Tables[tableName, schema].ExtendedProperties.Contains(_configuration.Prefix)) ?
                server.Databases[databaseName].Tables[tableName, schema].ExtendedProperties[_configuration.Prefix].Value.ToString() : string.Empty;
            DocumentedTable documentedTable = new DocumentedTable(serverName, databaseName, tableName, schema, description);
            foreach (Column col in server.Databases[databaseName].Tables[tableName, schema].Columns)
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

            foreach (ForeignKey fk in server.Databases[databaseName].Tables[tableName, schema].ForeignKeys)
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
            Server server = new Server(serverName);
            string description = (server.Databases[databaseName].Views[viewName, schema].ExtendedProperties.Contains(_configuration.Prefix)) ?
                server.Databases[databaseName].Views[viewName, schema].ExtendedProperties[_configuration.Prefix].Value.ToString() : string.Empty;
            DocumentedView view = new DocumentedView(serverName, databaseName, viewName, schema, description);

            foreach (Column col in server.Databases[databaseName].Views[viewName, schema].Columns)
            {
                view.Columns.Add(new DocumentedViewColumn()
                {
                    Name = col.Name,
                    DataType = col.DataType.Name
                });
            }

            return view;
        }

        public DocumentedStoredProcedure GetStoredProcedure(string serverName, string databaseName, string schema, string storedProcedureName)
        {
            Server server = new Server(serverName);
            string description = (server.Databases[databaseName].StoredProcedures[storedProcedureName, schema].ExtendedProperties.Contains(_configuration.Prefix)) ?
                server.Databases[databaseName].Views[storedProcedureName, schema].ExtendedProperties[_configuration.Prefix].Value.ToString() : string.Empty;
            return new DocumentedStoredProcedure(serverName, databaseName, storedProcedureName, schema, description);
        }

        public DocumentedDatabase SaveDatabase(DocumentedDatabase database)
        {
            Server server = new Server(database.ServerName);
            if (!server.Databases[database.Name].ExtendedProperties.Contains("desctiption"))
            {
                server.Databases[database.Name].ExtendedProperties.Add(new ExtendedProperty(server.Databases[database.Name], "desctiption", database.Description));
            }
            else
            {
                server.Databases[database.Name].ExtendedProperties["desctiption"].Value = database.Description;
            }
            return database;
        }

        public DocumentedSimpleObject SaveTable(DocumentedSimpleObject table)
        {
            return table;
        }

        private Server GetServer(string serverName)
        {
            if (this._configuration.Servers.Any(x => x.Name.Equals(serverName)))
                return new Server(serverName);
            else
                throw new KeyNotFoundException("Not exist server with the name: " + serverName);
        }
    }
}
