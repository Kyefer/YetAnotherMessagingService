using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.WebSockets;
using System.Threading;
using Newtonsoft.Json;

namespace WebApplication
{
    public class Program
    {

        public static HashSet<int> ServerPorts;
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("MISSING TWO ARGUMENTS");
                return;
            }
            int proxyPort = Int32.Parse(args[0]);
            int currentPort = Int32.Parse(args[1]);

            Connect(proxyPort, currentPort);

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:" + currentPort)
                .Build();

            host.Run();

        }

        public static async void Connect(int proxyPort, int currentPort)
        {
            var socket = new ClientWebSocket();
            await socket.ConnectAsync(new Uri("ws://localhost:" + proxyPort + "/register"), CancellationToken.None);
            var buffer = new byte[2048];
            var seg = new ArraySegment<byte>(buffer);
            var incoming = await socket.ReceiveAsync(seg, CancellationToken.None);

            var ports = JsonConvert.DeserializeObject<List<int>>(System.Text.Encoding.ASCII.GetString(seg.Array));
            ports.Remove(currentPort);

            Parallel.ForEach(ports, async port =>
            {
                var other = new ClientWebSocket();
                await other.ConnectAsync(new Uri("ws://localhost:" + port + "/server"), CancellationToken.None);
                System.Console.WriteLine("Connected to: " + port);

                var server = new Server(other, port);
                await server.WaitForRelays();

            });
        }
    }
}
