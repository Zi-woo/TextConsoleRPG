using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    #region 퀘스트인터페이스
    interface IQuest
    {
        string Name { get; }
        bool isCompleted { get; }
    }
    interface IKillMonsterQuest : IQuest
    {
        void MonsterKilled(string monster);
    }
    interface IUseItemQuest : IQuest
    {
        void UseItem(string item);
    }
    #endregion
    class QuestReward
    {
        public Item? Item { get; } = null;
        public int Gold { get; } = 0;
        public int Exp { get; } = 0;
        public QuestReward(int exp, int gold, Item item)
        {
            Gold = gold;
            Exp = exp;
            Item = item;
        }
        public void ApplyReward(Character player)
        {
            if (Item != null)
            {

            }
        }
        public class KillMonsterQuest : IKillMonsterQuest
        {
            public string Name { get; private set; }
            private string targetMonster;
            private int targetMonsterCount;
            private int currentKillCount = 0;
            public bool isCompleted => currentKillCount >= targetMonsterCount;

            public KillMonsterQuest(string name, string monster, int targetCount)
            {
                Name = name;
                targetMonsterCount = targetCount;
                targetMonster = monster;
            }
            public void MonsterKilled(string monster)
            {
                if (isCompleted) return;
                if (monster == targetMonster)
                {
                    ++targetMonsterCount;
                }
            }
        }

        public class UseItemQuest : IUseItemQuest
        {
            public string Name { get; private set; }
            private string targetItem;
            private int targetItemCount;
            private int currentItemCount = 0;
            public bool isCompleted => currentItemCount >= targetItemCount;
            public UseItemQuest(string name, string item, int targetCount)
            {
                Name = name;
                targetItem = item;
                targetItemCount = targetCount;
            }
            public void UseItem(string item)
            {
                if (isCompleted) return;
                if (item == targetItem)
                {
                    ++targetItemCount;
                }
            }
        }
    }
}
