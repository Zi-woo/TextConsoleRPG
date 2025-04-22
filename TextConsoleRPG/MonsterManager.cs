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
        new Monster("미니언", 2, 15, 5),
        new Monster("대포미니언", 5, 25, 8),
        new Monster("공허충", 3, 10, 9),
        new Monster("원거리미니언", 2, 15, 5),
        new Monster("슈퍼미니언", 4, 11, 11),
        new Monster("지휘관미니언", 5, 12, 11),
        new Monster("바론버프미니언", 6, 14, 10)

    };

        public List<Monster> spawnedMonsters = new List<Monster>(); // 실제로 스폰된 몬스터 List

        public void SpawnRandomMonster(int count)
        {
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                int index = rand.Next(monsterDb.Count);
                Monster selected = new Monster( 
                    monsterDb[index].Name,
                    monsterDb[index].Level,
                    monsterDb[index].Hp,
                    monsterDb[index].Atk
                );
                spawnedMonsters.Add(selected);
            }
        }
    }

}
