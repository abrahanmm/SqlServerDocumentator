﻿using SqlServerDocumentator.DocumentedDatabaseObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator
{
	public interface IDocumentator
	{
		IEnumerable<DocumentedServer> GetServers();

		IEnumerable<DocumentedDatabase> GetDatabases(string serverName);

		DocumentedDatabase SaveDatabase(DocumentedDatabase database);

		IEnumerable<DocumentedSimpleObject> GetTables(string serverName, string databaseName);

		IEnumerable<DocumentedSimpleObject> GetViews(string serverName, string databaseName);

		IEnumerable<DocumentedSimpleObject> GetStoredProcedures(string serverName, string databaseName);

		DocumentedTable GetTable(string serverName, string databaseName, string schema, string tableName);

	}
}
