using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft​.AspNetCore​.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebApplication.Models;


namespace WebApplication
{
    public class ProxyStartup
    {
        public ProxyStartup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddDbContext<ModelContext>(options =>
            //     options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app)
        {

            app.UseStaticFiles();

            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("slave", SlaveSelector.getSlavePort);

            app.UseRouter(routeBuilder.Build());
        }

        public class SlaveSelector
        {
            private static int last = -1;

            public static Task getSlavePort(HttpContext context)
            {
                int next = (last + 1) % Program.SlavePorts.Count;
                last = next;
                System.Console.WriteLine("Client connecting to slave on port {0}", Program.SlavePorts[next]);
                return context.Response.WriteAsync(Program.SlavePorts[next] + "");
            }


        }
    }


}
