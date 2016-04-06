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


                try
                {
                    TcpClient client = new TcpClient(selectedServer[0], Int32.Parse(selectedServer[1]));
                    var stream = client.GetStream();

                    stream.Write(ASCIIEncoding.ASCII.GetBytes(requestPayload), 0, ASCIIEncoding.ASCII.GetBytes(requestPayload).Length);
                
                    while (stream.Read(responseBuffer, 0, responseBuffer.Length) != 0)
                    {
                        this._clientSocket.Send(responseBuffer);
                    }

                    foreach (var serverRequest in Algoritme.requestPerServer)
                    {
                        if (serverRequest[0] == selectedServer[0])
                        {
                            Algoritme.requestPerServer.Remove(serverRequest);
                        }
                    }

                    client.Close();
                    stream.Dispose();
                }
                catch (SocketException e)
                {
                    Console.Write("Server has fallen", e);
                    Handler();
                }

                this._clientSocket.Disconnect(false);
                this._clientSocket.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occured: " + e.Message);
            }
        }
    }
}