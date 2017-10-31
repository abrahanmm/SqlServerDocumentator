using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SqlServerDocumentator.Configuration;
using SqlServerDocumentator;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerDocumentator(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<IDocumentator, Documentator>();
            return services;
        }
    }
}
