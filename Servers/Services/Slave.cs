using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using WebApplication.Models;
using System.Threading.Tasks;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System;

namespace WebApplication
{
    public class SlaveStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime)
        {
            app.Map("/ws", Client.Map);

            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapPost("msg", Slave.ReceiveMessage);

            app.UseRouter(routeBuilder.Build());

            Model.getInstance().SendToAllServersChange += Slave.RelayMessage;

            // slave = new Slave(Program.Port);
            // await slave.Start();
            // applicationLifetime.ApplicationStopping.Register(slave.Close);
        }
    }

    public class Slave
    {

        public static async Task ReceiveMessage(HttpContext context)
        {
            var reader = new StreamReader(context.Request.Body);
            var text = await reader.ReadToEndAsync();
            var msg = JsonConvert.DeserializeObject<Message>(text);
            reader.Dispose();
            if (msg != null)
            {
                Console.WriteLine("RECEIVED MESSAGE FROM ANOTHER SLAVE");
                Console.WriteLine(text);
                Model.getInstance().NewServerMessage(msg);
            }
        }

        public static void RelayMessage(Message m)
        {
            var text = JsonConvert.SerializeObject(m, Formatting.Indented);
            System.Console.WriteLine("SENDING MESSAGE TO ALL SLAVES");
            System.Console.WriteLine(text);
            Parallel.ForEach(Program.SlavePorts, async p =>
            {
                if (Program.Port != p)
                {
                    try
                    {
                        var request = WebRequest.Create("http://localhost:" + p + "/msg");

                        System.Console.WriteLine("SENDING MESSAGE TO SLAVE AT PORT " + p);
                        request.Method = "POST";
                        var writer = new StreamWriter(await request.GetRequestStreamAsync());
                        writer.Write(text);
                        writer.Flush();
                        await request.GetResponseAsync();
                        writer.Dispose();
                    } catch
                    {
                        System.Console.WriteLine("UNABLE TO REACH SERVER AT PORT {0}", p);
                    }
                }

            });
        }
    }
}
