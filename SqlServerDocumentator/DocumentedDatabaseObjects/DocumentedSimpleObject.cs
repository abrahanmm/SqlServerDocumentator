using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
    public class DocumentedSimpleObject
    {
        public DocumentedSimpleObject(string serverName, string databaseName, string procedureName)
        {
            this.ServerName = serverName;
            this.DatabaseName = databaseName;
            this.Name = procedureName;
        }

        public string ServerName { get; }

        public string DatabaseName { get; }

        public string Name { get; }
    }
}
