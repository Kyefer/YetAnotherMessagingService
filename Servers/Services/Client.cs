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


namespace WebApplication
{
    public class Client
    {
        private static ModelContext db = new ModelContext();
        public const int BufferSize = 4096;

        private string connectedUser;

        private WebSocket socket;

        Client(WebSocket socket)
        {
            this.socket = socket;
            Model.getInstance().NewClientMessageChange += RelayMessage;
        }

        private async Task WaitForMessages()
        {

            while (this.socket.State == WebSocketState.Open)
            {
                var buffer = new byte[BufferSize];
                var seg = new ArraySegment<byte>(buffer);
                var incoming = await this.socket.ReceiveAsync(seg, CancellationToken.None);

                var eventObj = JsonConvert.DeserializeObject<Event>(System.Text.Encoding.ASCII.GetString(seg.Array), new EventConverter());
                ProcessEvent(eventObj);
            }
        }

        private void ProcessEvent(Event message)
        {
            // ingore empty messages
            if (message == null)
                return;

            switch (message.type)
            {
                case EventType.NEW_MESSAGE:
                    NewMessage(message.data);
                    break;
                case EventType.AUTHENTICATION:
                    AuthenticateUser(message.data);
                    break;
                case EventType.REGISTER:
                    RegisterUser(message.data);
                    break;
                case EventType.MESSAGE_FETCH:
                    ReturnMessages();
                    break;
                default:
                    break;
            }
        }

        private void NewMessage(Message m)
        {
            m.sender = this.connectedUser;
            m.timestamp = DateTime.Now.Ticks;
            // db.Add(m);
            // db.SaveChanges();
            Model.getInstance().NewClientMessage(m);
        }

        private void RelayMessage(Message m)
        {
            m = TaggedUser(m);
            var e = new Event(EventType.MESSAGE_RELAY, m);
            SendEvent(e);
        }

        private async void ReturnMessages()
        {
            var messages = await db.Messages.ToListAsync();

            messages = messages.Select(m => TaggedUser(m)).ToList();

            SendEvent(new Event(EventType.MESSAGE_RETURN, messages));
        }

        private async void AuthenticateUser(Autentication auth)
        {

            var exists = await db.Users.AnyAsync(user => user.username == auth.username);
            if (exists)
            {
                var user = await db.Users.SingleAsync(u => u.username == auth.username);
                if (user.PasswordMatches(auth.password))
                {
                    this.connectedUser = auth.username;
                    var ev = new Event(EventType.AUTHENTICATION_RESULT, true);
                    SendEvent(ev);
                    return;
                }
            }

            var e = new Event(EventType.AUTHENTICATION_RESULT, false);
            SendEvent(e);

        }

        private async void RegisterUser(Autentication auth)
        {
            if (auth.password.Length < 8)
            {
                SendEvent(new Event(EventType.REGISTER_RESULT, new RegisterResult(false, "Password must be at least 8 characters")));
            }
            else
            {

                var exists = await db.Users.AnyAsync(user => user.username == auth.username);
                if (exists)
                {
                    SendEvent(new Event(EventType.REGISTER_RESULT, new RegisterResult(false, "Username is taken. Please choose another")));
                }
                else
                {
                    this.connectedUser = auth.username;
                    db.Add(new User(auth.username, auth.password));
                    var count = db.SaveChanges();
                    SendEvent(new Event(EventType.REGISTER_RESULT, new RegisterResult(true)));
                }

            }

        }


        private void SendEvent(Event e)
        {
            if (this.socket.State == WebSocketState.Open)
            {
                var outbuffer = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(e, Formatting.Indented));
                var outgoing = new ArraySegment<byte>(outbuffer, 0, outbuffer.Length);
                this.socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private Message TaggedUser(Message m)
        {
            if (Regex.IsMatch(m.content, ".*@" + connectedUser + "(\\s|$)"))
            {
                var tagged = new Message(m);
                tagged.tagged = true;
                return tagged;
            }
            return m;
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