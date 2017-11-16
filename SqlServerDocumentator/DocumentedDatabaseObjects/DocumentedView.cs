using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
    public class DocumentedView : DocumentedSimpleObject
    {
        public DocumentedView(string serverName, string databaseName, string procedureName) : base(serverName, databaseName, procedureName)
        {
        }
    }
}
