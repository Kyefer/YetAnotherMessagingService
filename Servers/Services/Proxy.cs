using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft​.AspNetCore​.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace WebApplication
{
    public class ProxyStartup
    {

        public void ConfigureServices(IServiceCollection services)
        {
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
