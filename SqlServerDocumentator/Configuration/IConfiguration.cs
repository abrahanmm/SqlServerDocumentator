using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using SqlServerDocumentator.DocumentedDatabaseObjects;

namespace SqlServerDocumentator.Configuration
{
    public interface IConfiguration
    {
        ImmutableList<ConfigurationServer> ConfigurationServers { get; }

        ImmutableList<DocumentedServer> DocumentedServers { get; }

        string Prefix { get; }
    }
}
