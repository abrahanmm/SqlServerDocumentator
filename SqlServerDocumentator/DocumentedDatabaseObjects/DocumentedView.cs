using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
	public class DocumentedView
	{
		public DocumentedView(string serverName, string databaseName, string tableName)
		{
			this.ServerName = serverName;
			this.DatabaseName = databaseName;
			this.Name = tableName;
		}

		public string ServerName { get; }

		public string DatabaseName { get; }

		public string Name { get; }
	}
}
