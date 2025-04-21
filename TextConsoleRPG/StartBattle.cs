using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{

    internal class StartBattle
    {
        

        Console.Clear();
        Console.WriteLine("Battle!!");
        Console.WriteLine();
        Console.WriteLine($"{Monster}");
        Console.WriteLine($"{Monster}");
        Console.WriteLine($"{Monster}");
        Console.WriteLine();
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{Level} {UserName} ({Class})");
        Console.WriteLine($"HP 100/{HpDate}");
        Console.WriteLine();
        Console.WriteLine("1. 공격");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");
        Console.WriteLine(">> ");
    }
}
