using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpoTest;
using Microsoft.Extensions.Configuration;
using OpoTest.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.Toast;

namespace OpoTest
{
    public class Startup
    {
        //Added lines begin.
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        //Added lines end.

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor(x => x.DetailedErrors = true);
            services.AddAuthentication();
            services.AddProtectedBrowserStorage();
            services.AddScoped<AuthenticationStateProvider, AutenticationService>();
            services.AddBlazoredToast();

            services.AddXpoDefaultDataLayer(ServiceLifetime.Singleton, dl => dl
                .UseConnectionString(Configuration.GetConnectionString("MSS"))
                .UseThreadSafeDataLayer(true)
                //.UseAutoCreationOption(DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema) // Remove this line if the database already exists
                .UseEntityTypes(System.Reflection.Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.GetInterface(typeof(DevExpress.Xpo.IXPSimpleObject).FullName) != null).ToArray()) // Pass all of your persistent object types to this method.
            );


            services.AddScoped<ExamenService>();
            services.AddScoped<XpoService<ExamenPregunta>>();
            services.AddScoped<XpoService<ExamenRespuesta>>();

            services.AddScoped<XpoService<PlantillaPregunta>>();
            services.AddScoped<XpoService<PlantillaRespuesta>>();
            services.AddScoped<XpoService<Tema>>();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

#if DEBUG
            //app.UseXpoDemoData();
#endif
        }
    }
}
