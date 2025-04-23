using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
     class MonsterManager
    {
        public List<Monster> monsterDb = new List<Monster>() // 이 세상에 존재하는 몬스터 종류
        {
            new Monster("미니언", 2, 15, 5, 2,  new List<Item>
            {
                new Item(101, "낡은 검", 0, 2, "무뎌진 검. 공격력이 조금 상승한다.", 500, 0.29f),
                new Item(102, "낡은 방패", 1, 3, "튼튼하지 않지만 막을 수는 있다.", 500, 0.29f)
            }),
            new Monster("대포미니언", 5, 25, 8, 5, new List<Item>
            {
                new Item(103, "부서진 칼자루", 3, 0, "뭔가 쓸모있어 보인다.", 200, 0.56f),
                new Item(104, "부서진 칼날", 3, 0, "어디선가 본 듯 하다.", 300, 0.45f)
            }),
            new Monster("공허충", 3, 10, 9, 3, new List<Item>
            {
                new Item(105, "낡은 스태프", 0, 7, "마력이 조금 깃든 오래된 지팡이.", 800, 0.26f),
                new Item(106, "낡은 방패", 1, 3, "튼튼하지 않지만 막을 수는 있다.", 500, 0.29f)
            }),
            new Monster("원거리미니언", 2, 15, 5, 2, new List<Item>
            {
                new Item(107, "회복 포션", 4, 30, "마시면 체력이 조금 회복된다.", 100, 0.7f),
                new Item(108, "두꺼운 장갑", 1, 5, "아주 질기다", 600, 0.28f)
            }),
            new Monster("슈퍼미니언", 4, 11, 11, 4, new List<Item>
            {
                new Item(109, "구리 단검", 0, 6, "날카롭지만 금방 날이 망가질 것 같다.", 500, 0.29f),
                new Item(110, "은 방패", 1, 8, "반짝반짝 빛난다.", 800, 0.26f)
            }),
            new Monster("지휘관미니언", 5, 12, 11,5, new List<Item>
            {
                new Item(111, "거대한 장궁", 0, 9, "활시위가 튼튼한 거대한 활.", 900, 0.25f),
                new Item(112, "멋진 모자", 1, 8, "멋이 좋다.", 800, 0.26f)
            }),
            new Monster("바론버프미니언", 6, 14, 10, 6, new List<Item>
            {
                new Item(113, "두꺼운 갑옷", 1, 10, "무겁지만 튼튼하다. 듬직하다.", 1000, 0.24f),
                new Item(114, "헤르메스의 신발", 1, 8, "신으면 빨라질 것 같다.", 1200, 0.226f)
            })
        };

        public List<Monster> spawnedMonsters = new List<Monster>(); // 실제로 스폰된 몬스터 List

        public void SpawnRandomMonster(int stage)
        {
            spawnedMonsters.Clear();

            Random random = new Random();
            int count = random.Next(1, 5); //1~4마리 랜덤
            int minLevel = 2;
            int maxLevel = 2 + ((stage - 1) / 2);// 스테이지 1: 2레벨, 2: 2~4레벨, 3: 2~6레벨
            List<Monster> filtered = monsterDb.Where(m => m.Level >= minLevel && m.Level <= maxLevel).ToList();

            if (filtered.Count == 0)
            {
                filtered = monsterDb;
            }

            for (int i = 0; i < count; i++)
            {
                int index = random.Next(filtered.Count);
                Monster baseMonster = filtered[index];

                int scaledHp = baseMonster.Hp + (stage * 2);
                int scaledAtk = baseMonster.Atk + (stage);
                int scaledExp = baseMonster.Exp + (stage);

                Monster selected = new Monster(
                    baseMonster.Name,
                    baseMonster.Level,
                    scaledHp,
                    scaledAtk,
                    scaledExp,
                    baseMonster.DropItemList
                );
                spawnedMonsters.Add(selected);
            }
        }

    }
}

