using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
    public class DocumentedViewColumn
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string DataType { get; set; }
    }
}
