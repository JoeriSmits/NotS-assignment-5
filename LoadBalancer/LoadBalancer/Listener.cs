using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadBalancer
{
    class Listener
    {
        private readonly int _listenPort;
        private readonly TcpListener listener;
        private ListBox serversLst;

        /// <summary>
        /// Constructor listener
        /// Set's the port of the loadbalancer and passes a printTextDelegate to output text
        /// </summary>
        /// <param name="port">Port of the proxy</param>
        /// <param name="serversLst"></param>
        /// <param name="printTextDelegate">delegate to output text</param>
        public Listener(int port, ListBox serversLst)
        {
            this._listenPort = port;
            this.listener = new TcpListener(IPAddress.Any, this._listenPort);
            this.serversLst = serversLst;
        }

        /// <summary>
        /// Start the loadbalancer
        /// </summary>
        public void StartListener()
        {
            try
            {
                this.listener.Start();

                var t = new Thread(delegate ()
                {
                    // Infinite loop to accept connections when one connection occures
                    while (true)
                    {
                        this.AcceptConnection();
                    }
                });
                t.Start();
            }
            catch (SocketException)
            {
                Console.WriteLine("There is already a LoadBalancer running on port: " + this._listenPort);
            }
        }

        /// <summary>
        /// Accept a connection from a client we start a clientConnection once the socket has been accepted
        /// </summary>
        public void AcceptConnection()
        {
            var newClient = this.listener.AcceptSocket();
            var client = new ClientConnection(newClient, serversLst);
            client.StartHandling();
        }
    }
}
