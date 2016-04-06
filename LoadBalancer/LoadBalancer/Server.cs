using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LoadBalancer
{
    internal class Server
    {
        private ListBox.ObjectCollection servers;

        /// <summary>
        /// Server constructor
        /// Set's the server propery to the one give in the constructor
        /// </summary>
        /// <param name="servers"></param>
        public Server(ListBox.ObjectCollection servers)
        {
            this.servers = servers;
        }

        /// <summary>
        /// Determine what server the server class has to connect to.
        /// It takes the algorithm and based on that it will choose the server
        /// </summary>
        /// <param name="cookie">Cookie if there is any otherwise it is null</param>
        /// <param name="session">Session if there is any otherwise it is null</param>
        /// <returns>Server object with connect information</returns>
        public string[] GetConnectionInfo(string cookie, string session)
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
                    // Chooses the server with the less Load. What is based on the current amount of connections to a server.
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
                case "Session Based":
                    var sessions = Algoritme.sessionsPerServer;

                    // If there is no session set yet it will choose a server based on Round Robin
                    if (session == null)
                    {
                        serverChosen = Algoritme.roundRobinPos;
                        if (Algoritme.roundRobinPos != servers.Count - 1)
                        {
                            Algoritme.roundRobinPos++;
                        }
                        else
                        {
                            Algoritme.roundRobinPos = 0;
                        }
                    }
                    // There is a session
                    else
                    {
                        // Check if the session is already stored in our list of stored sessions
                        var isAlreadyStored = false;
                        object selectedServer = null;

                        foreach (var stringse in Algoritme.sessionsPerServer)
                        {
                            if (session == stringse[0])
                            {
                                isAlreadyStored = true;
                                selectedServer = stringse[1];
                            }
                        }

                        if (isAlreadyStored)
                        {
                            // The session is already stored
                            var l = 0;
                            foreach (var server in servers)
                            {
                                if (server == selectedServer)
                                {
                                    serverChosen = l;
                                }
                                l++;
                            }
                        }
                        else
                        {
                            // Session is not stored yet
                            // Add new session id to the local storage
                            var k = 0;
                            string[] sessionPerServer = null;
                            if (Algoritme.roundRobinPos != 0)
                            {
                                Algoritme.roundRobinPos--;
                            }
                            else
                            {
                                Algoritme.roundRobinPos = servers.Count - 1;
                            }
                        
                            foreach (var server in servers)
                            {
                                if (k == Algoritme.roundRobinPos)
                                {
                                    sessionPerServer = new string[] {session, server.ToString()};
                                }
                                k++;
                            }
                            serverChosen = Algoritme.roundRobinPos;
                            Algoritme.sessionsPerServer.Add(sessionPerServer); 
                        }

                    }
                    break;
            }
            // Keep track of the requests for health monitoring
            HealthMonitor.addConnection(serverChosen);

            // Return the connectInfo to the server class
            var connectInfo = GetHostAndPort(servers[serverChosen] as string);
            return connectInfo;
        }

        /// <summary>
        /// Parse a string that is a valid url to seperate the host and the port
        /// </summary>
        /// <param name="url">A valid URL</param>
        /// <returns>string array with the host and the port of the input url</returns>
        private string[] GetHostAndPort(string url)
        {
            string[] result = { url.Split('/')[2].Split(':')[0], url.Split('/')[2].Split(':')[1] };
            return result;
        }
    }
}
