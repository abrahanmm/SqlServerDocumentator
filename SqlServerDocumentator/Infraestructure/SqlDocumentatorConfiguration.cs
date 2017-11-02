using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerDocumentator.Infraestructure
{
    public class SqlDocumentatorConfiguration
    {
        public string Prefix { get; set; }

        public Configuration.ConfigurationServer[] Servers { get; set; }
    }
}
