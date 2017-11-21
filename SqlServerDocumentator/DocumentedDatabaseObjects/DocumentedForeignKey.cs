using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
    public class DocumentedForeignKey
    {
        public DocumentedForeignKey()
        {
            this.Columns = new List<string>();
        }

        public string Name { get; set; }

        public IList<string> Columns { get; set; }
    }
}
