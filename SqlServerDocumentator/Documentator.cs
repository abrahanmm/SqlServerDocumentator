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

		public IEnumerable<DocumentedTable> GetTables(string serverName, string databaseName)
		{
            using (SqlConnection conn = new SqlConnection($"Server={serverName};Database={databaseName};Trusted_Connection=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT name FROM sys.views", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new DocumentedTable(serverName, databaseName, reader.GetString(0));
                        }
                    }
                }
            }
        }

		public IEnumerable<DocumentedView> GetViews(string serverName, string databaseName)
		{
            using (SqlConnection conn = new SqlConnection($"Server={serverName};Database={databaseName};Trusted_Connection=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT name FROM sys.views", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new DocumentedView(serverName, databaseName, reader.GetString(0));
                        }
                    }
                }
            }
        }

		public IEnumerable<DocumentedStoredProcedure> GetStoredProcedures(string serverName, string databaseName)
		{
            using (SqlConnection conn = new SqlConnection($"Server={serverName};Database={databaseName};Trusted_Connection=True;"))
            {
                using (SqlCommand command = new SqlCommand("SELECT name FROM sys.procedures", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new DocumentedStoredProcedure(serverName, databaseName, reader.GetString(0));
                        }
                    }
                }
            }

        }
    }
}
