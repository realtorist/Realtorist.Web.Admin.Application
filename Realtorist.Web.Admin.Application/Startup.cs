using AutoMapper;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Realtorist.Extensions.Base;
using Realtorist.Web.Admin.Application.Middleware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Realtorist.Web.Admin.Application
{
    public class Startup : IConfigureServicesExtension, IConfigureApplicationExtension, IConfigureAutoMapperProfileExtension
    {
        public int Priority => 3;

        public void ConfigureServices(IServiceCollection services, IServiceProvider serviceProvider)
        {
            var env = serviceProvider.GetService<IWebHostEnvironment>();

            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            var fileProvider = new PhysicalFileProvider(assemblyDirectory + "/wwwroot");
            env.WebRootFileProvider = new CompositeFileProvider(fileProvider, env.WebRootFileProvider);
        }

        public void ConfigureApplication(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseRouting();
            app.UseMiddleware<AuthMiddleware>();

            var env = serviceProvider.GetService<IWebHostEnvironment>();
            var logger = serviceProvider.GetService<ILogger<Startup>>();
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value) && (context.Request.Path.Value.StartsWith("/admin") || context.Request.Path.Value.StartsWith("admin")))
                {
                    logger.LogDebug($"Request '{context.Request.Path.Value}' with 404 status was hit. Redirection to admin panel");
                    context.Request.Path = "/admin/index.html";
                    await next();
                }
            })
            .UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" } });
        }

        public IEnumerable<Profile> GetAutoMapperProfiles()
        {
            yield return new AutoMapperProfile();
        }
    }
}
