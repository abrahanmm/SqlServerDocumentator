using SqlServerDocumentator.Configuration;
using SqlServerDocumentator.DocumentedDatabaseObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator
{
	public interface IDocumentator
	{
		IEnumerable<DocumentedServer> GetServers();

		IEnumerable<DocumentedDatabase> GetDatabases(string serverName);

		IEnumerable<DocumentedTable> GetTables(string serverName, string databaseName);

		IEnumerable<DocumentedView> GetViews(string serverName, string databaseName);

	}
}
