using System;
using System.ComponentModel;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using TextConsoleRPG;

namespace MyApp
{
    public class Program
    {
        private static Character player;
        private static List<Item> itemDb;
        private static MonsterManager mm;
        //private static List<Monster> spawnedMonster;
        public const string playerDataPath = "playerData.json";
        public const string itemDBPath = "items.json";

        static void Main(string[] args)
        {
            StartScreen();
            DisplayMainUI();
        }
        static void StartScreen()
        {
            Console.Clear();
            Console.WriteLine("1. 새 게임");
            Console.WriteLine("2. 불러오기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            int result = CheckInput(1, 2);

            switch (result)
            {
                case 1:
                    string name = CharacterCreationUI();
                    string job = ChoiceJobUI();
                    SetData(name, job);
                    break;
                case 2:
                    LoadPlayerData();
                    break;
            }
        }
        static void SetData(string name, string job)
        {

            //TODO 캐릭터 생성
            switch (job)
            {
                case "전사":
                    player = new Character(1, name, job, 8, 6, 110, 10000);
                    break;
                case "마법사":
                    player = new Character(1, name, job, 5, 5, 100, 10000);
                    break;
                case "궁수":
                    player = new Character(1, name, job, 11, 5, 90, 10000);
                    break;
                case "도적":
                    player = new Character(1, name, job, 13, 4, 80, 10000);
                    break;
            }
        }

        static string CharacterCreationUI()
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 이름을 설정해주세요.");
            string name = Console.ReadLine();
            return name;
        }
        static string ChoiceJobUI()
        {
            Console.Clear();
            Console.WriteLine("직업을 선택하세요");
            Console.WriteLine();
            Console.WriteLine("1. 전사");
            Console.WriteLine("2. 마법사");
            Console.WriteLine("3. 궁수");
            Console.WriteLine("4. 도적");

            int result = CheckInput(1, 4);

            switch (result)
            {
                case 1:
                    return "전사";
                    break;
                case 2:
                    return "마법사";
                    break;
                case 3:
                    return "궁수";
                    break;
                case 4:
                    return "도적";
                    break;
            }
            return "";
        }
        #region 메인화면
        static void DisplayMainUI()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 휴식");
            Console.WriteLine("5. 전투 시작");
            Console.WriteLine("6. 저장");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(1, 5);


            switch (result)
            {
                case 1:
                    DisplayStatUI();
                    break;

                case 2:
                    DisplayInventoryUI();
                    break;

                case 3:
                    DisplayShopUI();
                    break;
                case 4:
                    DisplayRestUI();
                    break;
                case 5:
                    InitializeBattle();
                    break;
                case 6:
                    SavePlayerData();
                    break;
            }
        }
        #endregion
        #region 상태보기
        static void DisplayStatUI()
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");

