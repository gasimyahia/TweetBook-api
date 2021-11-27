using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Data;

namespace TweetBook4.Installers
{
    public class HealthChecksInstaller : Installer
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>();
        }
    }
}
