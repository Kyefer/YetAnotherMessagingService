using System;
using System.Text.RegularExpressions;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebApplication.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Concurrent;

namespace WebApplication
{
    public class Server
    {
        private static ConcurrentBag<Server> Servers = new ConcurrentBag<Server>();


        private WebSocket socket;
        public Server(WebSocket socket)
        {
            this.socket = socket;
        }

        private async Task SendPorts()
        {
            if (this.socket.State == WebSocketState.Open)
            {
                var outbuffer = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Program.ServerPorts, Formatting.Indented));
                var outgoing = new ArraySegment<byte>(outbuffer, 0, outbuffer.Length);
                await this.socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private async Task WaitForAll()
        {
            while (Servers.Count != Program.ServerPorts.Count)
            {

            }

            await SendPorts();
        }

        static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
            {
                return;
            }

            var socket = await hc.WebSockets.AcceptWebSocketAsync();

            System.Console.WriteLine("New Server Connecting");
            var server = new Server(socket);
            Servers.Add(server);

            await server.WaitForAll();
        }
        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(Server.Acceptor);
        }
    }
}