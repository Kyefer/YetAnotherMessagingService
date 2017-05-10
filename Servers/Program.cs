using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;

namespace WebApplication
{
    public class Program
    {

        public static List<int> SlavePorts= new List<int>();
        public static int Port;
        public static void Main(string[] args)
        {

            if (args.Length <= 1)
            {
                PrintUsage();
                return;
            }
            var type = args[0];
            if (args.Length == 1 || !Regex.IsMatch(args[1], "\\d+"))
            {
                PrintUsage();
                return;
            }

            Port = Int32.Parse(args[1]);

            for (int i = 2; i < args.Length; i++)
            {
                if (Regex.IsMatch(args[i], "\\d+"))
                    SlavePorts.Add(Int32.Parse(args[i]));
            }
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls("http://localhost:" + Port);

            if (type == "proxy")
            {
                host.UseStartup<ProxyStartup>();
            }
            else if (type == "slave")
            {
                host.UseStartup<SlaveStartup>();
            }
            else
            {
                PrintUsage();
                return;
            }

            host.Build().Run();
        }

        private static void PrintUsage()
        {
            System.Console.WriteLine("To run the proxy server:");
            System.Console.WriteLine("dotnet run proxy <port> <slave port> <slave port> ...");
            System.Console.WriteLine("To run a slave server:");
            System.Console.WriteLine("dotnet run slave <port> <other slave port> <other slave port> ... ");

        }
    }
}
