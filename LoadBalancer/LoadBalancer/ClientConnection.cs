using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
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

                string cookie = null;
                requestPayload = "";
                foreach (string line in requestLines)
                {
                    var addLines = true;

                    if (line.Contains("Accept-Encoding"))
                    {
                        addLines = false;
                    }

                    if (line.Contains("Cookie"))
                    {
                        cookie = line.Split('=')[1];
                    }

                    if (addLines)
                    {
                        requestPayload += line;
                        requestPayload += EOL;
                    }
                }

                var selectedServer = server.GetConnectionInfo(cookie);

                try
                {
                    TcpClient client = new TcpClient(selectedServer[0], Int32.Parse(selectedServer[1]));
                    var stream = client.GetStream();

                    stream.Write(ASCIIEncoding.ASCII.GetBytes(requestPayload), 0, ASCIIEncoding.ASCII.GetBytes(requestPayload).Length);


                    if (Algoritme.Get() == "Cookie Based")
                    {
                        string response = null;
                        var contentLengthIsAvailable = false;
                        var headersInThePocket = false;
                        int contentLength = 0;


                        StringBuilder responseBytes = null;
                        var i = 0;
                        while (true)
                        {
                            stream.Read(responseBuffer, 0, responseBuffer.Length);

                            response += Encoding.ASCII.GetString(responseBuffer);

                            if (response.EndsWith(EOL + EOL))
                            {
                                response = response.Split(new[] { EOL + EOL }, StringSplitOptions.None)[0];
                                var connectionCookie = selectedServer[0] + ":" + selectedServer[1];
                                response += EOL + "Set-Cookie: fixed-server=" + Algoritme.CalculateMD5Hash(connectionCookie) + EOL + EOL;

                                var headers = response.Split(':');

                                var contentLengthAvailable = false;
                                foreach (var header in headers)
                                {
                                    if (contentLengthAvailable)
                                    {
                                        contentLength = int.Parse(header.Split('\r')[0]);
                                        contentLengthAvailable = false;
                                    }

                                    if (header.EndsWith("Content-Length"))
                                    {
                                        contentLengthAvailable = true;
                                    }
                                }
                                responseBytes = new StringBuilder(response);
                                headersInThePocket = true;
                            }

                            if (headersInThePocket)
                            {
                                if (contentLength == 0)
                                {
                                    contentLength = 2000;
                                }
                                if (i < contentLength)
                                {
                                    responseBytes.Append(Encoding.ASCII.GetString(responseBuffer));
                                    i++;
                                }
                                else
                                {
                                    var charResponse = responseBytes.ToString().ToCharArray(0, responseBytes.Length);
                                    byte[] test = Encoding.ASCII.GetBytes(charResponse);

                                    _clientSocket.Send(test);
                                    break;  
                                }

                            }
                        }
                    }
                    else
                    {
                        while (stream.Read(responseBuffer, 0, responseBuffer.Length) != 0)
                        {
                            this._clientSocket.Send(responseBuffer);
                        }
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