using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    class PartyMember : Character
    {
        public PartyMember(int level, string name, string job, int atk, int def, int matk, int hp, int mp,float cc, float ec, List<Skills> learnedSkills)
        : base(level, name, job, atk, def, matk, hp, mp, cc, ec, learnedSkills)
        {

        }

    }
}
