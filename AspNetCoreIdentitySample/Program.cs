using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreIdentitySample.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCoreIdentitySample
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
           var host= CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                #region Seed Roles
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await roleManager.CreateAsync(new IdentityRole { Name = "admin" }); 
                #endregion

                #region Sehirler seed
                var db = services.GetRequiredService<ApplicationDbContext>();
                if (!db.Sehirler.Any())
                {
                    db.Sehirler.Add(new Sehir { Id = 6, SehirAd = "Ankara" });
                    db.Sehirler.Add(new Sehir { Id = 34, SehirAd = "Istanbul" });
                    db.SaveChanges();
                }
                #endregion


                #region UserSeed
                var usermanager = services.GetRequiredService<UserManager<IdentityUser>>();
                var adminUser = new IdentityUser { UserName = "okan@karadag.com", Email = "okan@karadag.com", EmailConfirmed = true };
                await usermanager.CreateAsync(adminUser, "Password1.");
                await usermanager.AddToRoleAsync(adminUser, "admin");

                var sampleUser = new IdentityUser { UserName = "okan1@karadag.com", Email = "okan1@karadag.com", EmailConfirmed = true };
                await usermanager.CreateAsync(sampleUser, "Password1.");
                #endregion


            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
