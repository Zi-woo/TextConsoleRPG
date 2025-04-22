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
        public string Name { get; private set; }
        [JsonInclude]
        public string Job { get; private set; }
        [JsonInclude]
        public int Atk { get; private set; }
        [JsonInclude]
        public int Def { get; private set; }
        [JsonInclude]
        public int CurHp { get; private set; }
        public int PreDgnHp { get; set; } // 던전입장전체력
        public int MaxHp { get; private set; } // 최대체력
        [JsonInclude]
        public int Gold { get; private set; }
        [JsonInclude]
        public int ExtraAtk { get; private set; }
        [JsonInclude]
        public int ExtraDef { get; private set; }

        private List<Item> Inventory = new List<Item>();
        private List<Item> EquipList = new List<Item>();

        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }
       
        public Character(int level, string name, string job, int atk, int def, int hp, int gold)
        {
            Level = level;
            Name = name;
            Job = job;
            Atk = atk;
            Def = def;
            CurHp = hp;
            PreDgnHp = hp;
            MaxHp = hp;
            Gold = gold;
        }

        public void DisplayCharacterInfo()
        {
            Console.WriteLine($"Lv. {Level:D2}");
            Console.WriteLine($"{Name} {{ {Job} }}");
            Console.WriteLine(ExtraAtk == 0 ? $"공격력 : {Atk}" : $"공격력 : {Atk + ExtraAtk} (+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"방어력 : {Def}" : $"방어력 : {Def + ExtraDef} (+{ExtraDef})");
            Console.WriteLine($"체력 : {CurHp}");
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

        public void DamagebyMonster(int damage)//플레이어 체력 감소

        {
            CurHp -= damage;
            if (CurHp < 0) CurHp = 0;
            Console.WriteLine($"{damage} 데미지를 입혔다!");
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
    }
}
