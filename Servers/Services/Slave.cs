using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication.Models;
using System.Collections.Generic;
using Microsoft​.AspNetCore​.Routing;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;

namespace WebApplication
{
    public class SlaveSetup
    {
        public SlaveSetup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ModelContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();

            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.Map("/ws", Client.Map);

            // var selector = new SlaveSelector(new List<int>(Program.ServerPorts));

            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("slave", SlaveSelector.getSlavePort);

            app.UseRouter(routeBuilder.Build());


        }
    }


    public class Slave
    {

        private UdpClient Server;
        private int port;

        public Slave(int port)
        {
            this.port = port;
            this.Server = new UdpClient(port);
            Model.getInstance().SendToAllServersChange += RelayMessage;
        }

        private void RelayMessage(Message m)
        {   
            var text = JsonConvert.SerializeObject(m, Formatting.Indented);
            var bytes = Encoding.ASCII.GetBytes(text);
            System.Console.WriteLine("SENDING MESSAGE TO ALL SLAVES");
            System.Console.WriteLine(text);
            Parallel.ForEach(Program.ServerPorts, p =>
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
                System.Console.WriteLine("RECEIVED MESSAGE FROM ANOTHER SLAVE");
                System.Console.WriteLine(text);
                Model.getInstance().NewServerMessage(message);
            }
        }

        public void close()
        {
            this.Server.Dispose();
        }
    }
}
