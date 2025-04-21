using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    class Monster
    {
        public string Name { get; }
        public int Level { get; }
        public int Hp { get; private set;}
        public int Atk { get; }

        private List<Monster> MonsterList = new List<Monster>();

        public Monster(string name, int level, int hp, int atk)
        {
            Name = name;
            Level = level;
            Hp = hp;
            Atk = atk;
        }
        public string MonsterInfoText()
        {
            return $"Lv.{Level} {Name} HP {Hp}";
        }

        
        public bool AliveMonster() //몬스터 생존 확인 
        {
            return Hp > 0;
        }
        public void Damage(int damage)//몬스터 체력 감소
        {
            Hp -= damage;
            if (Hp < 0) Hp = 0;
            Console.WriteLine($"{damage} 데미지를 입혔다!");
        }
    }
}
