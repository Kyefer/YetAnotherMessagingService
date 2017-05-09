using System;
using System.Text.RegularExpressions;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WebApplication.Models;
using Newtonsoft.Json;


namespace WebApplication
{
    public class Client
    {
        public const int BufferSize = 4096;

        private WebSocket socket;

        Client(WebSocket socket)
        {
            this.socket = socket;
            Model.getInstance().NewClientMessageChange += SendMessage;
        }

        private async Task WaitForMessages()
        {

            while (this.socket.State == WebSocketState.Open)
            {
                var buffer = new byte[BufferSize];
                var seg = new ArraySegment<byte>(buffer);
                var incoming = await this.socket.ReceiveAsync(seg, CancellationToken.None);

                var message = JsonConvert.DeserializeObject<Message>(System.Text.Encoding.ASCII.GetString(seg.Array));
                if (message != null)
                {
                    Model.getInstance().NewClientMessage(message);
                }
            }
        }

        private void SendMessage(Message e)
        {
            if (this.socket.State == WebSocketState.Open)
            {
                var outbuffer = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(e, Formatting.Indented));
                var outgoing = new ArraySegment<byte>(outbuffer, 0, outbuffer.Length);
                this.socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;

            var socket = await hc.WebSockets.AcceptWebSocketAsync();
            var client = new Client(socket);
            await client.WaitForMessages();
        }

        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(Client.Acceptor);
        }
    }



}