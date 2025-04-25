using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    class Player : Character
    {
        [JsonInclude]
        public int Gold { get; private set; }
        [JsonInclude]
        public int Potion { get; private set; }
        [JsonInclude]
        private List<string> QuestNameList { get; set; } = new List<string>();
        private List<IQuest> Quests { get; } = new List<IQuest>();
        public Player() { }
        public Player(int level, string name, string job, int atk, int def, int matk, int hp, int mp, int gold, List<Skills> learnedSkills)
        : base(level, name, job, atk, def, matk, hp, mp, learnedSkills)
        {
            Gold = gold;
            Potion = 3;
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
        public void GetGold(int gold)
        {
            Gold += gold;
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
            if (Potion > 0)
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
        public void UpdateQuest(string targetName, QUEST_TYPE questType)
        {
            switch (questType)
            {
                case QUEST_TYPE.KILL_MONSTER:
                    foreach (var quest in Quests)
                    {
                        if (quest is KillMonsterQuest killQuest)
                        {
                            killQuest.MonsterKilled(targetName);
                        }
                    }
                    break;
                case QUEST_TYPE.USE_ITEM:
                    foreach (var quest in Quests)
                    {
                        if (quest is UseItemQuest itemQuest)
                        {
                            itemQuest.UseItem(targetName);
                        }
                    }
                    break;
            }
        }
        public void InitPlayerQuest(List<IQuest> quests)
        {
            foreach(var quest in quests)
            {
                if(QuestNameList.Contains(quest.Name))
                {
                    Quests.Add(quest);
                }
            }
        }
    }
}
