using SqlServerDocumentator.DocumentedDatabaseObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.Configuration
{
    public interface IConfigurationProvider
    {
        IConfigurationProvider AddServer(string serverName, string displayName, string description);
        IConfigurationProvider UsePrefix(string prefix);
        IConfiguration BuildConfiguration();
    }
}
