using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using SqlServerDocumentator;
using SqlServerDocumentator.Infraestructure;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddSqlServerDocumentator(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<SqlDocumentatorConfiguration>(configuration.GetSection(nameof(SqlDocumentatorConfiguration)))
				.AddScoped<IDocumentator, Documentator>();
			return services;
		}
	}
}
