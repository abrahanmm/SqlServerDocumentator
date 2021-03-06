﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
    public class DocumentedTableColumn
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool inPrimaryKey { get; set; }

        public bool isForeignKey { get; set; }

        public string DataType { get; set; }
    }
}
