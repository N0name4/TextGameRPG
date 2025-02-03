namespace TextGameRPG
{
    public class Item
    {
        public int ItemType { get; set; }
        public string Name { get; set; }
        public int Enhancement { get; set; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int Ap { get; set; }
        public int Atk { get; set; }
        public int Int { get; set; }
        public int Def { get; set; }
        public int Dodge { get; set; }
        public int Critical { get; set; }
        public int ActSpd { get; set; }

        public int Price { get; set; }
        public string Description { get; set; }


        //기본 아이템 생성자
        public Item(int type, string name, int Enhanced, int Hp, int Mp, int Ap, int Int, int Def, int Dodge, int Critical, int ActSpd, int Price, string Description)
        {
            this.ItemType = type;
            this.Name = name;
            this.Enhancement = Enhanced;
            this.Hp = Hp;
            this.Ap = Ap;
            this.Mp = Mp;
            this.Int = Int;
            this.Def = Def;
            this.Dodge = Dodge;
            this.Critical = Critical;
            this.ActSpd = ActSpd;
            this.Price = Price;
            this.Description = Description;
        }

        public void DisplayItem()       //스탯 보여주기
        {
            Console.WriteLine($"[아이템 정보]");
            Console.WriteLine($"이름: {Name} {(Enhancement > 0 ? $"+{Enhancement}" : "")}");
            Console.WriteLine($"종류: {(ItemType == 1 ? "무기" : "방어구")}");
            Console.WriteLine($"설명: {Description}");
            Console.WriteLine($"가격: {Price} G");

             void DisplayStat(string statName, int statValue)
             {
                if (statValue > 0) Console.WriteLine($"{statName}: {statValue}");
             }

             // 0이 아닌 속성만 출력
             DisplayStat("체력", Hp);
             DisplayStat("마나", Mp);
             DisplayStat("행동력", Ap);
             DisplayStat("공격력", Atk);
             DisplayStat("방어력", Def);
             DisplayStat("마력", Int);
             DisplayStat("회피율", Dodge);
             DisplayStat("치명타율", Critical);
             DisplayStat("행동 속도", ActSpd);
        }

    }
}