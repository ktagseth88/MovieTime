using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            ).AddCookie(options =>
            {
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            if (env.EnvironmentName.Equals(Environments.Development))
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Account}/{action=Login}");
                endpoints.MapControllerRoute("angular", "angular/{*url}", new { controller = "Angular", action = "Start" });
            });

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Account}/{action=Login}");

            //    routes.MapRoute(
            //        "angular",
            //        "angular/{*url}",
            //        new { controller = "Angular", action = "Start" });
            //});

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "Angular";
                spa.UseAngularCliServer(npmScript: "start");
            });
        }
    }
}
