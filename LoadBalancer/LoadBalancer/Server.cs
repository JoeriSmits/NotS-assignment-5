﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadBalancer
{
    internal class Server
    {
        private ListBox.ObjectCollection servers;

        public Server(ListBox.ObjectCollection servers)
        {
            this.servers = servers;
        }

        public string[] GetConnectionInfo(string cookie)
        {
            int serverChosen = 0;
            switch (Algoritme.Get())
            {
                case "Random":
                    Random rnd = new Random();
                    serverChosen = rnd.Next(0, servers.Count);
                    break;
                case "Round Robin":
                    serverChosen = Algoritme.roundRobinPos;
                    if (Algoritme.roundRobinPos != servers.Count - 1)
                    {
                        Algoritme.roundRobinPos++;
                    }
                    else
                    {
                        Algoritme.roundRobinPos = 0;
                    }
                    break;
                case "Load":
                    serverChosen = 0;
                    List<int> requestsServers = new List<int>();
                    foreach (var serverRequest in Algoritme.requestPerServer)
                    {
                        requestsServers.Add(Int32.Parse(serverRequest[1]));
                    }

                    var i = 0;
                    foreach (var requestsServer in requestsServers)
                    {
                        if (requestsServer == requestsServers.Min())
                        {
                            serverChosen = i;
                        }
                        i++;
                    }
                    break;
                case "Cookie Based":
                    var j = 0;
                    foreach (var server in servers)
                    {
                        if (server.ToString().Split('/')[2] == cookie)
                        {
                            serverChosen = j;
                        }
                        j++;
                    }
                    break;
            }
            HealthMonitor.addConnection(serverChosen);

            var connectInfo = GetHostAndPort(servers[serverChosen] as string);
            return connectInfo;
        }

        private string[] GetHostAndPort(string url)
        {
            string[] result = { url.Split('/')[2].Split(':')[0], url.Split('/')[2].Split(':')[1] };

            return result;
        }
    }
}
