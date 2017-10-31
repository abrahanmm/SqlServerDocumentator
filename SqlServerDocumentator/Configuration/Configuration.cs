using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using SqlServerDocumentator.DocumentedDatabaseObjects;

namespace SqlServerDocumentator.Configuration
{
    internal class Configuration : IConfiguration
    {
        public Configuration(ConfigurationServer[] servers, string prefix)
        {
            this.ConfigurationServers = ImmutableList.Create(servers);

            List<DocumentedServer> list = new List<DocumentedServer>();
            foreach (ConfigurationServer server in servers)
                list.Add(new DocumentedServer(server.Name, server.DisplayName, server.Description));
            this.DocumentedServers = ImmutableList.Create(list.ToArray());

            this.Prefix = prefix;
        }

        public ImmutableList<ConfigurationServer> ConfigurationServers { get; }

        public ImmutableList<DocumentedServer> DocumentedServers { get; }

        public string Prefix { get; }
    }
}
