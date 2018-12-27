using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Victim_Computer
{
    class Victim
    {
        public String Password { get; }
        public int Listen_Port { get; }

        private TcpListener listener;
        private ConcurrentDictionary<String, int> connTimes;

        public Victim()
        {
            // create random 6 chars password
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            Password = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            // create random port number
            Listen_Port = random.Next(1025, 65535);
            connTimes = new ConcurrentDictionary<String, int>();

            StartServer();
        }

        private void StartServer()
        {
            listener = new TcpListener(IPAddress.Any, Listen_Port);
            listener.Start();
            Console.WriteLine("Server listening on port " + Listen_Port + ", password is " + Password);
            Console.WriteLine(listener.Server.LocalEndPoint.ToString());

            new Thread(ListenTcpClient).Start();
            //ListenTcpClient();
        }

        private void ListenTcpClient()
        {
            try
            {
                while (true)
                {
                    // send enter password message
                    Console.WriteLine();
                    TcpClient botConn = listener.AcceptTcpClient(); // blocking call -- waiting for client(bot)

                    byte[] enterMsg = Encoding.UTF8.GetBytes("Please enter your password\r\n");
                    botConn.Client.Send(enterMsg);

                    byte[] passBuffer = new byte[6];
                    botConn.Client.Receive(passBuffer);
                    string bot_password = Encoding.UTF8.GetString(passBuffer);
                    if (Password.Equals(bot_password))
                    {
                        Console.WriteLine("Access granted");
                        string time = DateTime.Now.ToString();
                        Console.WriteLine(time);
                        if (connTimes.ContainsKey(time))
                            connTimes[time]++;
                        else
                            connTimes.TryAdd(time, 1);

                        //check if there were 10 bots in the last second
                        if(connTimes[time] >= 10)
                        {
                            byte[] hackMsg = new byte[100];
                            botConn.Client.Receive(hackMsg);
                            string msg = Encoding.UTF8.GetString(hackMsg);
                            Console.WriteLine(msg);
                        }
                        botConn.Close();
                    }
                    else
                    {
                        Console.WriteLine(Password + " != " + bot_password);
                        botConn.Close();
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace.ToString());
                Console.WriteLine(e.Message);
            }
        }
    }
}
