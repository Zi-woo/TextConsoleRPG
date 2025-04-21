using System;
using System.Numerics;
using System.Text.Json;
using TextConsoleRPG;

namespace MyApp
{
    public class Program
    {
        private static Character player;
        private static List<Item> itemDb;
        private static List<Monster> monsterDb;
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
            return Console.ReadLine();
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
            Console.WriteLine("4. 저장");
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
                    SavePlayerData();
                    break;
                case 5:
                    DisplayBattleUI();
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
        static void DisplayBattleUI()
        {
            Console.Clear();
            monsterDb = new List<Monster>
            {
            new Monster("미니언", 2, 15, 5),
            new Monster("대포미니언", 5, 25, 8),
            new Monster("공허충",3, 10, 9)
            };
            int result = CheckInput(0, 1);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;

                case 1:
                    DisplayAttackUI();
                    break;
            }
        }
        #region 공격
        static void DisplayAttackUI()
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");
            
            //몬스터 출력
            for (int i = 0; i < monsterDb.Count; i++)
            {
                Monster m = monsterDb[i];
                string status = m.AliveMonster() ? $"(HP: {m.Hp})" : "(Dead)";
                Console.WriteLine($"{i + 1}. {m.Name} {status}");
            }

            Console.WriteLine();
            Console.WriteLine("0. 취소");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, monsterDb.Count);//몬스터수에 따른 입력값 

            switch (result)
            {
                case 0:
                    DisplayBattleUI();
                    break;
                default:
                    int Monsterdx = result - 1;
                    Monster targetMonster = monsterDb[Monsterdx];
                    {
                        if (!targetMonster.AliveMonster()) //타격 전 생존 확인 
                        {
                            Console.WriteLine("이미 죽은 대상입니다");
                            Console.WriteLine("Enter 를 눌러주세요.");
                            Console.ReadLine();
                        }
                        else // 타격
                        {
                            double damage = player.Atk;
                            Random random = new Random();
                            int d = (int)Math.Ceiling(damage * 0.1);
                            int randomDamage = random.Next(-d, d + 1);
                            int total = (int)damage + randomDamage;
                            targetMonster.Damage(total);
                            Console.WriteLine($"{targetMonster.Name}을 공격!");
                            Thread.Sleep(500);


                        }
                        if (!targetMonster.AliveMonster()) //타격 전 생존 확인 
                        {
                            Console.WriteLine($"{targetMonster.Name}이(가) 쓰러졌습니다!");
                            Console.WriteLine("\nEnter 를 눌러주세요.");
                            Console.ReadLine();
                        }
                    }
                    DisplayAttackUI();
                    break;

            }
        }
        #endregion
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