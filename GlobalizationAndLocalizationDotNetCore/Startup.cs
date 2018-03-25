using System.Globalization;
using System.Threading.Tasks;
using GlobalisationAndLocalisationDotNetCore.Infrastucture.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GlobalisationAndLocalisationDotNetCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

           

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add the localization services to the services container
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
              // Add support for finding localized views, based on file name suffix, e.g. Index.fr.cshtml
              .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
              // Add support for localizing strings in data annotations (e.g. validation messages) via the
              // IStringLocalizer abstractions.
              .AddDataAnnotationsLocalization();


            // Configure supported cultures and localization options
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-GB"),
                    new CultureInfo("fr"),
                    new CultureInfo("ar-YE")
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture(culture: "en-GB", uiCulture: "en-GB");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;

              //  options.RequestCultureProviders.Insert(0,new RouteDataRequestCultureProvider());
                // By default, the following built-in providers are configured:
                // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
                // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
                // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
            });

            services.Configure<RouteOptions>(opts =>
                opts.ConstraintMap.Add("culturecode", typeof(CultureRouteConstraint)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<RequestLocalizationOptions> localizationOptions)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{culture:culturecode}/{controller=Home}/{action=Index}/{id?}");
                routes.MapGet("{culture:culturecode}/{*path}", appBuilder => { return null; /* 404 route */ });

                // if no culture set then redirect
                routes.MapGet("{*path}", (RequestDelegate)(context =>  
                {
                    var defaultCulture = localizationOptions.Value.DefaultRequestCulture.Culture.Name;
                    // Retrieves the requested culture
                    var rqf = context.Request.HttpContext.Features.Get<IRequestCultureFeature>();
                    // Culture contains the information of the requested culture
                    var culture = rqf.RequestCulture.Culture.Name ?? defaultCulture;

                    var path = context.GetRouteValue("path") ?? string.Empty;
                    var culturedPath = $"{context.Request.PathBase}/{culture}/{path}";
                    context.Response.Redirect(culturedPath);
                    return Task.CompletedTask;
                }));
            });
        }
    }
}
