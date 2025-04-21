using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    internal class ItemManager
    {
        private static ItemManager _instance;
        public static ItemManager Instance => _instance ??= new ItemManager();
        private ItemManager() { }

        private Dictionary<int, Item> itemDict = new();
        private List<Item> items = new();

        public void LoadItems()
        {
            string json = File.ReadAllText(TextMessages.itemListFileFath);
            items = JsonSerializer.Deserialize<List<Item>>(json);
            foreach (var item in items)
            {
                itemDict[item.id] = item;
            }
        }
        public void ShowItems(bool isBuyMode = false)
        {
            if (!isBuyMode)
            {
                foreach (var item in items)
                {
                    Console.Write("-");
                    item.ViewInfo(ViewMode.Shop_Buy);
                }
            }
            else
            {
                int i = 1;
                foreach (var item in items)
                {
                    Console.Write("- {0} ", i++);
                    item.ViewInfo(ViewMode.Shop_Buy);
                }
            }
            Console.WriteLine();
        }
        public void BuyItem(int index)
        {
            Player p = PlayerManager.Instance.MainPlayer;
            if (index > items.Count())
            {
                GameManager.Instance.WrongInput();
                return;
            }
            Item item = items[--index];
            if (item.isSold)
            {
                Console.WriteLine("이미 구매한 아이템입니다.(계속하려면 아무 키나 입력)");
                Console.ReadKey();
                return;
            }
            if (item.price > p.gold)
            {
                Console.WriteLine("Gold가 부족합니다.(계속하려면 아무 키나 입력)");
                Console.ReadKey();
                return;
            }
            else
            {
                item.ItemSold();
                Console.WriteLine("구매를 완료했습니다.(계속하려면 아무 키나 입력)");
                Console.ReadKey();
                return;
            }
        }
        public Item? GetItembyId(int id)
        {
            return itemDict.TryGetValue(id, out var item) ? item : null;
        }
        public void SaveData()
        {
            string jsonString = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(TextMessages.itemListFileFath, jsonString);
        }
    }
}
