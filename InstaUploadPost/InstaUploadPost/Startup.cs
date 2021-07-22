using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaUploadPost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            #region Configure Instagram Api

            var username = Configuration.GetSection("Insta.Username").Value;
            var password = Configuration.GetSection("Insta.Password").Value;

            Console.WriteLine($"Username : {username} , Password : {password}");

            var user = new UserSessionData()
            {
                UserName = username,
                Password = password
            };
            var api = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseLogger(new DebugLogger(InstagramApiSharp.Logger.LogLevel.Info))
                .SetRequestDelay(RequestDelay.FromSeconds(10, 20))
                .Build();

            services.AddSingleton<UserSessionData>(user);
            services.AddSingleton<IInstaApi>(api);

            var logginResult = await api.LoginAsync();

            if (logginResult.Succeeded)
            {
                Console.WriteLine("Instagram Logged In Successfully ;)");
            }
            else
                throw new Exception("Username Or Password Is Invalid :(");

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
