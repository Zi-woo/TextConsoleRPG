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
    internal class Character
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
        [JsonInclude]
        public int Potion { get; private set; }

        [JsonInclude]
        private List<int> InventoryIdList { get; set; } = new List<int>();
        [JsonInclude]
        private List<int> EquipItemIdList { get; set; } = new List<int>();

        [JsonInclude]
        private List<string> QuestNameList { get; set; } = new List<string>();
        public List<Skills> LearnedSkills { get; private set; }
        private List<Item> Inventory { get; set; } = new List<Item>();
        private List<Item> EquipList { get; set; } = new List<Item>();

        private List<IQuest> Quests { get; set; } = new List<IQuest>();

        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }
        public Character() { }
        public Character(int level, string name, string job, int atk, int def, int matk, int hp, int mp, int gold, List<Skills> learnedSkills)
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
            Potion = 3;
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
            Console.WriteLine($"Lv. {Level:D2}");
            Console.WriteLine($"{Name} {{ {Job} }}");
            Console.WriteLine(ExtraAtk == 0 ? $"공격력 : {Atk}" : $"공격력 : {Atk + ExtraAtk} (+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"방어력 : {Def}" : $"방어력 : {Def + ExtraDef} (+{ExtraDef})");
            Console.WriteLine(ExtraDef == 0 ? $"마법공격력 : {Matk}" : $"마법공격력 : {Matk + ExtraMatk} (+{ExtraMatk})");
            Console.WriteLine($"HP : {CurHp}/{MaxHp}");
            Console.WriteLine($"MP : {CurMp}/{MaxMp}");
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

        public void EquipItem(int index)
        {
            EquipItem(Inventory[index]);
        }
        public void EquipItem(Item item)
        {
            if (IsEquipped(item))
            {
                EquipList.Remove(item);
                EquipItemIdList.Remove(item.Id);
                if (item.Type == 0)
                    ExtraAtk -= item.Value;
                else
                    ExtraDef -= item.Value;
            }
            else
            {
                EquipList.Add(item);
                EquipItemIdList.Add(item.Id);
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
        public void GetReward(Item item, int itemCount, int gold, int exp)
        {
            Gold += gold;
            Exp += exp;
            if (item != null)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    Inventory.Add(item);
                    InventoryIdList.Add(item.Id);
                }
            }
        }
        public void BuyItem(Item item)
        {
            Gold -= item.Price;
            Inventory.Add(item);
            InventoryIdList.Add(item.Id);
        }

        public bool HasItem(Item item)
        {
            return Inventory.Contains(item);
        }
        public void GetItem(Item item)
        {
            Inventory.Add(item);
        }

        public void Rest(int cost)
        {
            Console.Clear();
            if (Gold >= cost)//보유 여부 확인
            {
                Gold -= cost;
                Console.WriteLine("몸이 한결 가벼워진 느낌을 받습니다.");
                Console.WriteLine($"HP {CurHp} -> {MaxHp}\n");
                CurHp = MaxHp;
                Console.WriteLine("Enter 를 눌러주세요.");
                Console.ReadLine();

            }
            else
            {
                Console.WriteLine("Gold 가 부족합니다.");
                Console.WriteLine("Enter 를 눌러주세요.");
                Console.ReadLine();
            }
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
            Console.WriteLine($"{damage} 데미지를 입었다!");
        }

        public int Damage(float Atkf) //데미지 계산
        {
            Random random = new Random();//치명타
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
                totaldamage = (int)damage - Def;
                if (totaldamage < 0) totaldamage = 0;
                return totaldamage;

            }
            else
            {
                totaldamage = (int)damage - Def;
                if (totaldamage < 0) totaldamage = 0;
                return totaldamage;
            }
        }
        public void ReceivedDamage(int damage)
        {
            CurHp -= damage;
            if (CurHp <= 0) CurHp = 0;

        }
        public int SkillDamageAttack(int manaCost, float damageMul)
        {
            CurMp -= manaCost;
            return (int)(Atk * damageMul);
        }
        public int SkillDamageMagic(int manaCost, float damageMul)
        {
            CurMp -= manaCost;
            return (int)(Matk * damageMul);
        }

        public bool Evasion() //회피
        {
            Random random = new Random();
            int E = random.Next(1, 101);
            bool evasion = false;
            if (E <= 10)
                evasion = true;
            return evasion;
        }
        public void GetGold(int gold)
        {
            Gold += gold;
        }
        public void UsePotion()
        {
            if (CurHp == MaxHp)
            {
                Console.WriteLine("더 이상 체력을 회복할 수 없습니다.");
                Console.WriteLine("아무키나 누르세요.");
                Console.WriteLine();
                Console.ReadLine();
                return;
            }
            if (Potion> 0)
            {
                Potion--;
                CurHp += 30;
                if (CurHp > MaxHp) CurHp = MaxHp;
                Console.WriteLine("회복을 완료했습니다.");
            }
            else
            {
                Console.WriteLine("포션이 부족합니다.");
            }
            Console.WriteLine();
        }
        public bool isAcceptedQuest(string questName)
        {
            return QuestNameList.Contains(questName);
        }
        public void AcceptQuest(IQuest quest)
        {
            Quests.Add(quest);
            QuestNameList.Add(quest.Name);
        }
        public void RemoveQuest(IQuest quest)
        {
            Quests.Remove(quest);
            QuestNameList.Remove(quest.Name);
        }
    }
}
