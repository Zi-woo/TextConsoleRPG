using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    internal class QuestReward
    {
        public Item? RewardItem { get; } = null;
        public int ItemCount = 0;
        public int Gold { get; } = 0;
        public int Exp { get; } = 0;
        public QuestReward(int exp, int gold, Item item, int itemCount)
        {
            Gold = gold;
            Exp = exp;
            RewardItem = item;
            ItemCount = itemCount;
        }
        public void ApplyReward(Player player)
        {
            player.GetReward(RewardItem, ItemCount, Gold, Exp);
        }
    }
    #region 퀘스트인터페이스
    interface IQuest
    {
        string Name { get; }
        string Description { get; }
        bool isCompleted { get; }
        QuestReward Reward { get; }
        void DisplayQuestInfo();
        void Completed(Player player);
    }
    interface IKillMonsterQuest : IQuest
    {
        void MonsterKilled(string monster);
    }
    interface IUseItemQuest : IQuest
    {
        void UseItem(string item);
    }
    interface IEquipItemQuest : IQuest
    {
        void EquipItem(string item);
    }
    #endregion
        internal class KillMonsterQuest : IKillMonsterQuest
        {
            public string Name { get; private set; }
            public string Description { get; private set; }
            private string targetMonster;
            private int targetMonsterCount;
            private int currentKillCount = 0;
            public bool isCompleted => currentKillCount >= targetMonsterCount;
            //퀘스트 완료 조건 외부에서 호출 필요 - quest.Reward.GetReward(player);
            public QuestReward Reward { get; private set; }

            public KillMonsterQuest(string name,string description, string monster, int targetCount, int exp, int gold, Item rewardItem, int itemCount)
            {
                Name = name;
                Description = description;
                targetMonsterCount = targetCount;
                targetMonster = monster;
                Reward = new QuestReward(exp, gold, rewardItem, itemCount);
            }
            public void MonsterKilled(string monster)
            {
                if (isCompleted) return;
                if (monster == targetMonster)
                {
                    ++targetMonsterCount;
                }
            }
            public void DisplayQuestInfo()
            {
                Console.WriteLine(Name);
                Console.WriteLine($"\n{Description}\n");
                Console.WriteLine($"- {targetMonster} {targetMonsterCount}마리 처치 ({currentKillCount} / {targetMonsterCount})\n");
                Console.WriteLine($"- 보상");
                string reward = "";
                if(Reward.RewardItem != null)
                {
                    reward += $" {Reward.RewardItem} x {Reward.ItemCount}\n";
                }
                if (Reward.Gold > 0) {
                    reward += $" {Reward.Gold}  G\n";
                }
                if (Reward.Exp >0)
                {
                    reward += $" {Reward.Exp} 경험치";
                }
                Console.WriteLine(reward);
            }
        public void Completed(Player player)
        {
            player.GetReward(Reward.RewardItem, Reward.ItemCount, Reward.Gold, Reward.Exp);
        }
    }

        internal class UseItemQuest : IUseItemQuest
        {
            public string Name { get; private set; }
            public string Description { get; private set; }
            private string targetItem;
            private int targetItemCount;
            private int currentItemCount = 0;
            public bool isCompleted => currentItemCount >= targetItemCount;
            public QuestReward Reward { get; private set; }
            public UseItemQuest(string name,string description, string item, int targetCount, int exp, int gold, Item rewardItem, int itemCount)
            {
                Name = name;
                Description= description;
                targetItem = item;
                targetItemCount = targetCount;
                Reward = new QuestReward(exp, gold, rewardItem, itemCount);
        }
            public void UseItem(string item)
            {
                if (isCompleted) return;
                if (item == targetItem)
                {
                    ++targetItemCount;
                }
            }
            public void DisplayQuestInfo()
            {
                Console.WriteLine(Name);
                Console.WriteLine($"\n{Description}\n");
                Console.WriteLine($"- {targetItem} {targetItemCount}개 사용 ({currentItemCount} / {targetItemCount})\n");
                Console.WriteLine($"- 보상");
                string reward = "";
                if (Reward.RewardItem != null)
                {
                    reward += $" {Reward.RewardItem.Name} x {Reward.ItemCount}\n";
                }
                if (Reward.Gold > 0)
                {
                    reward += $" {Reward.Gold} G\n";
                }
                if (Reward.Exp > 0)
                {
                    reward += $" {Reward.Exp} 경험치";
                }
                Console.WriteLine(reward);
            }
            public void Completed(Player player)
            {
                player.GetReward(Reward.RewardItem,Reward.ItemCount,Reward.Gold,Reward.Exp);
            }
        }

        //TODO 아이템 장착 퀘스트 - 작업 필요
        internal class EquipItemQuest : IEquipItemQuest
        {
            public string Name { get; private set; }
            public string Description { get; private set; }
            private string targetItem;

            public bool isCompleted { get; }
            public QuestReward Reward { get; private set; }
            public EquipItemQuest(string name, string description,string item, int exp, int gold, Item rewardItem, int itemCount)
            {
                Name = name;
                Description = description;
                targetItem = item;
                Reward = new QuestReward(exp, gold, rewardItem, itemCount);
        }
            public void EquipItem(string item) { }
            public void DisplayQuestInfo()
            {
                Console.WriteLine(Name);
                Console.WriteLine($"\n{Description}\n");
                //Console.WriteLine($"- {targetMonster} {targetMonsterCount}마리 처치 ({currentKillCount / targetMonsterCount})\n");
                Console.WriteLine($"- 보상");
                string reward = "  ";
                if (Reward.RewardItem != null)
                {
                    reward += $"{Reward.RewardItem.Name} x {Reward.ItemCount}\n";
                }
                if (Reward.Gold > 0)
                {
                    reward += $" {Reward.Gold} G\n";
                }
                if (Reward.Exp > 0)
                {
                    reward += $" {Reward.Exp} 경험치";
                }
                Console.WriteLine(reward);
            }
        public void Completed(Player player)
        {
            player.GetReward(Reward.RewardItem, Reward.ItemCount, Reward.Gold, Reward.Exp);
        }
    }
    }

