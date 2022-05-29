using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using XrefViewer.Core.Network;

namespace XrefViewerUI.Core.Network
{
    public static class NetworkManager
    {
        public static Socket CurrentSocket;
        public static event Action<XrefPacket> DataReceived;

        private static string data = null;

        public static void Init()
        {
            byte[] bytes = new byte[1024];
            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, 12000);

                CurrentSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    Task.Run(async () =>
                    {
                        while (true)
                        {
                            CurrentSocket.Connect(remoteEndPoint);
                            Console.WriteLine("Socket connected to " + CurrentSocket.RemoteEndPoint.ToString());

                            while (true)
                            {
                                while (true)
                                {
                                    int bytesReceived = CurrentSocket.Receive(bytes);
                                    data += Encoding.ASCII.GetString(bytes, 0, bytesReceived);

                                    int endOfFile = data.IndexOf("<EOF>");
                                    if (endOfFile > -1)
                                    {
                                        string packetData = data.Substring(0, endOfFile);
                                        XrefPacket packet = XrefPacket.FromJson(packetData);
                                        Application.Current.Dispatcher.Invoke(() => DataReceived?.Invoke(packet));
                                        data = data.Substring(endOfFile + 5);
                                        break;
                                    }
                                }
                            }
                        }
                    }).ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                            Console.Error.WriteLine(t.Exception);
                    });
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void SendData(CommandPacket packet)
        {
            if (CurrentSocket != null && CurrentSocket.Connected)
                CurrentSocket.Send(Encoding.ASCII.GetBytes(packet.ToJson() + "<EOF>"));
        }
    }
}
