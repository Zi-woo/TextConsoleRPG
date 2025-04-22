using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    class Character
    {
        [JsonInclude]
        public int Level { get; private set; }
        [JsonInclude]
        public int Exp { get; private set; }
        [JsonInclude]
        public string Name { get; private set; }
        [JsonInclude]
        public string Job { get; private set; }
        [JsonInclude]
        public int Atk { get; private set; }
        [JsonInclude]
        public int Def { get; private set; }
        [JsonInclude]
        public int Matk { get; private set; } // 마법공격력
        [JsonInclude]
        public int CurHp { get; private set; }
        [JsonInclude]
        public int PreDgnHp { get; set; } // 던전입장전체력
        [JsonInclude]
        public int MaxHp { get; private set; } // 최대체력
        [JsonInclude]
        public int CurMp { get; private set; }
        [JsonInclude]
        public int PreDgnMp { get; set; } 
        [JsonInclude]
        public int MaxMp { get; private set; }
        [JsonInclude]
        public int Gold { get; private set; }
        [JsonInclude]
        public int ExtraAtk { get; private set; }
        [JsonInclude]
        public int ExtraDef { get; private set; }
        [JsonInclude]
        public int ExtraMatk { get; private set; }
        public List<Skills> LearnedSkills { get; private set; }

        private List<Item> Inventory = new List<Item>();
        private List<Item> EquipList = new List<Item>();

        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }
        public Character() { }
        public Character(int level, string name, string job, int atk, int def, int matk, int hp,int mp, int gold, List<Skills> learnedSkills)
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
            Gold = gold;
            LearnedSkills = learnedSkills;
        }

        public void DisplayCharacterInfo()
        {
            Console.WriteLine($"Lv. {Level:D2}");
            Console.WriteLine($"{Name} {{ {Job} }}");
            Console.WriteLine(ExtraAtk == 0 ? $"공격력 : {Atk}" : $"공격력 : {Atk + ExtraAtk} (+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"방어력 : {Def}" : $"방어력 : {Def + ExtraDef} (+{ExtraDef})");
            Console.WriteLine(ExtraDef == 0 ? $"마법공격력 : {Matk}" : $"마법공격력 : {Matk + ExtraMatk} (+{ExtraMatk})");
            Console.WriteLine($"HP : {CurHp}");
            Console.WriteLine($"MP : {CurMp}");
            Console.WriteLine($"Gold : {Gold} G");
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

        public void EquipItem(Item item)
        {
            if (IsEquipped(item))
            {
                EquipList.Remove(item);
                if (item.Type == 0)
                    ExtraAtk -= item.Value;
                else
                    ExtraDef -= item.Value;
            }
            else
            {
                EquipList.Add(item);
                if (item.Type == 0)
                    ExtraAtk += item.Value;
                else
                    ExtraDef += item.Value;
            }
        }

        public bool IsEquipped(Item item)
        {
            return EquipList.Contains(item);
        }

        public void BuyItem(Item item)
        {
            Gold -= item.Price;
            Inventory.Add(item);
        }

        public bool HasItem(Item item)
        {
            return Inventory.Contains(item);
        }


        public int Damage(float Atkf) //데미지 계산
        {
            Random random = new Random();
            int C = random.Next(1, 101);
            bool critical = false;
            if (C <= 15)
                critical = true;

            int d = (int)Math.Ceiling(Atkf * 0.1);
            float randomAtk = random.Next(-d, d + 1);
            float damage = Atkf + randomAtk;

            int totaldamage;
            if (critical)
            {
                damage = damage * 1.6f;
                Console.WriteLine($"치명타");
                return totaldamage = (int)damage;
                
            }
            else
                return totaldamage = (int)damage;      
        }
        public void ReceivedDamage(int damage)
        {
            CurHp -= damage;
            if (CurHp <= 0) CurHp = 0;
            
        }
        public int SkillDamageAttack(int manaCost, float damageMul) 
        {
            if (CurMp < manaCost) return 0;
            CurMp -= manaCost;
            return (int)(Atk * damageMul);
        }
        public int SkillDamageMagic(int manaCost, float damageMul)
        {
            if (CurMp < manaCost) return 0;
            CurMp -= manaCost;
            return (int)(Matk * damageMul);
        }
    }
}
