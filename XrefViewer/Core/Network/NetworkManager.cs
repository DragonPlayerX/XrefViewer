using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XrefViewer.Core.Network
{
    public static class NetworkManager
    {
        public static event Action<Connection, CommandPacket> DataReceived;
        public static List<Connection> Connections;

        public static void Init()
        {
            Connections = new List<Connection>();

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 12000);

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                Task.Run(async () =>
                {
                    while (true)
                    {
                        try
                        {
                            XrefViewerMod.Logger.Msg("Waiting for a client connection...");

                            Socket handler = await listener.AcceptAsync();
                            Connections.Add(new Connection(handler));

                            XrefViewerMod.Logger.Msg("Client connected from " + handler.RemoteEndPoint.ToString());
                        }
                        catch { }
                    }
                }).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                        XrefViewerMod.Logger.Error(t.Exception);
                });

            }
            catch (Exception e)
            {
                XrefViewerMod.Logger.Error(e);
            }
        }

        public static void SendData(Connection connection, XrefPacket packet)
        {
            if (connection.Handler != null && connection.Handler.Connected)
                connection.Handler.Send(Encoding.ASCII.GetBytes(packet.ToJson() + "<EOF>"));
        }

        public class Connection
        {
            public Socket Handler;
            public Task WorkingTask;
            public CancellationTokenSource CancellationTokenSource;
            private byte[] buffer = new byte[4096];
            private string jsonBuffer;

            public Connection(Socket handler)
            {
                Handler = handler;
                CancellationTokenSource = new CancellationTokenSource();
                jsonBuffer = null;

                WorkingTask = Task.Run(async () =>
                {
                    while (!CancellationTokenSource.Token.IsCancellationRequested)
                    {
                        while (!CancellationTokenSource.Token.IsCancellationRequested)
                        {
                            int bytesReceived = Handler.Receive(buffer);
                            jsonBuffer += Encoding.ASCII.GetString(buffer, 0, bytesReceived);

                            int endOfFile = jsonBuffer.IndexOf("<EOF>");
                            if (endOfFile > -1)
                            {
                                string packetData = jsonBuffer.Substring(0, endOfFile);
                                CommandPacket packet = CommandPacket.FromJson(packetData);
                                DataReceived?.Invoke(this, packet);
                                jsonBuffer = jsonBuffer.Substring(endOfFile + 5);
                                break;
                            }
                        }
                    }
                }, CancellationTokenSource.Token);
            }
        }
    }
}
