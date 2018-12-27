using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Command_and_Control
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Command and Control";

            CnCserver cc = new CnCserver();

            string inn;
            while (true)
            {
                if ((inn = Console.ReadLine()).Equals("a"))
                {
                    cc.AttackVictim();
                }
            }
        }
    }
}
