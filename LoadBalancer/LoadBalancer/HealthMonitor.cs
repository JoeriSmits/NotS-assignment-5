using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace LoadBalancer
{
    internal static class HealthMonitor
    {
        public static ListBox serversLst;
        public static List<String[]> servers = new List<string[]>();

        /// <summary>
        /// Add the request to our list that keeps track of all the requests per server for health monitoring purposes.
        /// </summary>
        /// <param name="serverChosen">The server</param>
        public static void addConnection(int serverChosen)
        {
            foreach (var server in servers)
            {
                if (server[0] == (string) serversLst.Items[serverChosen])
                {
                    var requests = Int32.Parse(server[1]);
                    requests++;
                    server[1] = requests.ToString();
                }
            }

            foreach (var server in Algoritme.requestPerServer)
            {
                if (server[0] == (string)serversLst.Items[serverChosen])
                {
                    var requests = Int32.Parse(server[1]);
                    requests++;
                    server[1] = requests.ToString();
                }
            }

            // Check if the server is already in list. If not it will add the server
            var alreadyInList = false;
            foreach (var server in servers)
            {
                if (server[0] == (string) serversLst.Items[serverChosen])
                {
                    alreadyInList = true;
                }
            }
            if (!alreadyInList)
            {
                string[] stringArrayOfServer = { (string)serversLst.Items[serverChosen], "1" };
                Algoritme.requestPerServer.Add(stringArrayOfServer);
                servers.Add(stringArrayOfServer);
            }
        }
    }
}
