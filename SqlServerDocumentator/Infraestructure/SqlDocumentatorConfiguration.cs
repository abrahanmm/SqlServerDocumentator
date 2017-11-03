using SqlServerDocumentator.DocumentedDatabaseObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.Infraestructure
{
	public class SqlDocumentatorConfiguration
	{
		public string Prefix { get; set; }

		public ConfigurationServer[] Servers { get; set; }

		public IEnumerable<DocumentedServer> DocumentedServers
		{
			get
			{
				foreach (ConfigurationServer server in this.Servers)
				{
					yield return new DocumentedServer(server.Name, server.DisplayName, server.Description);
				}
			}
		}

	}
}
