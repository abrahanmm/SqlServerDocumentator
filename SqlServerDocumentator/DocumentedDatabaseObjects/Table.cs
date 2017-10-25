using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
    public class Table
    {
        public Table(string serverName, string tableName)
        {
            this.ServerName = serverName;
            this.Name = tableName;
        }

        public string ServerName { get; }

        public string Name { get; }
    }
}
