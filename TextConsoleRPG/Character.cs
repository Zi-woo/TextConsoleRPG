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
        public int PreviousLevel { get; private set; }
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

        public int Exp { get; private set; }

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
        public Character(int previousLevel,int level, string name, string job, int atk, int def, int hp, int gold, int exp)
        {
            PreviousLevel = previousLevel;
            Level = level;
            Name = name;
            Job = job;
            Atk = atk;
            Def = def;
            CurHp = hp;
            PreDgnHp = hp;
            MaxHp = hp;
            Gold = gold;
            Exp = exp;
        }

        public void AddExp(int amount)
        {

            Exp += amount;
            
            while (CanLevelUp())
            {
                LevelUp();
            }
        }

        private bool CanLevelUp() // 레벨업 할 수 있니?
        {
            int requiredExp = GetRequiredExpForNextLevel(); // 다음 레벨에 필요한 경험치 계산
            return Exp >= requiredExp;  // 지금 경험치가 그보다 많거나 같으면 true
        }

        private int GetRequiredExpForNextLevel() //다음 레벨까지 필요한 경험치를 계산해서 가져오는 함수
        {
            switch (Level)
            {
                case 1: return 10;
                case 2: return 35;
                case 3: return 65;
                case 4: return 100;
                default: return int.MaxValue; // 최대 레벨
            }
        }

        private void LevelUp()
        {
            int previousLevel = Level;  // 이전 레벨을 저장 (필요 시 로그나 비교용)
            Exp -= GetRequiredExpForNextLevel();    // 현재 경험치에서 레벨업에 필요한 경험치를 차감
            Level++;
            MaxHp += 10;
            Atk += (int)0.5f;
            Def += 1;
            CurHp = MaxHp;
         
        }



        public void DisplayCharacterInfo()
        {
            Console.WriteLine($"Lv. {Level:D2}");
            Console.WriteLine($"{Name} {{ {Job} }}");
            Console.WriteLine(ExtraAtk == 0 ? $"공격력 : {Atk}" : $"공격력 : {Atk + ExtraAtk} (+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"방어력 : {Def}" : $"방어력 : {Def + ExtraDef} (+{ExtraDef})");
            Console.WriteLine($"체력 : {CurHp}");
            Console.WriteLine($"Gold : {Gold} G");
            Console.WriteLine($"Exp : {Exp}");
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
    }
}
