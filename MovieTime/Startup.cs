﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieTime.Entities;
using MovieTime.Entities.Overwatch;
using MovieTime.Services;
using MovieTime.Services.Overwatch;

namespace MovieTime
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
            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme
            ).AddCookie(options => {
                options.LoginPath = "/Account/Login/";
            });

            services.AddMvc();
            services.AddDbContext<MovieTimeContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MovieTimeContext")));

            services.AddDbContext<OverwatchContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MovieTimeContext")));
            services.AddDbContext<OverwatchContextBase>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MovieTimeContext")));

            services.AddScoped<AccountService>();
            services.AddScoped<MovieService>();
            services.AddScoped<WatchPartyService>();
            services.AddTransient<MatchService>(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCors(options => {
                options.AllowAnyOrigin();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}");
            });
        }
    }
}
