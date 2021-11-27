using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Data;

namespace TweetBook4
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // automitic update database
            var host= CreateHostBuilder(args).Build();
            using (var serviceScope = host.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.MigrateAsync();
                // add role
                var roleManger = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if(!await roleManger.RoleExistsAsync("Admin"))
                {
                    var adminRole = new IdentityRole("Admin");
                    await roleManger.CreateAsync(adminRole);
                }

                if (!await roleManger.RoleExistsAsync("Poster"))
                {
                    var posterRole = new IdentityRole("Poster");
                    await roleManger.CreateAsync(posterRole);
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
