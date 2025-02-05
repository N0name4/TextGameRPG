namespace TextGameRPG
{
    public class Item
    {
        public int ItemType { get; set; }       // 0 무기 1 방어구
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
        public Item(int type, string name, int Enhanced, int Hp, int Mp, int Ap, int Atk,int Int, int Def, int Dodge, int Critical, int ActSpd, int Price, string Description)
        {
            this.ItemType = type;
            this.Name = name;
            this.Enhancement = Enhanced;
            this.Hp = Hp;
            this.Ap = Ap;
            this.Mp = Mp;
            this.Atk = Atk;
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
            Console.WriteLine($"종류: {(ItemType == 0 ? "무기" : "방어구")}");
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
             Console.WriteLine();
            
        }

    }

    public class ItemDatabase       //메서드 처리를 위해 클래스 분리
    {
        private static ItemDatabase _instance;  // 싱글톤 인스턴스
        private Dictionary<int, List<Item>> itemByFloor = new Dictionary<int, List<Item>>();        //층별 등장 아이템
        private List<Item> itemList; // 전체 아이템 리스트

        private ItemDatabase()
        {
            itemList = new List<Item>();
            InitializeItems();
        }

        public static ItemDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ItemDatabase();
                }
                return _instance;
            }
        }

        private void InitializeItems()
        {
            AddItem(1, new Item(0, "낡은 검", 0, 0, 0, 0, 2, 0, 0, 0, 3, 0, 100, "쉽게 볼 수 있는 낡은 검"));
            AddItem(1, new Item(1, "천 갑옷", 0, 10, 0, 0, 0, 0, 3, 3, 0, 0, 150, "가벼운 천으로 만든 갑옷"));
            AddItem(2, new Item(0, "훈련용 검", 0, 0, 0, 0, 5, 0, 0, 0, 5, 0, 200, "훈련용으로 사용되는 검"));
            AddItem(2, new Item(1, "훈련용 갑옷", 0, 15, 0, 0, 0, 0, 5, 5, 0, 0, 300, "훈련을 위한 방어구"));

            // 3~4층 아이템 (중급 장비)
            AddItem(3, new Item(0, "강철 검", 1, 0, 0, 0, 10, 0, 0, 0, 7, 0, 500, "강철로 만들어진 검"));
            AddItem(3, new Item(1, "강철 갑옷", 1, 30, 0, 0, 0, 0, 10, 8, 0, 0, 700, "강철 재질로 제작된 갑옷"));
            AddItem(4, new Item(0, "도끼", 1, 0, 0, 0, 15, 0, 0, 0, 10, 0, 800, "무거운 전투용 도끼"));
            AddItem(4, new Item(1, "판금 갑옷", 1, 40, 0, 0, 0, 0, 12, 10, 0, 0, 1000, "판금으로 만든 튼튼한 갑옷"));

            // 5~6층 아이템 (고급 장비)
            AddItem(5, new Item(0, "스파르타 창", 2, 0, 0, 0, 20, 0, 0, 0, 12, 0, 1500, "스파르타 전사들이 사용한 창"));
            AddItem(5, new Item(1, "미스릴 갑옷", 2, 50, 0, 0, 0, 0, 15, 12, 0, 0, 2000, "희귀한 미스릴로 만든 갑옷"));
            AddItem(6, new Item(0, "용의 검", 2, 0, 0, 0, 25, 0, 0, 0, 15, 0, 2500, "용을 베었던 전설적인 검"));
            AddItem(6, new Item(1, "드래곤 갑옷", 2, 60, 0, 0, 0, 0, 18, 15, 0, 0, 3000, "드래곤 가죽으로 만든 갑옷"));

            // 7~8층 아이템 (전설 장비)
            AddItem(7, new Item(0, "성검 엑스칼리버", 3, 0, 0, 0, 35, 5, 0, 0, 20, 0, 5000, "성스러운 힘이 깃든 검"));
            AddItem(7, new Item(1, "성스러운 갑옷", 3, 70, 0, 0, 0, 10, 20, 18, 0, 0, 6000, "성스러운 빛이 깃든 갑옷"));
            AddItem(8, new Item(0, "암흑검", 3, 0, 0, 0, 50, 0, 0, 0, 25, 0, 7500, "암흑의 힘이 깃든 검"));
            AddItem(8, new Item(1, "악마의 갑옷", 3, 80, 0, 0, 0, 0, 25, 20, 0, 0, 9000, "악마가 착용했던 전설의 갑옷"));
        }

        private void AddItem(int floor, Item item)
        {
            if (!itemByFloor.ContainsKey(floor))
                itemByFloor[floor] = new List<Item>();

            itemByFloor[floor].Add(item);
            itemList.Add(item);
        }

        public Item GetItemByName(string name)
        {
            foreach (var itemList in itemByFloor.Values)
            {
                var foundItem = itemList.FirstOrDefault(item => item.Name == name);
                if (foundItem != null)
                    return foundItem;
            }

            Console.WriteLine($"[경고] '{name}' 아이템이 존재하지 않습니다.");
            return null;
        }
        public List<Item> GetAllItems()
        {
            return itemByFloor.Values.SelectMany(itemList => itemList).ToList();
        }

        public Item RandomItemGen(int floor)
        {
            Random random = new Random();
            int maxFloor = Math.Min(floor, itemByFloor.Keys.Max());
            List<Item> availableItems = itemByFloor[maxFloor];

            return availableItems[random.Next(availableItems.Count)];
        }
    }
}