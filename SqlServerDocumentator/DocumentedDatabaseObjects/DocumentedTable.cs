using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
    public class DocumentedTable : DocumentedSimpleObject
    {
        public DocumentedTable(string serverName, string databaseName, string procedureName) : base(serverName, databaseName, procedureName)
        {
        }
    }
}
