using SqlServerDocumentator.DocumentedDatabaseObjects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        List<DocumentedServer> servers = new List<DocumentedServer>();

        private ConfigurationProvider()
        {

        }

        public static IConfigurationProvider StartConfiguration()
        {
            return new ConfigurationProvider();
        }

        public IConfigurationProvider AddServer(string serverName, string displayName, string description)
        {
            servers.Add(new DocumentedServer()
            {
                Name = serverName,
                DisplayName = displayName,
                Description = description
            });

            return this;
        }

        public IConfigurationProvider AddServer(DocumentedServer server)
        {
            servers.Add(server);
            return this;
        }

        public IDocumentator CreateInstance()
        {
            return new Documentator(servers.ToArray());
        }
    }
}
