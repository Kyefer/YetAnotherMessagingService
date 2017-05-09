using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Sockets;

namespace WebApplication
{
    public class Program
    {

        public static List<int> ServerPorts;
        public static void Main(string[] args)
        {
            string type = args[0] = args[0].ToLower();
            var config = new Configuration("servers.conf");
            ServerPorts = config.ServerPorts;

            if (type == "proxy")
            {
                Console.WriteLine("Running proxy server on port {0}", config.ProxyPort);
                Console.WriteLine("Starting servers...");
                Parallel.ForEach(config.ServerPorts, port =>
                {
                    System.Console.WriteLine("Starting new slave on port {0}", port);
                    startSlave(port);
                });
                System.Console.WriteLine("Finished starting slaves");

                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<ProxyStartup>()
                    .UseUrls("http://localhost:" + config.ProxyPort)
                    .Build();

                host.Run();
            }
            else if (type == "slave")
            {
                int port = Int32.Parse(args[1]);
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseStartup<ProxyStartup>()
                    .UseUrls("http://localhost:" + port)
                    .Build();
                startUdp(port);
                host.Run();
            }
            else
            {
                System.Console.WriteLine("Unknown server type");
            }

        }

        private static void startSlave(int port)
        {

        }

        private static async void startUdp(int port)
        {
            await new Slave(port).WaitForMessages();
            
        }
    }
}
