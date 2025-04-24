using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    internal class Item
    {
        public int Id { get; }
        public string Name { get; }
        public int Type { get; }
        public int Value { get; }
        public string Desc { get; }
        public int Price { get; }
        public float ItemDropRate { get; set; } //아이템 개별 드랍확률을 위해 추가

        public string DisplayTypeText
        {
            get
            {
                return Type == 0 ? "공격력" : "방어력";
            }
        }


        public Item(int id, string name, int type, int value, string desc, int price, float itemDropRate) // Type 0 : 무기 Type 1: 방어구 Type 2: 기타아이템 Type 3: 회복 아이템

        {
            Id = id;
            Name = name;
            Type = type;
            Value = value;
            Desc = desc;
            Price = price;
            ItemDropRate = itemDropRate;
        }

        public string ItemInfoText()
        {
            return $"{PadingKorean(Name,20)}  |  {DisplayTypeText} +{Value,-2}  |  {PadingKorean(Desc,50)}";
        }

        public static string PadingKorean(string input, int width)
        {
            int len = 0;
            foreach (char c in input)
            {
                len += (c >= 0xAC00 && c <= 0xD7A3) ? 2 : 1;
            }
            int pad = width - len;
            if (pad > 0)
                return input + new string(' ', pad);
            return input;
        }


    }
}
