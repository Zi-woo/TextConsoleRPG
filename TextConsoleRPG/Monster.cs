using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    class Monster
    {
        public string Name { get; }
        public int Level { get; }
        public int Hp { get; private set;}
        public int Atk { get; }
        public int Exp { get; }
        
        public List<Item> DropItemList { get; }
        public Monster(string name, int level, int hp, int atk, int exp, List<Item> dropItemList)
        {
            Name = name;
            Level = level;
            Hp = hp;
            Atk = atk;
            Exp = exp;
            DropItemList = dropItemList;
        }
        
        public bool AliveMonster() //몬스터 생존 확인 
        {
            return Hp > 0;
        }

        public void DamageByPlayer(int damage)//몬스터 체력 감소
        {
            Hp -= damage;
            if (Hp < 0) Hp = 0;
            Console.WriteLine($"{damage} 데미지를 입혔다!");
        }
        public Item ItemDrop()
        {
            Random random = new Random();
            int chance = random.Next(0, 100);
            if (chance > 30 && DropItemList.Count>0)
            {
                int index = random.Next(DropItemList.Count);
                return DropItemList[index];
            }
            return null;

        }
    }

}
