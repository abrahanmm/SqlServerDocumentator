using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlServerDocumentator;
using SqlServerDocumentator.Infraestructure;

namespace WebSqlServerDocumentator
{
	public class Startup
	{
		//public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration)
		//{
		//	Configuration = configuration;
		//}

		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddUserSecrets<Startup>();
			Configuration = builder.Build();
		}


		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) =>
			services
				.AddSqlServerDocumentator(Configuration)
				.AddMvc();

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseMvc();

			//app.UseMvc(routes =>
			//{
			//    routes.MapRoute(
			//        name: "default",
			//        template: "{controller=Home}/{action=Index}/{id?}");
			//});
		}
	}
}
