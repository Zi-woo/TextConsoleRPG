using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    class PartyMember : Character
    {
        public PartyMember(int level, string name, string job, int atk, int def, int matk, int hp, int mp, float cc, float ec)
        : base(level, name, job, atk, def, matk, hp, mp, cc, ec)
        {

        }
        public void SetSkills(string job)
        {
            List<Skills> skillsWarrior = new List<Skills>()
            {
                new Skills("파워 슬래쉬", "공격력 * 2의 데미지로 적 하나를 강하게 내리칩니다.", 10, 1,
                    (partyMember, targetList) => {
                        Monster target = targetList.First();
                        if (partyMember.CurMp < 10)
                        {
                            Console.WriteLine("MP가 부족합니다!");
                            return;
                        }
                        partyMember.SetCurMp(partyMember.CurMp - 10);

                        target.DamageByPlayer(partyMember.SkillDamageAttack(2f));
                    }),
                new Skills("우렁찬 함성", "팀원 전체의 공격력을 전투가 끝날 때까지 공격력 * 1만큼 증가 시킵니다.", 10, 3,
                    actionToCharacter: (partyMember, Characters) => {
                        if (partyMember.CurMp < 10)
                        {
                            Console.WriteLine("MP가 부족합니다!");
                            return;
                        }
                        partyMember.SetCurMp(partyMember.CurMp - 10);
                        int atkUp = partyMember.Atk;
                        foreach (var targetCharacter in Characters)
                        {
                            targetCharacter.SetAtk(targetCharacter.Atk + atkUp);
                        }
                    })
            };
            List<Skills> skillsWizard = new List<Skills>()
            {
                new Skills("힐", "마법공격력 * 1만큼 팀원 전체의 체력을 회복합니다.", 10, 3,
                    actionToCharacter: (partyMember, Characters) => {
                        if (partyMember.CurMp < 10)
                        {
                            Console.WriteLine("MP가 부족합니다!");
                            return;
                        }
                        partyMember.SetCurMp(partyMember.CurMp - 10);
                        foreach(var targetCharacter in Characters)
                            {
                                targetCharacter.SetCurHp(targetCharacter.CurHp + partyMember.Matk);
                                if (targetCharacter.CurHp > targetCharacter.MaxHp) targetCharacter.SetCurHp(targetCharacter.MaxHp);
                            }
                    }),
                new Skills("지혜 분출", "팀원 전체의 마법공격력을 전투가 끝날 때까지 마법공격력 * 1만큼 증가 시킵니다.", 10, 3,
                    actionToCharacter: (partyMember, Characters) => {
                        if (partyMember.CurMp < 10)
                        {
                            Console.WriteLine("MP가 부족합니다!");
                            return;
                        }
                        partyMember.SetCurMp(partyMember.CurMp - 10);
                        int matkUp = partyMember.Matk;
                        foreach(var targetCharacter in Characters)
                        {
                            targetCharacter.SetMatk(targetCharacter.Matk + matkUp);
                        }
                    }),
            };
            List<Skills> skillsArcher = new List<Skills>()
            {
                new Skills("진실의 눈", "적 전체의 공격력을 공격력 * 1만큼 감소시킵니다.", 10, 2,
                    (partyMember, targetList) => {
                        if (partyMember.CurMp < 10)
                        {
                            Console.WriteLine("MP가 부족합니다!");
                            return;
                        }
                        partyMember.SetCurMp(partyMember.CurMp - 10);
                        foreach(var targetMonster in targetList)
                        {
                            targetMonster.Atk = partyMember.Atk - targetMonster.Atk;
                            if (targetMonster.Atk < 0) targetMonster.Atk = 0;
                        }
                    }),
                new Skills("애로우 봄", "공격력 * 2의 데미지로 전 전체를 공격합니다.", 10, 2,
                    (partyMember, targetList) => {
                        if (partyMember.CurMp < 10)
                        {
                            Console.WriteLine("MP가 부족합니다!");
                            return;
                        }
                        partyMember.SetCurMp(partyMember.CurMp - 10);
                        foreach (var targetMonster in targetList)
                        {
                                targetMonster.DamageByPlayer(partyMember.SkillDamageAttack(1.5f));
                        }
                    }),
            };
            List<Skills> skillsRogue = new List<Skills>()
            {
                new Skills("명치 슬래쉬", "적 하나를 공격합니다. 적은 현재 체력의 50%만큼 데미지를 입습니다.", 10, 1,
                    (partyMember, targetList) => {
                        if (partyMember.CurMp < 10)
                        {
                            Console.WriteLine("MP가 부족합니다!");
                            return;
                        }
                        partyMember.SetCurMp(partyMember.CurMp - 10);
                        Monster target = targetList.First();
                        target.DamageByPlayer((int)(target.Hp / 2));
                    }),
                new Skills("삼연 수리검 던지기", "적 하나에게 수리검을 던져 공격력 * 1의 데미지로 공격합니다. 3번 던집니다.", 10, 1,
                    (partyMember, targetList) => {
                        if (partyMember.CurMp < 10)
                        {
                            Console.WriteLine("MP가 부족합니다!");
                            return;
                        }
                        partyMember.SetCurMp(partyMember.CurMp - 10);
                        Monster target = targetList.First();
                        for (int i = 0; i < 3; i++)
                        {
                            target.DamageByPlayer(partyMember.SkillDamageAttack(1f));
                        }
                    }),
            };
            switch (job)
            {
                case "전사":
                    this.LearnedSkills = skillsWarrior;
                    break;
                case "마법사":
                    this.LearnedSkills = skillsWizard;
                    break;
                case "궁수":
                    this.LearnedSkills = skillsArcher;
                    break;
                case "도적":
                    this.LearnedSkills = skillsRogue;
                    break;
            }
        }
    }

}
