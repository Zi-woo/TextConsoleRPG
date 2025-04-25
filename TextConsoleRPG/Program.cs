using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using TextConsoleRPG;

namespace MyApp
{
    public class Program
    {
        private static Player player;
        private static List<Item> itemDb;
        private static List<IQuest> QuestDb;
        private static MonsterManager mm;
        private static StageManager stage = new StageManager();
        private static PartyManager pm = new PartyManager();
        private static LevelManager lm = new LevelManager();
        public const string playerDataPath = "playerData.json";
        public const string itemDBPath = "items.json";

        static void Main(string[] args)
        {
            StartScreen();
            while (true)
            {
                DisplayMainUI();
            }
            //pm.CreatePartyMem();
        }
        static void StartScreen()
        {
            Console.Clear();
            Console.WriteLine("1. 새 게임");
            Console.WriteLine("2. 불러오기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            int result = CheckInput(1, 2);

            //아이템 정보 불러오기 - 게임 데이터를 로드할 때 한 번에 하면 좋을 것
            if (File.Exists(itemDBPath))
            {
                string jsonItemDB = File.ReadAllText(itemDBPath);
                itemDb = JsonSerializer.Deserialize<List<Item>>(jsonItemDB);
            }
            else
            {
                //Console.Write("Fatal Error!!! - 아이템 db 못 찾음");
                Console.WriteLine("저장된 데이터가 없습니다.");
                Console.ReadKey();
                StartScreen();
            }
            //퀘스트 불러오기 테스트 코드
            string json = File.ReadAllText("quests.json");
            List<QuestDTO> dtos = JsonSerializer.Deserialize<List<QuestDTO>>(json);

            QuestDb = dtos.Select(d => QuestDTOConverter.FromDTO(d, itemDb)).ToList();

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
            List<Skills> skillsWarrior = new List<Skills>()
            {
                new Skills(
                    "알파 스트라이크",
                    "공격력 * 2로 하나의 적을 공격합니다.",
                    10,
                    1,
                    (user, targetList) => {
                        Monster target = targetList.First();
                        if (user.CurMp < 10)
                        {
                            Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                            return;
                        }
                        user.SetCurMp(user.CurMp - 10);
                        target.DamageByPlayer(user.SkillDamageAttack(2f));
                    }
                ),
                new Skills("더블 스트라이크",
                "공격력 * 1.5로 두 명의 적을 랜덤으로 공격합니다.",
                10,
                2,
                (user, targetList) => {
                    var targets = targetList.Take(2);
                    if (user.CurMp < 10)
                    {
                        Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                        return;
                    }
                    user.SetCurMp(user.CurMp - 10);
                    foreach (var target in targets)
                    {
                       target.DamageByPlayer(user.SkillDamageAttack(1.5f));
                    }
                  }
                )
            };
            List<Skills> skillsWizard = new List<Skills>(){
                new Skills(
                    "파이어 볼",
                    "마법공격력 * 2로 하나의 적을 공격합니다.",
                    10,
                    1,
                    (user, targetList) => {
                        if (user.CurMp < 10)
                    {
                        Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                        return;
                    }
                        Monster target = targetList.First();
                        user.SetCurMp(user.CurMp - 10);
                        target.DamageByPlayer(user.SkillDamageMagic(2f));
                    }
                ),
                new Skills("파이어 애로우",
                "마법공격력 * 1.5로 두 명의 적을 랜덤으로 공격합니다.",
                10,
                2,
                (user, targetList) => {
                    if (user.CurMp < 10)
                    {
                        Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                        return;
                    }
                        var targets = targetList.Take(2);
                    user.SetCurMp(user.CurMp - 10);
                    foreach (var target in targets)
                    {
                        target.DamageByPlayer(user.SkillDamageMagic(1.5f));
                    }
                  }
                )
            };
            List<Skills> skillsArcher = new List<Skills>(){
                new Skills(
                    "스나이핑",
                    "공격력 * 2로 하나의 적을 공격합니다.",
                    10,
                    1,
                    (user, targetList) => {
                        if (user.CurMp < 10)
                        {
                            Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                            return;
                        }
                        user.SetCurMp(user.CurMp - 10);
                        Monster target = targetList.First();
                        target.DamageByPlayer(user.SkillDamageAttack(2f));
                    }
                ),
                new Skills("더블 애로우",
                "공격력 * 1.5로 두 명의 적을 랜덤으로 공격합니다.",
                10,
                2,
                (user, targetList) => {
                    if (user.CurMp < 10)
                        {
                            Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                            return;
                        }
                    user.SetCurMp(user.CurMp - 10);
                        var targets = targetList.Take(2);
                    foreach (var target in targets)
                    {
                        target.DamageByPlayer(user.SkillDamageAttack(1.5f));
                    }
                  }
                )
            };
            List<Skills> skillsRogue = new List<Skills>()
            {
                new Skills(
                    "암습",
                    "공격력 * 2로 하나의 적을 공격합니다.",
                    10,
                    1,
                    (user, targetList) => {
                        if (user.CurMp < 10)
                        {
                            Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                            return;
                        }
                        user.SetCurMp(user.CurMp - 10);
                        Monster target = targetList.First();
                        target.DamageByPlayer(user.SkillDamageAttack(2f));
                    }
                ),
                new Skills("그림자 베기",
                "공격력 * 1.5로 두 명의 적을 랜덤으로 공격합니다.",
                10,
                2,
                (user, targetList) => {
                    if (user.CurMp < 10)
                    {
                        Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                        return;
                    }
                    user.SetCurMp(user.CurMp - 10);
                    var targets = targetList.Take(2);
                    foreach (var target in targets)
                    {
                        target.DamageByPlayer(user.SkillDamageAttack(1.5f));
                    }
                  }
                )
            };
            //TODO 캐릭터 생성
            switch (job)
            {
                //레벨, 이름, 직업, 공격력, 방어력, 마법공격력, 체력, 마나, 돈, 보유 스킬
                case "전사":
                    player = new Player(1, name, job, 8, 6, 0, 110, 50, 10000, skillsWarrior);
                    break;
                case "마법사":
                    player = new Player(1, name, job, 5, 5, 10, 100, 100, 10000, skillsWizard);
                    break;
                case "궁수":
                    player = new Player(1, name, job, 11, 5, 0, 90, 50, 10000, skillsArcher);
                    break;
                case "도적":
                    player = new Player(1, name, job, 13, 4, 0, 80, 50, 10000, skillsRogue);
                    break;
            }
        }

        static string CharacterCreationUI()
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 이름을 설정해주세요.");
            Console.WriteLine();
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
            Console.WriteLine("5. 회복 아이템");
            Console.WriteLine("6. 전투 시작");
            Console.WriteLine("7. 퀘스트");
            Console.WriteLine("8. 파티원 모집");
            Console.WriteLine("9. 저장");
            Console.WriteLine("0. 게임 종료");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(0, 9);

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
                    DisplayPotionUI();
                    break;
                case 6:
                    StageSelection();
                    break;
                case 7:
                    DisplayQuestUI();
                    break;
                case 8:
                    DisplayRecruitPartyMember();
                    break;
                case 9:
                    SavePlayerData();
                    break;
                case 0:
                    Console.WriteLine("게임을 종료합니다.");
                    Environment.Exit(0); break;
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
        }
        #endregion
        #region 인벤토리
        static void DisplayInventoryUI()
        {
            while (true)
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
                        return;
                    case 1:
                        DisplayEquipUI();
                        break;
                }
            }
        }
        #endregion
        #region 장착관리
        static void DisplayEquipUI()
        {
            while (true)
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
                        return;

                    default:
                        //int itemIdx = result - 1;
                        //Item targetItem = itemDb[itemIdx];
                        //player.EquipItem(targetItem);

                        //---------인벤토리 리스트 내 아이템 선택 코드 수정-------------
                        int itemIdx = result - 1;
                        player.EquipItem(itemIdx);
                        break;
                }
            }

        }
        #endregion
        #region 상점
        static void DisplayShopUI()
        {
            while (true)
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
                        return;

                    case 1:
                        DisplayBuyUI();
                        break;
                }
            }

        }
        #endregion
        #region 아이템 구매
        static void DisplayBuyUI()
        {
            while (true)
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
                        return;

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
                        break;
                }
            }

        }
        #endregion

        #region 회복
        static void DisplayRestUI()
        {
            int restcost = 500;
            while (true)
            {
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
                        return;
                    case 1:
                        player.Rest(restcost);
                        break;
                }
            }
        }
        static void DisplayPotionUI()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("회복");
                Console.WriteLine($"포션을 사용하면 체력을 30 회복 할 수 있습니다. (남은 포션 : {player.Potion} )");
                Console.WriteLine();
                Console.WriteLine("1. 사용하기");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                int result = CheckInput(0, 1);
                switch (result)
                {
                    case 1:
                        player.UsePotion();
                        player.UpdateQuest("회복 포션", QUEST_TYPE.USE_ITEM);
                        break;
                    case 0:
                        return;
                }
            }
        }
        #endregion
        #region 스테이지 선택
        static void StageSelection()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("던전 입구");
                Console.WriteLine("스테이지를 선택해 주세요\n");
                for (int i = 1; i <= stage.TopStage; i++)
                {
                    Console.WriteLine($"{i}. 스테이지 {i}");
                }
                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("\n원하시는 스테이지를 입력해주세요.");
                int result = CheckInput(0, stage.TopStage);
                switch (result)
                {
                    case 0:
                        return;
                    default:
                        stage.SetStage(result);
                        InitializeBattle();
                        break;
                }
            }
        }
        #endregion
        #region 전투 세팅
        static void InitializeBattle()
        {
            Console.Clear();
            Console.WriteLine("Battle!!");
            Console.WriteLine();
            Random random = new Random();

            mm = new MonsterManager();
            mm.SpawnRandomMonster(stage.CurStage);
            player.PreDgnHp = player.CurHp;
            DisplayBattleUI();
        }
        static void DisplayBattleUI()
        {
            Console.Clear();
            Console.WriteLine($"스테이지{stage.CurStage}");

            mm.MonsterInfoText(true);//몬스터 출력

            Console.WriteLine();
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv. {player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"HP {player.CurHp}/{player.MaxHp}");
            Console.WriteLine();
            Console.WriteLine("1. 공격");
            Console.WriteLine("2. 스킬");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    PlayerPhaseAttack();
                    break;
                case 2:
                    PlayerPhaseSkill();
                    break;
            }

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

                bool evasion = player.Evasion();
                if (evasion) //회피
                {
                    Console.WriteLine($"{player.Name}은 공격을 피했다!");//현재체력 최대체력
                    Console.WriteLine("Enter 를 눌러주세요.");
                    Console.ReadLine();
                }
                else //명중
                {
                    Console.WriteLine($"{player.Name}을(를) 맞췄습니다. [데미지: {m.Atk}]\n");
                    int Atkm = player.Damage(m.Atk, player.Def);
                    player.DamagebyMonster(Atkm);

                    Console.WriteLine($"Lv. {player.Level} {player.Name}");
                    Console.WriteLine($"HP {player.PreDgnHp} -> {player.CurHp}\n");
                    Console.WriteLine("Enter 를 눌러주세요.");
                    Console.ReadLine();
                    Console.WriteLine();
                }
                if (player.CurHp <= 0)
                {
                    player.SetCurHp(0);
                    DisplayBattleResult(false);
                    break;
                }
            }
            return;
        }
        static void PlayerPhaseAttack()
        {
            bool isOver = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!!\n");
                Console.WriteLine($"[{player.Name}의 턴!]\n");
                Console.WriteLine();

                //몬스터 출력
                mm.MonsterInfoText();

                Console.WriteLine();
                Console.WriteLine("0. 취소");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                int result = CheckInput(0, mm.spawnedMonsters.Count);//몬스터수에 따른 입력값 

                switch (result)
                {
                    case 0:
                        return;
                    default:
                        int MonsterIdx = result - 1;
                        Monster targetMonster = mm.spawnedMonsters[MonsterIdx];
                        {
                            if (!targetMonster.AliveMonster()) //타격 전 생존 확인 
                            {
                                Console.WriteLine("이미 죽은 대상입니다");
                                Console.WriteLine("Enter 를 눌러주세요.");
                                Console.ReadLine();
                                break;
                            }
                            else
                            {
                                bool evasion = player.Evasion();
                                if (evasion) //회피
                                {
                                    Console.WriteLine($"{targetMonster.Name}은(는) 공격을 피했다!");
                                    Console.WriteLine("Enter 를 눌러주세요.");
                                    Console.ReadLine();
                                }
                                else //명중
                                {
                                    Console.WriteLine($"{targetMonster.Name}을 공격!");
                                    float Atkf = player.Atk;
                                    int total = player.Damage(Atkf, 0/*몬스터 방어력*/);
                                    targetMonster.DamageByPlayer(total);
                                    Console.WriteLine("Enter 를 눌러주세요.");
                                    Console.ReadLine();
                                }

                                if (!targetMonster.AliveMonster()) //타격 후 생존 확인 
                                {
                                    player.UpdateQuest(targetMonster.Name, QUEST_TYPE.KILL_MONSTER);
                                    Console.WriteLine($"{targetMonster.Name}이(가) 쓰러졌습니다!");
                                    Console.WriteLine("\nEnter 를 눌러주세요.");
                                    Console.ReadLine();
                                }
                                for (int i = 0; i < mm.spawnedMonsters.Count; i++)
                                {
                                    if (mm.spawnedMonsters[i].Hp > 0) break;
                                    if (i == mm.spawnedMonsters.Count - 1)
                                    {
                                        isOver = true;
                                        DisplayBattleResult(true);
                                        break;
                                    }
                                }
                                if (isOver) break;
                                if (pm.OwnedPartyMembers.Count > 0) PartyPhase();
                                else EnemyPhase();
                            }
                        }
                        break;
                }
            }

        }
        static void PlayerPhaseSkill()
        {
            bool isOver = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!!\n");
                Console.WriteLine($"[{player.Name}의 턴!]\n");
                Console.WriteLine();

                //몬스터 출력
                mm.MonsterInfoText();

                Console.WriteLine();
                int idx = 1;
                foreach (var skill in player.LearnedSkills)
                {
                    Console.WriteLine($"{idx}. {skill.Name} - MP {skill.MpCost}");
                    Console.WriteLine($"{skill.Description}");
                    idx++;
                }
                Console.WriteLine("0. 취소");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                int userChoice = CheckInput(0, player.LearnedSkills.Count);
                int skillChoice = userChoice - 1;
                if (userChoice == 0) return;
                if (player.LearnedSkills[userChoice - 1].Type == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("대상을 입력하세요.");
                    int result = CheckInput(1, mm.spawnedMonsters.Count);//몬스터수에 따른 입력값
                    int MonsterIdx = result - 1;

                    List<Monster> targetMonster = new List<Monster>();
                    targetMonster.Add(mm.spawnedMonsters[MonsterIdx]);
                    {
                        if (!targetMonster[0].AliveMonster()) //스킬시전 전 생존 확인 
                        {
                            Console.WriteLine("이미 죽은 대상입니다");
                            Console.WriteLine("Enter 를 눌러주세요.");
                            Console.ReadLine();
                            break;
                        }
                        else
                        {
                            player.LearnedSkills[skillChoice].ActionToMonster(player, targetMonster);
                            Console.WriteLine($"{player.LearnedSkills[skillChoice].Name}을(를) 시전!");
                            Thread.Sleep(500);
                            if (!targetMonster[0].AliveMonster())
                            {
                                player.UpdateQuest(targetMonster[0].Name, QUEST_TYPE.KILL_MONSTER);
                                Console.WriteLine($"{targetMonster[0].Name}이(가) 쓰러졌습니다!");
                                Console.WriteLine("\nEnter 를 눌러주세요.");
                                Console.ReadLine();
                            }
                        }
                    }
                }
                else if (player.LearnedSkills[userChoice - 1].Type == 2)
                {
                    List<Monster> targetMonster = new List<Monster>();
                    Random random = new Random();
                    foreach (var target in mm.spawnedMonsters)
                    {
                        if (target.Hp > 0)
                        {
                            targetMonster.Add(target);
                        }
                    }
                    if (targetMonster.Count >= 2)
                    {
                        List<Monster> shuffledList = targetMonster.OrderBy(x => random.Next()).ToList(); // 몬스터리스트 섞어서 렌덤성부여
                    }

                    player.LearnedSkills[skillChoice].ActionToMonster(player, targetMonster);

                    Console.WriteLine($"{player.LearnedSkills[skillChoice].Name}을(를) 시전!");
                    Thread.Sleep(500);
                    foreach (var target in targetMonster)
                    {
                        if (!target.AliveMonster())
                        {
                            player.UpdateQuest(target.Name, QUEST_TYPE.KILL_MONSTER);
                            Console.WriteLine($"{target.Name}이(가) 쓰러졌습니다!");
                            Console.WriteLine("\nEnter 를 눌러주세요.");
                            Console.ReadLine();
                        }
                    }
                }

                for (int i = 0; i < mm.spawnedMonsters.Count; i++)
                {
                    if (mm.spawnedMonsters[i].Hp > 0) break;
                    if (i == mm.spawnedMonsters.Count - 1)
                    {
                        isOver = true;
                        DisplayBattleResult(true);
                        break;
                    }
                }
                if (isOver) break;
                if (pm.OwnedPartyMembers.Count > 0) PartyPhase();
                else EnemyPhase();
            }

        }
        static void PartyPhase()
        {
            bool isOver = false;
            for (int j = 0; j < pm.OwnedPartyMembers.Count; j++)
            {
                PartyMember partyMem = pm.OwnedPartyMembers[j];
                Console.Clear();
                Console.WriteLine("Battle!!\n");
                Console.WriteLine($"[{partyMem.Name}의 턴!]\n");
                Console.WriteLine();

                //몬스터 출력
                for (int i = 0; i < mm.spawnedMonsters.Count; i++)
                {
                    Monster m = mm.spawnedMonsters[i];
                    string status = m.AliveMonster() ? $"(HP: {m.Hp})" : "(Dead)";
                    Console.WriteLine($"{i + 1}. {m.Name} {status}");
                }

                Console.WriteLine();
                Console.WriteLine("1. 공격");
                Console.WriteLine("2. 스킬");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                int result = CheckInput(1, 2);//몬스터수에 따른 입력값 

                switch (result)
                {
                    case 1:
                        Console.WriteLine("대상을 선택해주세요.");
                        int monsterIdx = CheckInput(1, mm.spawnedMonsters.Count) - 1;

                        Monster targetMonster = mm.spawnedMonsters[monsterIdx];
                        {
                            if (!targetMonster.AliveMonster()) //타격 전 생존 확인 
                            {
                                Console.WriteLine("이미 죽은 대상입니다");
                                Console.WriteLine("Enter 를 눌러주세요.");
                                Console.ReadLine();
                                j--;
                            }
                            else
                            {
                                bool evasion = partyMem.Evasion();
                                if (evasion) //회피
                                {
                                    Console.WriteLine($"{targetMonster.Name}은(는) 공격을 피했다!");
                                    Console.WriteLine("Enter 를 눌러주세요.");
                                    Console.ReadLine();
                                }
                                else //명중
                                {
                                    Console.WriteLine($"{targetMonster.Name}을 공격!");
                                    float Atkf = partyMem.Atk;
                                    int total = partyMem.Damage(Atkf, partyMem.Def);
                                    targetMonster.DamageByPlayer(total);
                                    Console.WriteLine("Enter 를 눌러주세요.");
                                    Console.ReadLine();
                                }

                                if (!targetMonster.AliveMonster()) //타격 후 생존 확인 
                                {
                                    Console.WriteLine($"{targetMonster.Name}이(가) 쓰러졌습니다!");
                                    Console.WriteLine("\nEnter 를 눌러주세요.");
                                    Console.ReadLine();
                                }
                            }
                            break;
                        }
                        for (int i = 0; i < mm.spawnedMonsters.Count; i++)
                        {
                            if (mm.spawnedMonsters[i].Hp > 0) break;
                            if (i == mm.spawnedMonsters.Count - 1)
                            {
                                isOver = true;
                                DisplayBattleResult(true);
                                break;
                            }
                        }
                        break;
                    case 2:
                        Console.WriteLine();
                        int idx = 1;
                        foreach (var skill in partyMem.LearnedSkills)
                        {
                            Console.WriteLine($"{idx}. {skill.Name} - MP {skill.MpCost}");
                            Console.WriteLine($"{skill.Description}");
                            idx++;
                        }
                        Console.WriteLine();
                        Console.WriteLine("시전할 스킬을 입력해주세요.");

                        int userChoice = CheckInput(1, partyMem.LearnedSkills.Count);
                        int skillChoice = userChoice - 1;
                        if (partyMem.LearnedSkills[userChoice - 1].Type == 1)
                        {
                            Console.WriteLine();
                            Console.WriteLine("대상을 입력하세요.");
                            int MonsterIdx = CheckInput(1, mm.spawnedMonsters.Count) - 1;//몬스터수에 따른 입력값

                            List<Monster> target = new List<Monster>();
                            target.Add(mm.spawnedMonsters[MonsterIdx]);
                            {
                                if (!target[0].AliveMonster()) //스킬시전 전 생존 확인 
                                {
                                    Console.WriteLine("이미 죽은 대상입니다");
                                    Console.WriteLine("Enter 를 눌러주세요.");
                                    Console.ReadLine();
                                    j--;
                                }
                                else
                                {
                                    partyMem.LearnedSkills[skillChoice].ActionToMonster(partyMem, target);
                                    Console.WriteLine($"{partyMem.LearnedSkills[skillChoice].Name}을(를) 시전!");
                                    Thread.Sleep(500);
                                    if (!target[0].AliveMonster())
                                    {
                                        Console.WriteLine($"{target[0].Name}이(가) 쓰러졌습니다!");
                                        Console.WriteLine("\nEnter 를 눌러주세요.");
                                        Console.ReadLine();
                                    }
                                }
                            }
                        }
                        else if (partyMem.LearnedSkills[userChoice - 1].Type == 2)
                        {
                            List<Monster> target = new List<Monster>();
                            foreach (var targetMon in mm.spawnedMonsters)
                            {
                                if (targetMon.Hp > 0)
                                {
                                    target.Add(targetMon);
                                }
                            }

                            partyMem.LearnedSkills[skillChoice].ActionToMonster(partyMem, target);

                            Console.WriteLine($"{partyMem.LearnedSkills[skillChoice].Name}을(를) 시전!");
                            Thread.Sleep(500);
                            foreach (var targetMon in target)
                            {
                                if (!targetMon.AliveMonster())
                                {
                                    Console.WriteLine($"{targetMon.Name}이(가) 쓰러졌습니다!");
                                    Console.WriteLine("\nEnter 를 눌러주세요.");
                                    Console.ReadLine();
                                }
                            }
                        }
                        else if (partyMem.LearnedSkills[userChoice - 1].Type == 3)
                        {
                            List<Character> target = new List<Character>();
                            target.Add(player);

                            foreach (var targetCharacter in pm.OwnedPartyMembers)
                            {
                                if (targetCharacter.CurHp > 0)
                                {
                                    target.Add(targetCharacter);
                                }
                            }
                            if (partyMem.LearnedSkills[skillChoice].ActionToCharacter != null)
                            {
                                partyMem.LearnedSkills[skillChoice].ActionToCharacter(partyMem, target);
                                Console.WriteLine($"{partyMem.LearnedSkills[skillChoice].Name}을(를) 시전!");
                                Thread.Sleep(500);
                            }
                        }
                    break;
                }
            }
            EnemyPhase();
        }
        #endregion
        #region 전투결과
        static void DisplayBattleResult(bool isWin)
        {
            Console.Clear();
            if (isWin)
            {
                Random random = new Random();
                int preExp = player.Exp;
                int preLv = player.Level;
                int expUp = 0;
                int getGold = 0;
                player.ResetStat();
                if (pm.OwnedPartyMembers.Count > 0)
                {
                    for (int i = 0; i < pm.OwnedPartyMembers.Count; i++)
                    {
                        pm.OwnedPartyMembers[i].ResetStat();
                    }
                }
                Item droppedItem;
                List<Item> getItem = new List<Item>();

                Console.WriteLine("Battle!! - Result");
                Console.WriteLine();
                Console.WriteLine("Victory");
                Console.WriteLine();
                Console.WriteLine($"던전에서 몬스터 {mm.spawnedMonsters.Count}마리를 잡았습니다.");
                Console.WriteLine();
                foreach (var monster in mm.spawnedMonsters)
                {
                    expUp += monster.Exp;
                    droppedItem = monster.ItemDrop();
                    if (droppedItem != null)
                    {
                        player.GetItem(droppedItem);
                        getItem.Add(droppedItem);
                    }
                }
                player.GetExp(expUp, lm);
                Console.WriteLine($"Lv. {player.Level} {player.Name}");
                Console.WriteLine($"HP {player.PreDgnHp} -> {player.CurHp}");
                if (preLv == player.Level) Console.WriteLine($"exp {preExp} -> {player.Exp}");
                else Console.WriteLine($"Lv. {preLv} -> Lv. {player.Level}");
                Console.WriteLine();
                Console.WriteLine("[획득 아이템]");
                getGold = random.Next(10 * expUp, 20 * expUp);
                Console.WriteLine($"{getGold} Gold");
                player.GetGold(getGold);
                foreach (var item in getItem)
                {
                    Console.WriteLine($"{item.Name} - 1");
                }
                Console.WriteLine();
                Console.WriteLine("0. 다음");
                Console.WriteLine();
                stage.NextStage();
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
            return;
        }
        #endregion
        #region 퀘스트
        static void DisplayQuestUI()
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("Quest!!\n");
                for (int i = 0; i < QuestDb.Count; ++i)
                {
                    Console.WriteLine($"{i + 1}. {QuestDb[i].Name}");

                }
                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("\n원하시는 행동을 선택해주세요.");
                int result = CheckInput(0, QuestDb.Count);
                switch (result)
                {
                    case 0: return;
                    default:
                        DisplaySelectedQuest(--result);
                        break;
                }
            }
            
        }

        static void DisplaySelectedQuest(int index)
        {
            Console.Clear();
            Console.WriteLine("Quest!!\n");
            QuestDb[index].DisplayQuestInfo();
            if (player.isAcceptedQuest(QuestDb[index].Name))
            {
                DisplayFinishedQuest(index);
            }
            else
            {
                DisplayAcceptQuest(index);
            }

        }
        static void DisplayAcceptQuest(int index)
        {
            Console.WriteLine("\n1. 수락");
            Console.WriteLine("2. 돌아가기\n");
            Console.WriteLine("원하시는 행동을 선택해주세요.");
            int result = CheckInput(1, 2);
            switch (result)
            {
                case 1:
                    player.AcceptQuest(QuestDb[index]);
                    break;
                case 2:
                    break;
            }
        }
        static void DisplayFinishedQuest(int index)
        {
            int i = 1;
            if (QuestDb[index].isCompleted)
            {
                Console.WriteLine($"\n{i}. 보상 받기");
                ++i;
            }
            Console.WriteLine($"{i}. 돌아가기\n");
            Console.WriteLine("원하시는 행동을 선택해주세요.");
            int result = CheckInput(1, i);
            if (i == 1)
            {
                return;
            }
            else if (result == 1)
            {
                QuestDb[index].Completed(player);
                player.RemoveQuest(QuestDb[index]);
            }
        }

        #endregion

        #region 세이브

        static void SavePlayerData()
        {
            string jsonPlayer = JsonSerializer.Serialize(player, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(playerDataPath, jsonPlayer);
            //퀘스트 리스트 변환 후 json 저장
            List<QuestDTO> dtos = QuestDb.Select(QuestDTOConverter.ToDTO).ToList();

            string jsonQuest = JsonSerializer.Serialize(dtos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("quests.json", jsonQuest);

            //string jsonString2 = JsonSerializer.Serialize(itemDb, new JsonSerializerOptions { WriteIndented = true });
            //File.WriteAllText(itemDBPath, jsonString2);
        }
        static void LoadPlayerData()
        {
            if (File.Exists(playerDataPath))
            {
                //json 파일 읽어오기
                string jsonTemp = File.ReadAllText(playerDataPath);
                player = (JsonSerializer.Deserialize<Player>(jsonTemp));
                player.LoadItemList(itemDb);
                player.InitPlayerQuest(QuestDb);
            }
            else
            {
                Console.WriteLine("저장된 데이터가 없습니다."); // 저장된 데이터가 없다고 뜨고 StartScreen으로 바로넘어가니까 콘솔창이 클리어되서 이 문구가 안보임 -> 여유되면 고치기 --> 수정 완료
                Console.ReadKey();
                StartScreen();
            }
        }
        #endregion
        #region 파티원
        static void DisplayRecruitPartyMember()
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("[파티원 모집]");
                Console.WriteLine();

                for (int i = 1; i < pm.PartyMembers.Count + 1; i++)
                {
                    Console.WriteLine($"{i}. {pm.PartyMembers[i - 1].Name} ({pm.PartyMembers[i - 1].Job})");
                }

                Console.WriteLine();
                Console.Write("[현재 파티원]");
                Console.WriteLine();
                if (pm.OwnedPartyMembers.Count == 0)
                {
                    Console.WriteLine("현재 파티원은 없습니다.");
                }
                else
                {
                    for (int i = 1; i < pm.OwnedPartyMembers.Count + 1; i++)
                    {
                        Console.WriteLine($"{i}. {pm.OwnedPartyMembers[i - 1].Name} ({pm.OwnedPartyMembers[i - 1].Job})");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("1. 파티원 모집");
                Console.WriteLine("2. 파티원 방출");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                int result = CheckInput(0, 2);
                Console.WriteLine();
                switch (result)
                {
                    case 1:
                        if (pm.OwnedPartyMembers.Count > 1)
                        {
                            Console.WriteLine("파티원은 최대 2명까지 보유할 수 있습니다.");
                            Thread.Sleep(500);
                        }
                        else
                        {
                            Console.WriteLine("모집하고자 하는 파티원을 선택해주세요.");
                            int userChoice = CheckInput(1, pm.PartyMembers.Count) - 1;
                            pm.OwnedPartyMembers.Add(pm.PartyMembers[userChoice]);
                            pm.PartyMembers.Remove(pm.PartyMembers[userChoice]);

                        }
                        break;
                    case 2:
                        if (pm.OwnedPartyMembers.Count == 0)
                        {
                            Console.WriteLine("파티원이 없습니다.");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("방출하고자 하는 파티원을 선택해주세요.");
                            int userChoice = CheckInput(1, pm.OwnedPartyMembers.Count) - 1;
                            pm.PartyMembers.Add(pm.OwnedPartyMembers[userChoice]);
                            pm.OwnedPartyMembers.Remove(pm.OwnedPartyMembers[userChoice]);

                        }
                        break;
                    case 0:
                        return;
                }
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
