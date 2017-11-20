using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
	public class DocumentedSimpleObject
	{
		public DocumentedSimpleObject(string serverName, string databaseName, string objectName, string schemaName, string description, TypeDocumentedObject type)
		{
			this.ServerName = serverName;
			this.DatabaseName = databaseName;
			this.Name = objectName;
			this.Description = description;
			this.Type = type;
			this.Schema = schemaName;
		}

		public string ServerName { get; }

		public string DatabaseName { get; }

		public string Name { get; }

		public string CompleteName
		{
			get
			{
				if ("dbo".Equals(this.Schema))
					return this.Name;
				else
					return this.Schema + "." + this.Name;
			}

		}

		public string Schema { get; }

		public string Description { get; }

		public TypeDocumentedObject Type { get; set; }
	}
}
