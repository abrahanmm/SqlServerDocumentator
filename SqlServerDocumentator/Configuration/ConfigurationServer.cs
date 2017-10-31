using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.Configuration
{
    public class ConfigurationServer
    {
        internal ConfigurationServer(string serverName, string displayName, string description)
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
