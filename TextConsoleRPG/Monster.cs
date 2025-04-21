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
        public int Hp { get; }
        public int Atk { get; }

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
    }
}
