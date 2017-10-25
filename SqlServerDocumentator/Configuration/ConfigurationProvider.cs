using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.Configuration
{
    public static class ConfigurationProvider
    {
        private static ConcurrentBag<DocumentedServer> servers = new ConcurrentBag<DocumentedServer>();

        public static void AddServer(string serverName, string displayName, string description)
        {
            ConfigurationProvider.AddServer(new DocumentedServer()
            {
                Name = serverName,
                DisplayName = displayName,
                Description = description
            });
        }

        public static void AddServer(DocumentedServer server)
        {
            ConfigurationProvider.servers.Add(server);
        }

        public static IDocumentator CreateInstance()
        {
            return new Documentator(ConfigurationProvider.servers.ToArray());
        }
    }
}
