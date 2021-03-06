﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.DocumentedDatabaseObjects
{
    public class DocumentedServer
    {
        internal DocumentedServer(string serverName, string displayName, string description)
        {
            this.Name = serverName;
            this.DisplayName = displayName;
            this.Description = description;
        }

        public string Name { get; }

        public string DisplayName { get; }

        public string Description { get; }

    }
}
