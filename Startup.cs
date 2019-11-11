using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using BlazorServerSideApplication.Data; //Comment out.
using XpoTutorial;
using Microsoft.Extensions.Configuration;
using BlazorServerSideApplication.Services;

namespace BlazorServerSideApplication
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
            //Added lines begin.
            //services.AddSingleton<WeatherForecastService>();
            services.AddScoped<PreguntaService>();
            services.AddScoped<RespuestaService>();
            services.AddScoped<TemaService>();
            services.AddXpoDefaultDataLayer(ServiceLifetime.Singleton, dl => dl
                .UseConnectionString(Configuration.GetConnectionString("ImMemoryDataStore"))
                .UseThreadSafeDataLayer(true)
                .UseConnectionPool(false) // Remove this line if you use a database server like SQL Server, Oracle, PostgreSql etc.
                .UseAutoCreationOption(DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema) // Remove this line if the database already exists
                .UseEntityTypes(typeof(Pregunta), typeof(Respuesta), typeof(Tema)) // Pass all of your persistent object types to this method.
            );
            services.AddXpoDefaultUnitOfWork();
            //Added lines end.
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            //Added lines begin.
            app.UseXpoDemoData();
            //Added lines end.
        }
    }
}
