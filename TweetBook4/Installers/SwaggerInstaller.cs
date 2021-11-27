using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TweetBook4.Installers
{
    public class SwaggerInstaller : Installer
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(
                x => {
                    x.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "API Title is",
                        Version = "v1"
                    });

                    var security = new Dictionary<string, IEnumerable<string>>
                    {
                        {"Bearer",new string[0] }
                    };
                    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        //Type = SecuritySchemeType.Http,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        In = ParameterLocation.Header,
                        Description = "Basic Authorization header using the Bearer scheme."
                    });
                    x.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                              new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] {}
                        }
                    });
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    x.IncludeXmlComments(xmlPath);
                }
            );
        }
    }
}
