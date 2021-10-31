using AnindaKapinda_MVC.CustomValidator;
using AnindaKapinda_MVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AnindaKapinda_MVC
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
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddHttpContextAccessor();
            services.AddControllersWithViews().AddFluentValidation(a => a.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddRazorPages();
            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AuthConnectionString")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(Configuration.GetSection(nameof(IdentityOptions)));


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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
