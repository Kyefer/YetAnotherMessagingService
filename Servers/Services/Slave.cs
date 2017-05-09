using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Models;
using System.Threading.Tasks;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;

namespace WebApplication
{
    public class SlaveStartup
    {
        private static Slave slave;
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        public async void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime)
        {
            app.Map("/ws", Client.Map);

            slave = new Slave(Program.Port);
            await slave.Start();
            applicationLifetime.ApplicationStopping.Register(slave.Close);
        }
    }

    public class Slave
    {
        private UdpClient Server;
        private int port;

        public Slave(int port)
        {
            this.port = port;
        }

        public async Task Start()
        {
            this.Server = new UdpClient(port);
            Model.getInstance().SendToAllServersChange += RelayMessage;
            await this.WaitForMessages();
        }

        private void RelayMessage(Message m)
        {
            var text = JsonConvert.SerializeObject(m, Formatting.Indented);
            var bytes = Encoding.ASCII.GetBytes(text);
            System.Console.WriteLine("SENDING MESSAGE TO ALL SLAVES");
            System.Console.WriteLine(text);
            Parallel.ForEach(Program.SlavePorts, p =>
            {
                if (this.port != p)
                {
                    System.Console.WriteLine("SENDING MESSAGE TO SLAVE AT PORT " + p);
                    this.Server.SendAsync(bytes, bytes.Length, "localhost", p);
                }

            });
        }

        public async Task WaitForMessages()
        {
            while (true)
            {
                var result = await this.Server.ReceiveAsync();
                var text = Encoding.ASCII.GetString(result.Buffer);
                var message = JsonConvert.DeserializeObject<Message>(text);
                if (message != null)
                {
                    System.Console.WriteLine("RECEIVED MESSAGE FROM ANOTHER SLAVE");
                    System.Console.WriteLine(text);
                    Model.getInstance().NewServerMessage(message);
                }
            }
        }

        public void Close()
        {
            this.Server.Dispose();
        }
    }
}
