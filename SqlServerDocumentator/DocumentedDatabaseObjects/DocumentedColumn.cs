using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
    public class DocumentedColumn
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool isPrimaryKey { get; set; }
    }
}
