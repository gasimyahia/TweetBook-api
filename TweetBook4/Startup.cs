using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.HealthCheck;
using TweetBook4.Data;
using TweetBook4.Domain;
using TweetBook4.Installers;
using TweetBook4.Options;

namespace TweetBook4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.installServiceInAssembly(Configuration);
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health",new HealthCheckOptions 
            {
                ResponseWriter=async (context, report) =>
                {
                    context.Response.ContentType = "appliction/json";
                    var response = new HealthCheckResponse
                    {
                        Status = report.Status.ToString(),
                        Checks = report.Entries.Select(x => new HealthCheck
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description=x.Value.Description
                        }),
                        Duration=report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            });

            var swaggerOption = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOption);

            app.UseSwagger(option => {
                //option.RouteTemplate = swaggerOption.JsonRoute;
                option.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(option => {
                option.SwaggerEndpoint(swaggerOption.UiEndpoint, swaggerOption.Description);
                option.RoutePrefix = string.Empty;
            });


            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
