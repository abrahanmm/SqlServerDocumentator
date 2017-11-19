using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
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

		public IEnumerable<DocumentedSimpleObject> GetTables(string serverName, string databaseName)
		{                            
            return this.GetSimpleObject(serverName, databaseName
                , "SELECT t.name, p.value FROM sys.tables t left join sys.extended_properties p on t.object_id = p.major_id and p.minor_id = 0 and p.name = @description ORDER BY t.name"
                , TypeDocumentedObject.Table);
		}

		public IEnumerable<DocumentedSimpleObject> GetViews(string serverName, string databaseName)
		{
			return this.GetSimpleObject(serverName, databaseName
                , "SELECT t.name, p.value FROM sys.views t left join sys.extended_properties p on t.object_id = p.major_id and p.minor_id = 0 and p.name = @description ORDER BY t.name"
                , TypeDocumentedObject.View);
        }

		public IEnumerable<DocumentedSimpleObject> GetStoredProcedures(string serverName, string databaseName)
		{
			return this.GetSimpleObject(serverName, databaseName
                , "SELECT t.name, p.value FROM sys.procedures t left join sys.extended_properties p on t.object_id = p.major_id and p.minor_id = 0 and p.name = @description ORDER BY t.name"
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
                                ,reader.GetString(0)
                                ,(reader.IsDBNull(1))? null: reader.GetString(1)
                                ,type);
						}
					}
				}
			}
		}

        public DocumentedTable GetTable(string serverName, string databaseName, string tableName)
        {
            Server server = new Server(serverName);
            string description = (server.Databases[databaseName].Tables[tableName].ExtendedProperties.Contains(_configuration.Prefix)) ?
                server.Databases[databaseName].Tables[tableName].ExtendedProperties[_configuration.Prefix].Value.ToString() : string.Empty;
            DocumentedTable documentedTable = new DocumentedTable(serverName, databaseName, tableName, description);
            foreach (Column col in server.Databases[databaseName].Tables[tableName].Columns)
            {
                documentedTable.Columns.Add(
                    new DocumentedColumn()
                    {
                        isPrimaryKey = col.InPrimaryKey,
                        Name = col.Name,
                        Description = (col.ExtendedProperties.Contains(_configuration.Prefix)) ? col.ExtendedProperties[_configuration.Prefix].Value.ToString() : string.Empty
                    });
            }
            return documentedTable;
        }

    }
}
