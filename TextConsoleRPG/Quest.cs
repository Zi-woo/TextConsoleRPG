using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    #region 퀘스트 DTO
    internal class QuestDTO
    {
        [JsonInclude]
        public string Type; // "KillMonster", "UseItem", "EquipItem"
        [JsonInclude]
        public string Name;
        [JsonInclude]
        public string Description;
        [JsonInclude]
        public int Exp;
        [JsonInclude]
        public int Gold;

        // 보상 아이템
        [JsonInclude]
        public string? RewardItemName;
        [JsonInclude]
        public int RewardItemCount;

        // KillMonster 전용
        [JsonInclude]
        public string? TargetMonster;
        [JsonInclude]
        public int TargetMonsterCount;
        [JsonInclude]
        public int CurrentKillCount;

        // UseItem 전용
        [JsonInclude]
        public string? TargetItem;
        [JsonInclude]
        public int TargetItemCount;
        [JsonInclude]
        public int CurrentItemCount;

        // EquipItem 전용
        [JsonInclude]
        public string? EquipTargetItem;
        [JsonInclude]
        public bool IsEquipCompleted;
    }
    internal static class QuestDTOConverter
    {
        public static QuestDTO ToDTO(IQuest quest)
        {
            QuestDTO dto = new QuestDTO
            {
                Name = quest.Name,
                Description = quest.Description,
                Exp = quest.Reward.Exp,
                Gold = quest.Reward.Gold,
                RewardItemName = quest.Reward.RewardItem?.Name,
                RewardItemCount = quest.Reward.ItemCount
            };

            switch (quest)
            {
                case KillMonsterQuest kmq:
                    dto.Type = "KillMonster";
                    dto.TargetMonster = kmq.TargetMonster;
                    dto.TargetMonsterCount = kmq.TargetMonsterCount;
                    dto.CurrentKillCount = kmq.CurrentKillCount;
                    break;

                case UseItemQuest uiq:
                    dto.Type = "UseItem";
                    dto.TargetItem = uiq.TargetItem;
                    dto.TargetItemCount = uiq.TargetItemCount;
                    dto.CurrentItemCount = uiq.CurrentItemCount;
                    break;

                //case EquipItemQuest eiq:
                //    dto.Type = "EquipItem";
                //    dto.EquipTargetItem = eiq.TargetItem;
                //    dto.IsEquipCompleted = eiq.IsCompleted;
                //    break;
            }

            return dto;
        }
        public static IQuest FromDTO(QuestDTO dto, List<Item> itemList)
        {
            var rewardItem = itemList.FirstOrDefault(item => item.Name == dto.RewardItemName);

            return dto.Type switch
            {
                "KillMonster" => new KillMonsterQuest(
                    dto.Name,
                    dto.Description,
                    dto.TargetMonster!,
                    dto.TargetMonsterCount,
                    dto.Exp,
                    dto.Gold,
                    rewardItem!,
                    dto.RewardItemCount,
                    dto.CurrentKillCount
                ),

                "UseItem" => new UseItemQuest(
                    dto.Name,
                    dto.Description,
                    dto.TargetItem!,
                    dto.TargetItemCount,
                    dto.Exp,
                    dto.Gold,
                    rewardItem!,
                    dto.RewardItemCount,
                    dto.CurrentItemCount
                ),

                //"EquipItem" => new EquipItemQuest(
                //    dto.Name,
                //    dto.Description,
                //    dto.EquipTargetItem!,
                //    dto.Exp,
                //    dto.Gold,
                //    rewardItem!,
                //    dto.RewardItemCount,
                //    dto.IsEquipCompleted
                //),

                _ => throw new InvalidOperationException($"Unknown quest type: {dto.Type}")
            };
        }
    }
    #endregion
    public enum QUEST_TYPE { KILL_MONSTER, USE_ITEM }
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
    internal interface IQuest
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
            public string TargetMonster => targetMonster;
            public int TargetMonsterCount => targetMonsterCount;
            public int CurrentKillCount => currentKillCount;
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
            public KillMonsterQuest(string name, string description, string monster, int targetCount, int exp, int gold, Item rewardItem, int itemCount, int current = 0)
    : this(name, description, monster, targetCount, exp, gold, rewardItem, itemCount)
            {
                currentKillCount = current;
            }
            public void MonsterKilled(string monster)
            {
                if (isCompleted) return;
                if (monster == targetMonster)
                {
                    ++currentKillCount;
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
                    reward += $" {Reward.RewardItem.Name} x {Reward.ItemCount}\n";
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
            currentKillCount = 0;
        }
    }

        internal class UseItemQuest : IUseItemQuest
        {
            public string Name { get; private set; }
            public string Description { get; private set; }
            public string TargetItem => targetItem;
            public int TargetItemCount => targetItemCount;
            public int CurrentItemCount => currentItemCount;
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
            public UseItemQuest(string name, string description, string item, int targetCount, int exp, int gold, Item rewardItem, int itemCount, int current = 0)
    : this(name, description, item, targetCount, exp, gold, rewardItem, itemCount)
            {
                currentItemCount = current;
            }
            public void UseItem(string item)
            {
                if (isCompleted) return;
                if (item == targetItem)
                {
                    ++currentItemCount;
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
            currentItemCount = 0;
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

