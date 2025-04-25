using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyApp;

namespace TextConsoleRPG
{
    abstract class Character
    {
        [JsonInclude]
        public int Level { get; private set; }
        [JsonInclude]
        public int Exp { get; protected set; }

        [JsonInclude]
        public string Name { get; private set; }
        [JsonInclude]
        public string Job { get; private set; }
        [JsonInclude]
        public int Atk { get; protected set; }
        [JsonInclude]
        public int Def { get; protected set; }
        [JsonInclude]
        public int Matk { get; protected set; } // 마법공격력
        [JsonInclude]
        public int originalAtk { get; protected set; } // 전투에서 올랐던 공격력을 전투종료시 원래값으로 되돌리기 위한 변수
        [JsonInclude]
        public int originalMatk { get; protected set; }
        [JsonInclude]
        public int CurHp { get; protected set; }
        [JsonInclude]
        public int PreDgnHp { get; set; } // 던전입장전체력
        [JsonInclude]
        public int MaxHp { get; private set; } // 최대체력
        [JsonInclude]
        public int CurMp { get; protected set; }
        [JsonInclude]
        public int PreDgnMp { get; set; }
        [JsonInclude]
        public int MaxMp { get; private set; }
        [JsonInclude]
        public float CriticalChance { get; set; }
        [JsonInclude]
        public float EvasionChance { get; set; }


        [JsonInclude]
        public int ExtraAtk { get; private set; }
        [JsonInclude]
        public int ExtraDef { get; private set; }
        [JsonInclude]
        public int ExtraMatk { get; private set; }


        [JsonInclude]
        protected List<int> InventoryIdList { get; set; } = new List<int>();
        [JsonInclude]
        protected List<int> EquipItemIdList { get; set; } = new List<int>();

        [JsonIgnore]
        public List<Skills> LearnedSkills { get; private set; }
        protected List<Item> Inventory { get; set; } = new List<Item>();
        protected List<Item> EquipList { get; set; } = new List<Item>();


        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }
        public Character() { }
        public Character(int level, string name, string job, int atk, int def, int matk, int hp, int mp, float cc, float ec, List<Skills> learnedSkills)
        {
            Level = level;
            Exp = 0;
            Name = name;
            Job = job;
            Atk = atk;
            Def = def;
            Matk = matk;
            CurHp = hp;
            PreDgnHp = hp;
            MaxHp = hp;
            CurMp = mp;
            PreDgnMp = mp;
            MaxMp = mp;
            LearnedSkills = learnedSkills;
            originalAtk = atk;
            originalMatk = matk;
            CriticalChance = cc;
            EvasionChance = ec;
        }

        public void LoadItemList(List<Item> items)
        {
            foreach (Item item in items)
            {
                if (InventoryIdList.Contains(item.Id))
                {
                    Inventory.Add(item);
                }
            }
            foreach (Item item in Inventory)
            {
                if (EquipItemIdList.Contains(item.Id))
                {
                    EquipList.Add(item);
                }
            }
        }

