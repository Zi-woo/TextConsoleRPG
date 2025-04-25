using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextConsoleRPG
{
    class PartyManager
    {
        public List<PartyMember> PartyMembers = new List<PartyMember>()
        {
            new PartyMember(1, "고드프리", "전사", 5, 10, 0, 100, 50, 0.15f, 0.1f),
            new PartyMember(1, "엘린", "마법사", 5, 5, 10, 100, 100,  0.15f, 0.1f),
            new PartyMember(1, "실비아", "궁수", 10, 5, 0, 100, 50,  0.15f, 0.1f), 
            new PartyMember(1, "닉스", "도적", 10, 5, 0, 100, 50,  0.15f, 0.1f)
        };
        public List<PartyMember> OwnedPartyMembers = new List<PartyMember>();
        public void CreatePartyMem()
        {
            for (int i = 0; i < 4; i++)
            {
                PartyMember selected = new PartyMember(
                    PartyMembers[i].Level,
                    PartyMembers[i].Name,
                    PartyMembers[i].Job,
                    PartyMembers[i].Atk,
                    PartyMembers[i].Def,
                    PartyMembers[i].Matk,
                    PartyMembers[i].CurHp,
                    PartyMembers[i].CurMp,
                    PartyMembers[i].CriticalChance,
                    PartyMembers[i].EvasionChance
                );
                PartyMembers[i].SetSkills(PartyMembers[i].Job);
                OwnedPartyMembers.Add(selected);
            }
        }
    }
}
