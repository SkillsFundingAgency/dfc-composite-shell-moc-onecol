﻿using DFC.Composite.Shell.Moc.OneCol.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DFC.Composite.Shell.Moc.OneCol
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<ITradeService, TradeService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                // add the trades routing
                routes.MapRoute(
                    name: $"Trade-Index-Category",
                    template: "Trade/Category/{category}",
                    defaults: new { controller = "Trade", action = "Index" }
                );
                routes.MapRoute(
                    name: $"Trade-Index-Filter",
                    template: "Trade/Filter/{filter}",
                    defaults: new { controller = "Trade", action = "Index" }
                );
                routes.MapRoute(
                    name: $"Trade-Index",
                    template: "Trade/Index",
                    defaults: new { controller = "Trade", action = "Index" }
                );
                routes.MapRoute(
                    name: $"Trade-Search",
                    template: "Trade/Search/{searchClue?}",
                    defaults: new { controller = "Trade", action = "Search" }
                );
                routes.MapRoute(
                    name: $"Trade-Index-Search",
                    template: "Trade/{searchClue}",
                    defaults: new { controller = "Trade", action = "Index" }
                );

                // add the site map route
                routes.MapRoute(
                    name: "Sitemap",
                    template: "Sitemap",
                    defaults: new { controller = "Sitemap", action = "Sitemap" }
                );

                // add the default route
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
