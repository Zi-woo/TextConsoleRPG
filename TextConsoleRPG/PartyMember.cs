using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    class PartyMember : Character
    {
        public PartyMember(int level, string name, string job, int atk, int def, int matk, int hp, int mp, List<Skills> learnedSkills)
        : base(level, name, job, atk, def, matk, hp, mp, learnedSkills)
        {

        }

    }
}
