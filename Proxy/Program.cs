using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebApplication
{
    public class Program
    {

        public static HashSet<int> ServerPorts;
        public static void Main(string[] args)
        {

            var config = new Configuration("servers.conf");
            Console.WriteLine("Running proxy server on port {0}", config.ProxyPort);

            ServerPorts = config.ServerPorts;

            Console.WriteLine("Starting servers...");
            Parallel.ForEach(config.ServerPorts, port => {
                System.Console.WriteLine("Starting new server on port {0}", port);
            });
            System.Console.WriteLine("Finished starting servers");

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:" + config.ProxyPort)
                .Build();

            host.Run();




        }
    }
}
