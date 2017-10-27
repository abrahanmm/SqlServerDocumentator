using SqlServerDocumentator.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using SqlServerDocumentator.DocumentedDatabaseObjects;
using Microsoft.SqlServer.Management.Smo;

namespace SqlServerDocumentator
{
	class Documentator : IDocumentator
	{
		List<DocumentedServer> servers = new List<DocumentedServer>();

		public Documentator(DocumentedServer[] servers)
		{
			this.servers.AddRange(servers);
		}

		public IEnumerable<DocumentedServer> GetServers()
		{
			return this.servers;
		}

		public IEnumerable<DocumentedDatabase> GetDatabases(string serverName)
		{
			Server server = new Server(serverName);
			foreach (Database database in server.Databases)
			{
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
	}
}
