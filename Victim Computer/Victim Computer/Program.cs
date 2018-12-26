using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Victim_Computer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Victim";

            Victim v1 = new Victim(8080);
        }
    }
}

//Console.Write("enter port number: ");
//int port;
//try
//{
//    port = Convert.ToInt16(Console.ReadLine());
//    Victim v = new Victim(port);

//    v.StopServer();
//}
//catch (Exception e) {
//    Console.WriteLine(e.ToString());
//}

//Console.ReadLine();
