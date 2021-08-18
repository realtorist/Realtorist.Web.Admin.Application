using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Realtorist.Web.Admin.Application
{
    public class Startup : IConfigureServicesAction, IConfigureAction
    {
        public int Priority => 3;

        void IConfigureServicesAction.Execute(IServiceCollection services, IServiceProvider serviceProvider)
        {
            var env = serviceProvider.GetService<IWebHostEnvironment>();

            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            var fileProvider = new PhysicalFileProvider(assemblyDirectory + "/wwwroot");
            env.WebRootFileProvider = new CompositeFileProvider(fileProvider, env.WebRootFileProvider);
        }

        void IConfigureAction.Execute(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var env = serviceProvider.GetService<IWebHostEnvironment>();
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value) && (context.Request.Path.Value.StartsWith("/admin") || context.Request.Path.Value.StartsWith("admin")))
                {
                    context.Request.Path = "/admin/index.html";
                    await next();
                }
            })
            .UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" } });
        }
    }
}
