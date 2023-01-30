using Autofac;
using Api.Constants;
using Domain.RulesEngine.Business.Modules;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Api.RulesEngine
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    c => c.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc(ApiConstant.S_VERSION, new OpenApiInfo
				{
					Title = ApiConstant.TITLE,
					Version = ApiConstant.S_VERSION
				});
				c.CustomSchemaIds(x => x.FullName);
				c.OperationFilter<SwaggerCustomOperationFilter>();
			});
            services.AddHealthChecks().AddNpgSql(
                name: "db-health",
                npgsqlConnectionString: Configuration["ConnectionStrings:RulesEngine"],
                failureStatus: HealthStatus.Degraded,
                tags: new string[] {"db", "npgsql", "postgres"});

            services.AddControllers();

		}

		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule(new BapsUtilityModule());
			builder.RegisterModule(new BapsDataBapsModule(Configuration));
			builder.RegisterModule(new BapsDomainServicesModule(ApiConstant.D_NAME));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHealthChecks("/selfcheck", new HealthCheckOptions
			{
				Predicate = _ => true,
				ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
			}).UseHealthChecksUI();
			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint($"/Swagger/{ApiConstant.S_VERSION}/swagger.json", ApiConstant.TITLE));
			app.UseRouting();
			app.UseAuthorization();
            app.UseCors("CorsPolicy");

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHealthChecks("/health");
				endpoints.MapControllers();
				endpoints.MapHealthChecksUI();
			});
		}
	}
}
