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
        public int Type { get; set; } // Type 1 : 대상지정필요 Type 2 : 대상지정불필요

        public Action<Character, List<Monster>> Effect { get; set; }

        public Skills(string name, string description, int mpCost,int type, Action<Character, List<Monster>> effect)
        {
            Name = name;
            Description = description;
            MpCost = mpCost;
            Type = type;
            Effect = effect;
        }

    }
}
