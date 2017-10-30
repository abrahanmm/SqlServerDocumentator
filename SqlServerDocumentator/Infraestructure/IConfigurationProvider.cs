using SqlServerDocumentator.DocumentedDatabaseObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.Configuration
{
    public interface IConfigurationProvider
    {
        IConfigurationProvider AddServer(string serverName, string displayName, string description);
        IConfigurationProvider AddServer(DocumentedServer server);
        IDocumentator CreateInstance();
    }
}
