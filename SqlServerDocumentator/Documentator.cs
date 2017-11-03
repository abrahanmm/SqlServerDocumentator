using System;
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

		public IEnumerable<DocumentedTable> GetTables(string serverName, string databaseName)
		{
			Server server = new Server(serverName);
			foreach (Table table in server.Databases[databaseName].Tables)
			{
				if (!table.IsSystemObject)
					yield return new DocumentedTable(serverName, databaseName, table.Name);
			}
		}

		public IEnumerable<DocumentedView> GetViews(string serverName, string databaseName)
		{
			Server server = new Server(serverName);
			foreach (View view in server.Databases[databaseName].Views)
			{
				if (!view.IsSystemObject)
					yield return new DocumentedView(serverName, databaseName, view.Name);
			}
		}

		public IEnumerable<DocumentedStoredProcedure> GetStoredProcedures(string serverName, string databaseName)
		{
			Server server = new Server(serverName);
			foreach (StoredProcedure procedure in server.Databases[databaseName].StoredProcedures)
			{
				if (!procedure.IsSystemObject)
					yield return new DocumentedStoredProcedure(serverName, databaseName, procedure.Name);
			}
		}
	}
}
