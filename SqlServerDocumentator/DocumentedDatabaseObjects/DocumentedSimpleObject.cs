using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
	public class DocumentedSimpleObject
	{
		public DocumentedSimpleObject(string serverName, string databaseName, string objectName, string description, TypeDocumentedObject type)
		{
			this.ServerName = serverName;
			this.DatabaseName = databaseName;
			this.Name = objectName;
			this.Description = description;
			this.Type = type;
		}

		public string ServerName { get; }

		public string DatabaseName { get; }

		public string Name { get; }

		public string Description { get; }

		public TypeDocumentedObject Type { get; set; }
	}
}