        public void DisplayCharacterInfo()
        {
            Console.WriteLine("╔═══════════════════════╗");
            Console.WriteLine($"  Lv. {Level:D2}");
            Console.WriteLine($"  {Name} {{ {Job} }}");
            Console.WriteLine(ExtraAtk == 0 ? $"  공격력 : {Atk}" : $"  공격력 : {Atk + ExtraAtk} (+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"  방어력 : {Def}" : $"  방어력 : {Def + ExtraDef} (+{ExtraDef})");
            Console.WriteLine(ExtraDef == 0 ? $"  마법공격력 : {Matk}" : $"  마법공격력 : {Matk + ExtraMatk} (+{ExtraMatk})");
            Console.WriteLine($"  HP : {CurHp}/{MaxHp}");
            Console.WriteLine($"  MP : {CurMp}/{MaxMp}");
            // Console.WriteLine($"Gold : {Gold} G");
            Console.WriteLine("╚═══════════════════════╝");
        }

        public void DisplayInventory(bool showIdx)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                Item targetItem = Inventory[i];

                string displayIdx = showIdx ? $"{i + 1} " : "";
                string displayEquipped = IsEquipped(targetItem) ? "[E]" : "";
                Console.WriteLine($"- {displayIdx}{displayEquipped} {targetItem.ItemInfoText()}");
            }
        }

        public void EquipItem(int index)
        {
            EquipItem(Inventory[index]);
        }
        public void EquipItem(Item item)
        {
            // 장착 가능 아이템인지 확인 (무기 or 방어구만 가능)
            if (item.Type == 0 || item.Type == 1)
            {
                // 같은 타입의 장비가 이미 장착돼 있다면 자동 해제
                for (int i = 0; i < EquipList.Count; i++)
                {
                    if (EquipList[i].Type == item.Type)
                    {
                        Item oldItem = EquipList[i];
                        EquipList.RemoveAt(i);      //찾은 장비(oldItem)를 장착 목록에서 제거
                        EquipItemIdList.Remove(oldItem.Id);    //EquipItemIdList에서도 제거

                        if (oldItem.Type == 0)      //기존 장비의 스탯 보정값 제거
                            ExtraAtk -= oldItem.Value;
                        else if (oldItem.Type == 1)
                            ExtraDef -= oldItem.Value;

                        Console.WriteLine($"[{oldItem.Name}]을(를) 해제했습니다.");
                        break;
                    }
                }

                // 새 장비 장착
                EquipList.Add(item);
                EquipItemIdList.Add(item.Id);

                if (item.Type == 0)
                    ExtraAtk += item.Value;
                else if (item.Type == 1)
                    ExtraDef += item.Value;

                Console.WriteLine($"[{item.Name}]을(를) 장착했습니다.");
            }
            else
            {
                Console.WriteLine($"[{item.Name}]은(는) 장착할 수 없는 아이템입니다.");
            }

            Console.WriteLine("Enter 키를 눌러주세요.");
            Console.ReadLine();
        }

        public bool IsEquipped(Item item)
        {
            return EquipList.Contains(item);
        }
        public void ResetStat()
        {
            Atk = originalAtk;
            Matk = originalMatk;
        }
        public void GetExp(int exp, LevelManager lm)
        {
            Exp += exp;
            while (lm.LevelUp(Level, Exp))
            {
                Exp -= lm.GetRequiredExp(Level);
                Level++;
                Atk++; // 0.5 올라야 되지만 Atk int형을 깨기싫어서..
                Matk++;
                Def++;
                Console.WriteLine("레벨업!!");
                Console.WriteLine($"현재레벨 : {Level}");
                Console.WriteLine();
            }
        }
        public void DamagebyMonster(int damage)//플레이어 체력 감소

        {
            CurHp -= damage;
            if (CurHp < 0) CurHp = 0;
            Console.WriteLine($"{damage} 데미지를 입었다!\n");
        }

        public int Damage(float Atkf, int DefI, float CC) //데미지 계산
        {
            Random random = new Random();

            // 치명타 판정
            double roll = random.NextDouble();

            // 공격력 변동
            int d = (int)Math.Ceiling(Atkf * 0.1);
            float randomAtk = random.Next(-d, d + 1);
            float damage = Atkf + randomAtk;

            // 치명타 적용
            if (roll < CC)
            {
                damage *= 1.6f;
                Console.Write("치명타! ");
            }

            int totalDamage = (int)damage - DefI;
            if (totalDamage < 0) totalDamage = 0;

            return totalDamage;

        }
        public bool Evasion(double ec) //회피
        {
            Random random = new Random();
            double roll = random.NextDouble();

            return roll < ec;
        }
        public void ReceivedDamage(int damage)
        {
            CurHp -= damage;
            if (CurHp <= 0) CurHp = 0;

        }
        public int SkillDamageAttack(float damageMul)
        {
            return (int)((Atk + ExtraAtk)* damageMul);
        }
        public int SkillDamageMagic(float damageMul)
        {
            return (int)((Matk + ExtraMatk) * damageMul);
        }

        public void SetAtk(int atk)
        {
            Atk = atk;
        }
        public void SetMatk(int matk)
        {
            Matk = matk;
        }
        public void SetCurHp(int curHp)
        {
            CurHp = curHp;
        }
        public void SetCurMp(int curMp)
        {
            CurMp = curMp;
        }

    }
}
