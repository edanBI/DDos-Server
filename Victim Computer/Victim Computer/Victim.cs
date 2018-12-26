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
        public int Listen_Port { get; set; }

        private TcpListener listener;
        private ConcurrentDictionary<DateTime, int> connTimes;

        public Victim(int arg_port)
        {
            // create random 6 chars password
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            Password = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            Listen_Port = arg_port;

            StartServer();
        }

        private void StartServer()
        {
            listener = new TcpListener(IPAddress.Any, Listen_Port);
            listener.Start();
            Console.WriteLine("Server listening on port " + Listen_Port + ", password is " + Password);
        }

        private void AddBot(TcpClient botConn)
        {
            NetworkStream stream = new NetworkStream(botConn.Client);
            try
            {
                botConn.Client.Send(Encoding.UTF8.GetBytes("Please enter your password\r\n")); // asks the client to enter password
                byte[] passBuffer = new byte[16]; // 6 chars == 12B
                stream.Read(passBuffer, 0, passBuffer.Length);
                string inPassword = Encoding.UTF8.GetString(passBuffer);
                if (Password.Equals(inPassword))
                {
                    Console.WriteLine("Access granted");
                    DateTime time = DateTime.Now;
                    if (connTimes.ContainsKey(time))
                        connTimes[time]++;
                    else
                        connTimes.TryAdd(time, 1);
                    //connTimes.AddOrUpdate(time, (k, v) => v + 1);
                }
                else
                {
                    botConn.Close(); // close the connection
                }
            }
            catch (Exception e)
            { Console.WriteLine(e.Message); }
        }

        public void Listen()
        {
            while (true)
            {
                TcpClient botConn = listener.AcceptTcpClient(); // blocking call -- waiting for client(bot)
                //ThreadStart ts = new ThreadStart(AddBot);
                //Thread botThread = new Thread(new ThreadStart(AddBot));
                //botThread.Start(botConn);
            }
        }

        override
        public String ToString()
        {
            return "Victim: port=" + Listen_Port + ", password=" + Password;
        }
    }
}
