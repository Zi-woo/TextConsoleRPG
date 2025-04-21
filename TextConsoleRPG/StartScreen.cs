using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    public class StartScreen
    {
        public StartScreen()
        {
        }
        public void Run()
        {
            string userChoice;
            
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("이제 전투를 시작할 수 있습니다.\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 전투 시작\n");


            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                userChoice = Console.ReadLine();
                if (userChoice == "1" || userChoice == "2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                }

            }


            switch (userChoice)
            {
                case "1":
                    Console.WriteLine("상태 보기 함수 실행");
                    break;
                case "2":
                    Console.WriteLine("전투 시작 함수 실행");
                    break;
            }
        }
    }


}
