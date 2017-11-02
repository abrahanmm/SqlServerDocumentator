using SqlServerDocumentator.DocumentedDatabaseObjects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        List<ConfigurationServer> _servers = new List<ConfigurationServer>();

        string _prefix;

        public ConfigurationProvider()
        {

        }

        public IConfigurationProvider AddServer(string serverName, string displayName, string description)
        {
            _servers.Add(new ConfigurationServer
            ()
            { Name = serverName, DisplayName = displayName, Description = description });

            return this;
        }

        public IConfigurationProvider UsePrefix(string prefix)
        {
            _prefix = prefix;
            return this;
        }

        public IConfiguration BuildConfiguration()
        {
            return new Configuration(_servers.ToArray(), _prefix);
        }


    }
}
