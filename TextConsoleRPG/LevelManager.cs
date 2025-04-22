using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    class LevelManager
    {
        private Dictionary<int, int> RequiredExp;

        public LevelManager()
        {
            RequiredExp = new Dictionary<int, int>
            {
                {1, 10},  // Lv1 → Lv2
                {2, 35},  // Lv2 → Lv3
                {3, 65},  // Lv3 → Lv4
                {4, 100}  // Lv4 → Lv5
            };
        }

        public int GetRequiredExp(int level)
        {
            if (RequiredExp.TryGetValue(level, out int exp))
                return exp;
            else
                return int.MaxValue;  
        }

        public bool LevelUp(int level, int exp)
        {
            if(RequiredExp.ContainsKey(level)) return exp >= RequiredExp[level];
            return false;
        }
    }
}
