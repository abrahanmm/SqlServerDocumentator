﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
	public class DocumentedStoredProcedure
	{
        public DocumentedStoredProcedure(string serverName, string databaseName, string name, string schema, string description)
        {
            this.ServerName = serverName;
            this.DatabaseName = databaseName;
            this.Name = name;
            this.Schema = schema;
            this.Description = description;
        }

        public string ServerName { get; }

        public string DatabaseName { get; }

        public string Name { get; }

        public string Description { get; }

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

        public TypeDocumentedObject Type { get { return TypeDocumentedObject.StoredProcedure; } }
    }
}
