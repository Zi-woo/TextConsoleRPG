using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    public class BattleStartScreen
    {
            public void battleStar()
        {
            Console.WriteLine("Battle!!");
            Console.WriteLine();
            Console.WriteLine("Lv.2 미니언 HP 15");
            Console.WriteLine("Lv.5 대포미니언 HP 25");
            Console.WriteLine("Lv.3 공허충 HP 10");
            Console.WriteLine();
            Console.WriteLine("[내정보]");
            Console.WriteLine("HP 100/100");
            Console.WriteLine();
            Console.WriteLine("1. 공격");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.WriteLine(">> ");
        }
    }
}
