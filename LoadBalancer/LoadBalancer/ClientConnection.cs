using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LoadBalancer
{
    internal class ClientConnection
    {
        private readonly Socket _clientSocket;
        private Server server;

        public ClientConnection(Socket client, ListBox serversLst)
        {
            this._clientSocket = client;

            server = new Server(serversLst.Items);
        }

        public void StartHandling()
        {
            var t = new Thread(Handler) { Priority = ThreadPriority.AboveNormal };
            t.Start();
        }

        private void Handler()
        {
            bool recvRequest = true;
            string EOL = "\r\n";

            string requestPayload = "";
            string requestTempLine = "";
            List<string> requestLines = new List<string>();
            byte[] requestBuffer = new byte[1];
            byte[] responseBuffer = new byte[1];

            requestLines.Clear();

            try
            {
                //State 0: Handle Request from Client
                while (recvRequest)
                {
                    this._clientSocket.Receive(requestBuffer);
                    string fromByte = ASCIIEncoding.ASCII.GetString(requestBuffer);
                    requestPayload += fromByte;
                    requestTempLine += fromByte;

                    if (requestTempLine.EndsWith(EOL))
                    {
                        requestLines.Add(requestTempLine.Trim());
                        requestTempLine = "";
                    }

                    if (requestPayload.EndsWith(EOL + EOL))
                    {
                        recvRequest = false;
                    }
                }
                Console.WriteLine("Raw Request Received...");
                Console.WriteLine(requestPayload);

                requestPayload = "";
                foreach (string line in requestLines)
                {
                    requestPayload += line;
                    requestPayload += EOL;
                }

                var selectedServer = server.GetConnectionInfo();

                Socket destServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                destServerSocket.Connect((selectedServer[0]), Int32.Parse(selectedServer[1]));

                //State 2: Sending New Request Information to Destination Server and Relay Response to Client
                destServerSocket.Send(ASCIIEncoding.ASCII.GetBytes(requestPayload));
                
                //Console.WriteLine("Begin Receiving Response...");
                while (destServerSocket.Receive(responseBuffer) != 0)
                {
                    //Console.Write(ASCIIEncoding.ASCII.GetString(responseBuffer));
                    this._clientSocket.Send(responseBuffer);
                }

                destServerSocket.Disconnect(false);
                destServerSocket.Dispose();
                this._clientSocket.Disconnect(false);
                this._clientSocket.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occured: " + e.Message);
                //Console.WriteLine(e.StackTrace);
            }
        }
    }
}