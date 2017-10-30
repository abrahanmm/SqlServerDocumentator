using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerDocumentator(this IServiceCollection services,
            (string serverName, string diplayServerName, string serverDescription)[] servers)
        {
            return services;
        }
    }
}
