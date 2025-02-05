namespace TextGameRPG
{
    public class Shop
    {
        public int MaxItemNum { get; private set; }     //최대 아이템 개수.
        private List<Item> AvailableItems;              //상점에 있는 아이템
        private List<bool> purchased;                   //이미 구매 하였는지

        public Shop(int maxItemNum)
        {
            MaxItemNum = maxItemNum;
            AvailableItems = new List<Item>();
            purchased = new List<bool>();
            RandomItemSet();
        }

        public void RandomItemSet()
        {
            AvailableItems.Clear();
            List<Item> allItems = ItemDatabase.Instance.GetAllItems();
            Random rand = new Random();
            for (int i = 0; i < MaxItemNum && i < allItems.Count; i++)
            {
                AvailableItems.Add(allItems[rand.Next(allItems.Count)]);
                purchased.Add(false);
            }
        }

        public void DisplayShopItems()
        {
            Console.WriteLine("===== 상점 아이템 목록 =====");
            if (AvailableItems.Count == 0)
            {
                Console.WriteLine("현재 판매 중인 아이템이 없습니다.");
                return;
            }
            int index = 0;
            foreach (var item in AvailableItems)
            {
                Console.WriteLine($"{index+1}. {item.Name} {(purchased[index] ? "| 판매됨" : $"| {item.Price} G")}");
                item.DisplayItem();
                Console.WriteLine();
                index++;
            }
        }

        public void BuyItem(Player player, Inventory inventory, int itemIndex)
        {
            if (itemIndex < 1 || itemIndex > AvailableItems.Count)
            {
                Console.WriteLine("잘못된 입력입니다.");
                return;
            }

            Item item = AvailableItems[itemIndex - 1];

            if (purchased[itemIndex-1])
            {
                Console.WriteLine("이미 구매 완료된 아이템입니다.");
                return;
            }

            if (player.Gold < item.Price)
            {
                Console.WriteLine("골드가 부족합니다.");
                return;
            }

            player.Gold -= item.Price;
            inventory.AddItem(item);
            purchased[itemIndex-1] = true; // 구매 상태 저장

            Console.WriteLine($"{item.Name}을(를) 구매했습니다! 남은 골드: {player.Gold} G");
        }

        public void SellItem(Player player, Inventory inventory, int itemIndex)
        {
            List<Item> playerItems = inventory.GetItems();

            if (itemIndex < 1 || itemIndex > playerItems.Count)
            {
                Console.WriteLine("잘못된 입력입니다.");
                return;
            }

            Item item = playerItems[itemIndex - 1];

            if (!inventory.RemoveItem(item))
            {
                Console.WriteLine("해당 아이템을 판매할 수 없습니다.");
                return;
            }

            int sellPrice = item.Price / 2;
            player.Gold += sellPrice;

            Console.WriteLine($"{item.Name}을(를) 판매했습니다! 현재 골드: {player.Gold} G");
        }


        public void UpgradeItem(Player player, Inventory inventory)
        {
            Console.WriteLine("\n===== 아이템 강화 =====");
            inventory.DisplayInventory(player);

            List<Item> upgradeableItems = inventory.GetItems();

            if (upgradeableItems.Count == 0)
            {
                Console.WriteLine("인벤토리에 강화할 아이템이 없습니다.");
                return;
            }

            Console.Write("\n강화할 아이템 번호를 입력하세요 (취소: 0): ");
            int itemIndex = Utility.GetValidInput(0, upgradeableItems.Count);
            if (itemIndex == 0) return;

            Item item = upgradeableItems[itemIndex - 1]; // 선택한 아이템

            int baseCost = 100 * (item.Enhancement + 1); // 강화 단계에 비례한 비용
            int finalCost = baseCost;

            Console.Write("\n강화에 사용할 아이템 번호를 입력하세요 (없으면 0 입력): ");
            int materialIndex = Utility.GetValidInput(0, upgradeableItems.Count);
            if (materialIndex != 0 && materialIndex != itemIndex)
            {
                Item materialItem = upgradeableItems[materialIndex - 1];

                if (materialItem != null)
                {
                    finalCost -= (materialItem.Enhancement * 100); // 사용한 아이템 강화 단계가 높을수록 비용 절감
                    inventory.RemoveItem(materialItem);
                    Console.WriteLine($"{materialItem.Name}을(를) 재료로 사용하여 비용이 감소했습니다.");
                }
            }

            finalCost = Math.Max(finalCost, 50); // 최소 강화 비용 50G
            if (player.Gold < finalCost)
            {
                Console.WriteLine("골드가 부족합니다.");
                return;
            }

            player.Gold -= finalCost;
            item.Enhancement++;

            Console.WriteLine($"강화 성공! {item.Name}의 강화 단계가 +{item.Enhancement}로 상승했습니다.");
            Console.WriteLine($"남은 골드: {player.Gold} G");

            Console.WriteLine("\n강화할 스탯을 선택하세요:");
            Console.WriteLine("1. HP  2. MP  3. Ap  4. Atk  5. Int  6. Def  7. Dodge  8. Crt  9. Act");
            
            int statChoice = Utility.GetValidInput(1, 9);

            int multiplier = 2;     //기본 스탯 증가량

            switch (player.Class)
            {
                case 1: // 전사: HP, AP, Atk, Def 강화 효과 증가
                    if (statChoice == 1 || statChoice == 3 || statChoice == 4 || statChoice == 6)
                        multiplier = 4;
                    if (statChoice == 1) multiplier = 6; // HP는 가장 많이 증가
                    break;
                case 2: // 마법사: MP, Crt 강화 효과 증가
                    if (statChoice == 2 || statChoice == 8)
                        multiplier = 4;
                    if (statChoice == 2 || statChoice == 5) multiplier = 6; // MP,Int는 가장 많이 증가
                    break;
                case 3: // 도적: AP, Atk, Dodge, ActSpd,Crt 강화 효과 증가
                    if (statChoice == 3 || statChoice == 4 || statChoice == 7 || statChoice == 8||statChoice == 9)
                        multiplier = 4;
                    if (statChoice == 9) multiplier = 6; // 행동 속도는 가장 많이 증가
                    break;
                case 4: // 사제: HP, MP, Int, Def 강화 효과 증가
                    if (statChoice == 1 || statChoice == 2 || statChoice == 5 || statChoice == 6)
                        multiplier = 4;
                    if (statChoice == 5) multiplier = 6; // Int는 가장 많이 증가
                    break;
            }
            int increaseValue = item.Enhancement * multiplier;

            switch (statChoice)
            {
                case 1:
                    item.Hp += increaseValue;
                    Console.WriteLine($"HP가 {increaseValue} 증가했습니다!");
                    break;
                case 2:
                    item.Mp += increaseValue;
                    Console.WriteLine($"MP가 {increaseValue} 증가했습니다!");
                    break;
                case 3:
                    item.Ap += increaseValue;
                    Console.WriteLine($"AP가 {increaseValue} 증가했습니다!");
                    break;
                case 4:
                    item.Atk += increaseValue;
                    Console.WriteLine($"공격력이 {increaseValue} 증가했습니다!");
                    break;
                case 5:
                    item.Int += increaseValue;
                    Console.WriteLine($"지능이 {increaseValue} 증가했습니다!");
                    break;
                case 6:
                    item.Def += increaseValue;
                    Console.WriteLine($"방어력이 {increaseValue} 증가했습니다!");
                    break;
                case 7:
                    item.Dodge += increaseValue;
                    Console.WriteLine($"회피율이 {increaseValue} 증가했습니다!");
                    break;
                case 8:
                    item.Critical += increaseValue;
                    Console.WriteLine($"치명타 확률이 {increaseValue} 증가했습니다!");
                    break;
                case 9:
                    item.ActSpd += increaseValue;
                    Console.WriteLine($"행동 속도가 {increaseValue} 증가했습니다!");
                    break;
            }
        }
        //상점 활성화 코드
        public void OpenShop(Player player, Inventory inventory)
        {
            int choice = 0;
            while (choice != 4)
            {
                DisplayShopItems();
                Console.WriteLine("1. 아이템 구매\n2. 아이템 판매\n3. 아이템 강화\n4. 나가기");
                choice = Utility.GetValidInput(1,4);

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("구매할 아이템 이름을 입력하세요:");
                        int buy = Utility.GetValidInput(1, AvailableItems.Count);
                        BuyItem(player, inventory, buy);
                        break;

                    case 2:
                        Console.WriteLine("판매할 아이템 이름을 입력하세요:");
                        int sell = Utility.GetValidInput(1,inventory.GetItems().Count);
                        SellItem(player, inventory, sell);
                        break;
                    case 3:
                        UpgradeItem(player, inventory);
                        break;
                    case 4:
                        Console.WriteLine("상점을 떠납니다.");
                        break;
                }
            }
        }

    }
}