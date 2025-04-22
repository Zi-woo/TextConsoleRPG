using System;
using System.Collections.Generic;
using System.Linq;
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
        public int Hp { get; private set; }
        [JsonInclude]
        public int Gold { get; private set; }
        [JsonInclude]
        public int ExtraAtk { get; private set; }
        [JsonInclude]
        public int ExtraDef { get; private set; }
        [JsonInclude]
        private List<int> InventoryIdList { get; set; } = new List<int>();
        [JsonInclude]
        private List<int> EquipItemIdList { get; set; } = new List<int>();
        private List<Item> Inventory { get; set; } = new List<Item>();
        private List<Item> EquipList { get; set; } = new List<Item>();

        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }
        public Character() { }
        public Character(int level, string name, string job, int atk, int def, int hp, int gold)
        {
            Level = level;
            Name = name;
            Job = job;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
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
            Console.WriteLine($"체력 : {Hp}");
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
    }
}
