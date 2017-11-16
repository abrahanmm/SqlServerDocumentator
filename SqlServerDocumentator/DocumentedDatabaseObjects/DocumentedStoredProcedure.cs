using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
    public class DocumentedStoredProcedure : DocumentedSimpleObject
    {
        public DocumentedStoredProcedure(string serverName, string databaseName, string procedureName) : base(serverName, databaseName, procedureName)
        {
        }
    }
}