            player.DisplayCharacterInfo();

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 0);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;
            }
        }
        #endregion
        #region 인벤토리
        static void DisplayInventoryUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            player.DisplayInventory(false);

            Console.WriteLine();
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 1);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;

                case 1:
                    DisplayEquipUI();
                    break;
            }
        }
        #endregion
        #region 장착관리
        static void DisplayEquipUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            player.DisplayInventory(true);

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, player.InventoryCount);

            switch (result)
            {
                case 0:
                    DisplayInventoryUI();
                    break;

                default:

                    int itemIdx = result - 1;
                    Item targetItem = itemDb[itemIdx];
                    player.EquipItem(targetItem);

                    DisplayEquipUI();
                    break;
            }
        }
        #endregion
        #region 상점
        static void DisplayShopUI()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < itemDb.Count; i++)
            {
                Item curItem = itemDb[i];

                string displayPrice = (player.HasItem(curItem) ? "구매완료" : $"{curItem.Price} G");
                Console.WriteLine($"- {curItem.ItemInfoText()}  |  {displayPrice}");
            }

            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 1);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;

                case 1:
                    DisplayBuyUI();
                    break;
            }
        }
        #endregion
        #region 아이템 구매
        static void DisplayBuyUI()
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < itemDb.Count; i++)
            {
                Item curItem = itemDb[i];

                string displayPrice = (player.HasItem(curItem) ? "구매완료" : $"{curItem.Price} G");
                Console.WriteLine($"- {i + 1} {curItem.ItemInfoText()}  |  {displayPrice}");
            }

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, itemDb.Count);

            switch (result)
            {
                case 0:
                    DisplayShopUI();
                    break;

                default:
                    int itemIdx = result - 1;
                    Item targetItem = itemDb[itemIdx];

                    // 이미 구매한 아이템이라면?
                    if (player.HasItem(targetItem))
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                        Console.WriteLine("Enter 를 눌러주세요.");
                        Console.ReadLine();
                    }
                    else // 구매가 가능할떄
                    {
                        //   소지금이 충분하다
                        if (player.Gold >= targetItem.Price)
                        {
                            Console.WriteLine("구매를 완료했습니다.");
                            player.BuyItem(targetItem);
                        }
                        else
                        {
                            Console.WriteLine("골드가 부족합니다.");
                            Console.WriteLine("Enter 를 눌러주세요.");
                            Console.ReadLine();
                        }

                        //   소지금이 부족핟
                    }

                    DisplayBuyUI();
                    break;
            }
        }
        #endregion
        #region 휴식
        static void DisplayRestUI()
        {
            int restcost = 500;
            Console.Clear();
            Console.WriteLine("휴식");
            Console.WriteLine("휴식을 취하여 체력을 회복 할 수 있습니다..\n");
            Console.WriteLine("휴식하기");
            Console.WriteLine($"{restcost} G를 내면 체력을 회복할 수 있습니다.(보유골드 : {player.Gold}G)\n");
            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을입력해주세요.");

            int result = CheckInput(0, 1);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;
                case 1:
                    player.Rest(restcost);
                    DisplayRestUI();
                    break;
            }
        }
        #endregion
        #region 전투 세팅
        static void InitializeBattle()
        {
            player.PreDgnHp = player.CurHp;
            Console.Clear();
            Console.WriteLine("Battle!!");
            Console.WriteLine();
            Random random = new Random();
            
            mm = new MonsterManager();
            mm.SpawnRandomMonster(random.Next(1,5));

            DisplayBattleUI();
        }
        static void DisplayBattleUI()
        {
            Console.Clear();
            for (int i = 0; i < mm.spawnedMonsters.Count; i++)
            {
                mm.spawnedMonsters[i].MonsterInfoText();
            }

            Console.WriteLine();
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv. {player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"HP {player.CurHp}/{player.MaxHp}");
            Console.WriteLine();
            Console.WriteLine("1. 공격");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(1, 1);

            PlayerPhase();
        }
        #endregion
        #region 공격
        static void EnemyPhase()
        {
            Console.Clear();
            
            Console.WriteLine("Battle!!");
            Console.WriteLine();
            for (int i = 0; i < mm.spawnedMonsters.Count; i++)
            {
                Monster m = mm.spawnedMonsters[i];
                if (m.Hp <= 0)
                    continue;
                Console.WriteLine($"Lv. {m.Level} {m.Name} 의 공격!");
                Console.WriteLine($"{player.Name}을(를) 맞췄습니다. [데미지: {m.Atk}]");
                Console.WriteLine();
                int Atkm = player.Damage(m.Atk);
                player.DamagebyMonster(Atkm);
                Console.WriteLine($"Lv. {player.Level} {player.Name}");
                Console.WriteLine($"HP {player.PreDgnHp} -> {player.CurHp}\n");//현재체력 최대체력
                Console.WriteLine("Enter 를 눌러주세요.");
                Console.ReadLine();
                Console.WriteLine();
                if (player.CurHp <= 0) DisplayBattleResult(false);
            }
            DisplayBattleUI();
        }
        static void PlayerPhase()
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");
            
            //몬스터 출력
            for (int i = 0; i < mm.spawnedMonsters.Count; i++)
            {
                Monster m = mm.spawnedMonsters[i];
                string status = m.AliveMonster() ? $"(HP: {m.Hp})" : "(Dead)";
                Console.WriteLine($"{i + 1}. {m.Name} {status}");
            }

            Console.WriteLine();
            Console.WriteLine("0. 취소");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, mm.spawnedMonsters.Count);//몬스터수에 따른 입력값 

            switch (result)
            {
                case 0:
                    DisplayBattleUI();
                    break;
                default:
                    int MonsterIdx = result - 1;
                    Monster targetMonster = mm.spawnedMonsters[MonsterIdx];
                    {
                        if (!targetMonster.AliveMonster()) //타격 전 생존 확인 
                        {
                            Console.WriteLine("이미 죽은 대상입니다");
                            Console.WriteLine("Enter 를 눌러주세요.");
                            Console.ReadLine();
                            PlayerPhase();
                        }
                        else // 타격
                        {
                            float Atkf = player.Atk;
                            int total = player.Damage(Atkf);
                            targetMonster.DamagebyPlayer(total);
                            Console.WriteLine($"{targetMonster.Name}을 공격!");
                            Thread.Sleep(500);
                            if (!targetMonster.AliveMonster()) //타격 후 생존 확인 
                            {
                                Console.WriteLine($"{targetMonster.Name}이(가) 쓰러졌습니다!");
                                Console.WriteLine("\nEnter 를 눌러주세요.");
                                Console.ReadLine();
                            }
                        }
                    }
                    for (int i = 0; i < mm.spawnedMonsters.Count; i++)
                    {
                        if (mm.spawnedMonsters[i].Hp > 0) break;
                        if (i == mm.spawnedMonsters.Count - 1) DisplayBattleResult(true);
                    }
                    EnemyPhase();
                    break;
            }
        }
        static void DisplayBattleResult(bool isWin)
        {
            Console.Clear();
            if (isWin)
            {
                Console.WriteLine("Battle!! - Result");
                Console.WriteLine();
                Console.WriteLine("Victory");
                Console.WriteLine();
                Console.WriteLine($"던전에서 몬스터 {mm.spawnedMonsters.Count}마리를 잡았습니다.");
                Console.WriteLine();
                Console.WriteLine($"Lv. {player.Level} {player.Name}");
                Console.WriteLine($"HP {player.PreDgnHp} -> {player.CurHp}");
                Console.WriteLine();
                Console.WriteLine("0. 다음");
                Console.WriteLine();
                CheckInput(0, 0);
            }
            else
            {
                Console.WriteLine("Battle!! - Result");
                Console.WriteLine();
                Console.WriteLine("You Lose");
                Console.WriteLine();
                Console.WriteLine($"Lv. {player.Level} {player.Name}");
                Console.WriteLine($"HP {player.PreDgnHp} -> 0");
                Console.WriteLine();
                Console.WriteLine("0. 다음");
                Console.WriteLine();
                CheckInput(0, 0);
            }
            DisplayMainUI();
        }
        #endregion

        #region 세이브
        static void SavePlayerData()
        {
            string jsonString = JsonSerializer.Serialize(player, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(playerDataPath, jsonString);

            string jsonString2 = JsonSerializer.Serialize(itemDb, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(itemDBPath, jsonString2);
        }
        static void LoadPlayerData()
        {
            if (File.Exists(playerDataPath))
            {
                //json 파일 읽어오기
                string json = File.ReadAllText(playerDataPath);
                player = (JsonSerializer.Deserialize<Character>(json));
            }
            else
            {
                Console.WriteLine("저장된 데이터가 없습니다."); // 저장된 데이터가 없다고 뜨고 StartScreen으로 바로넘어가니까 콘솔창이 클리어되서 이 문구가 안보임 -> 여유되면 고치기
                StartScreen();
            }
            if (File.Exists(itemDBPath))
            {
                string json = File.ReadAllText(itemDBPath);
                itemDb = JsonSerializer.Deserialize<List<Item>>(json);
            }
            else
            {
                //Console.Write("Fatal Error!!! - 아이템 db 못 찾음");
                Console.WriteLine("저장된 데이터가 없습니다.");
                StartScreen();
                Console.ReadKey();
            }
        }
        #endregion
        static int CheckInput(int min, int max)
        {
            int result;
            while (true)
            {
                string input = Console.ReadLine();
                bool isNumber = int.TryParse(input, out result);
                if (isNumber)
                {
                    if (result >= min && result <= max)
                        return result;
                }
                Console.WriteLine("잘못된 입력입니다!!!!");
            }
        }
    }
}