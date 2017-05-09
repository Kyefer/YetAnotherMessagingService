using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;

namespace WebApplication
{
    public class Configuration
    {

        public int ProxyPort { get; private set; }
        public List<int> ServerPorts { get; private set; }

        public Configuration(string configPath)
        {
            this.ProxyPort = 0;
            this.ServerPorts = new List<int>();

            var lines = File.ReadAllLines(configPath);

            foreach (var line in lines)
            {
                var text = line.Trim();
                if (!text.StartsWith("#"))
                {
                    if (Regex.IsMatch(text, "Proxy:\\s*\\d+"))
                    {
                        int val = 0;
                        Int32.TryParse(Regex.Match(text, "\\d+").Value, out val);
                        this.ProxyPort = val;
                    }
                    else if (Regex.IsMatch(text, "Servers: \\d+(,\\s*\\d+)*"))
                    {
                        foreach (Match match in Regex.Matches(text, "\\d+"))
                        {
                            int port = 0;
                            Int32.TryParse(match.Value, out port);
                            if (port > 0)
                                ServerPorts.Add(port);
                        }
                    }
                }
            }

        }
    }
}