using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    class Skills
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MpCost { get; set; }
        public int Type { get; set; } // Type 1 : 대상지정필요 Type 2 : 대상지정불필요 Type 3 : 대상이 캐릭터

        public Action<Character, List<Monster>> ActionToMonster { get; set; } 
        public Action<Character, List<Character>> ActionToCharacter { get; set; } 

        public Skills(string name, string description, int mpCost,int type, 
            Action<Character, List<Monster>> actionToMonster = null,
              Action<Character, List<Character>> actionToCharacter = null)
        {
            Name = name;
            Description = description;
            MpCost = mpCost;
            Type = type;
            ActionToMonster = actionToMonster;
            ActionToCharacter = actionToCharacter;
        }
       

    }
}
