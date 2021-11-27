using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Data;
using TweetBook4.Service;

namespace TweetBook4.Installers
{
    public class DbInstaller : Installer
    {
        private IConfiguration _config;
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            _config = configuration;
            services.AddDbContextPool<AppDbContext>(
              options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));
            services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ITagsService, TagsService>();
        }
    }
}
