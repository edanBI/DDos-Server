using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace Bot_Computer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Bot";

            //Bot bot = new Bot();
            //bot.SendAnnouncement();

            TcpClient client = new TcpClient("192.168.1.20", 47650); // bot is now connected on this ip in this port
            Socket socket = client.Client; // gets the client's socket
            byte[] data = Encoding.UTF8.GetBytes("HELLO FROM CLIENT!");
            socket.Send(data);

            while (true)
            {
                byte[] buff = new byte[128];
                int i = socket.Receive(buff);
                Console.WriteLine(Encoding.UTF8.GetString(buff));
                string in_pass = Console.ReadLine();
                socket.Send(Encoding.UTF8.GetBytes(in_pass));
            }
        }
    }
}
