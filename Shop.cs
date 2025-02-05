namespace TextGameRPG
{
    public class Shop
    {
        public int MaxItemNum { get; private set; }     //�ִ� ������ ����.
        private List<Item> AvailableItems;              //������ �ִ� ������
        private List<bool> purchased;                   //�̹� ���� �Ͽ�����

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
            Console.WriteLine("===== ���� ������ ��� =====");
            if (AvailableItems.Count == 0)
            {
                Console.WriteLine("���� �Ǹ� ���� �������� �����ϴ�.");
                return;
            }
            int index = 0;
            foreach (var item in AvailableItems)
            {
                Console.WriteLine($"{index+1}. {item.Name} {(purchased[index] ? "| �Ǹŵ�" : $"| {item.Price} G")}");
                item.DisplayItem();
                Console.WriteLine();
                index++;
            }
        }

        public void BuyItem(Player player, Inventory inventory, int itemIndex)
        {
            if (itemIndex < 1 || itemIndex > AvailableItems.Count)
            {
                Console.WriteLine("�߸��� �Է��Դϴ�.");
                return;
            }

            Item item = AvailableItems[itemIndex - 1];

            if (purchased[itemIndex-1])
            {
                Console.WriteLine("�̹� ���� �Ϸ�� �������Դϴ�.");
                return;
            }

            if (player.Gold < item.Price)
            {
                Console.WriteLine("��尡 �����մϴ�.");
                return;
            }

            player.Gold -= item.Price;
            inventory.AddItem(item);
            purchased[itemIndex-1] = true; // ���� ���� ����

            Console.WriteLine($"{item.Name}��(��) �����߽��ϴ�! ���� ���: {player.Gold} G");
        }

        public void SellItem(Player player, Inventory inventory, int itemIndex)
        {
            List<Item> playerItems = inventory.GetItems();

            if (itemIndex < 1 || itemIndex > playerItems.Count)
            {
                Console.WriteLine("�߸��� �Է��Դϴ�.");
                return;
            }

            Item item = playerItems[itemIndex - 1];

            if (!inventory.RemoveItem(item))
            {
                Console.WriteLine("�ش� �������� �Ǹ��� �� �����ϴ�.");
                return;
            }

            int sellPrice = item.Price / 2;
            player.Gold += sellPrice;

            Console.WriteLine($"{item.Name}��(��) �Ǹ��߽��ϴ�! ���� ���: {player.Gold} G");
        }


        public void UpgradeItem(Player player, Inventory inventory)
        {
            Console.WriteLine("\n===== ������ ��ȭ =====");
            inventory.DisplayInventory(player);

            List<Item> upgradeableItems = inventory.GetItems();

            if (upgradeableItems.Count == 0)
            {
                Console.WriteLine("�κ��丮�� ��ȭ�� �������� �����ϴ�.");
                return;
            }

            Console.Write("\n��ȭ�� ������ ��ȣ�� �Է��ϼ��� (���: 0): ");
            int itemIndex = Utility.GetValidInput(0, upgradeableItems.Count);
            if (itemIndex == 0) return;

            Item item = upgradeableItems[itemIndex - 1]; // ������ ������

            int baseCost = 100 * (item.Enhancement + 1); // ��ȭ �ܰ迡 ����� ���
            int finalCost = baseCost;

            Console.Write("\n��ȭ�� ����� ������ ��ȣ�� �Է��ϼ��� (������ 0 �Է�): ");
            int materialIndex = Utility.GetValidInput(0, upgradeableItems.Count);
            if (materialIndex != 0 && materialIndex != itemIndex)
            {
                Item materialItem = upgradeableItems[materialIndex - 1];

                if (materialItem != null)
                {
                    finalCost -= (materialItem.Enhancement * 100); // ����� ������ ��ȭ �ܰ谡 �������� ��� ����
                    inventory.RemoveItem(materialItem);
                    Console.WriteLine($"{materialItem.Name}��(��) ���� ����Ͽ� ����� �����߽��ϴ�.");
                }
            }

            finalCost = Math.Max(finalCost, 50); // �ּ� ��ȭ ��� 50G
            if (player.Gold < finalCost)
            {
                Console.WriteLine("��尡 �����մϴ�.");
                return;
            }

            player.Gold -= finalCost;
            item.Enhancement++;

            Console.WriteLine($"��ȭ ����! {item.Name}�� ��ȭ �ܰ谡 +{item.Enhancement}�� ����߽��ϴ�.");
            Console.WriteLine($"���� ���: {player.Gold} G");

            Console.WriteLine("\n��ȭ�� ������ �����ϼ���:");
            Console.WriteLine("1. HP  2. MP  3. Ap  4. Atk  5. Int  6. Def  7. Dodge  8. Crt  9. Act");
            
            int statChoice = Utility.GetValidInput(1, 9);

            int multiplier = 2;     //�⺻ ���� ������

            switch (player.Class)
            {
                case 1: // ����: HP, AP, Atk, Def ��ȭ ȿ�� ����
                    if (statChoice == 1 || statChoice == 3 || statChoice == 4 || statChoice == 6)
                        multiplier = 4;
                    if (statChoice == 1) multiplier = 6; // HP�� ���� ���� ����
                    break;
                case 2: // ������: MP, Crt ��ȭ ȿ�� ����
                    if (statChoice == 2 || statChoice == 8)
                        multiplier = 4;
                    if (statChoice == 2 || statChoice == 5) multiplier = 6; // MP,Int�� ���� ���� ����
                    break;
                case 3: // ����: AP, Atk, Dodge, ActSpd,Crt ��ȭ ȿ�� ����
                    if (statChoice == 3 || statChoice == 4 || statChoice == 7 || statChoice == 8||statChoice == 9)
                        multiplier = 4;
                    if (statChoice == 9) multiplier = 6; // �ൿ �ӵ��� ���� ���� ����
                    break;
                case 4: // ����: HP, MP, Int, Def ��ȭ ȿ�� ����
                    if (statChoice == 1 || statChoice == 2 || statChoice == 5 || statChoice == 6)
                        multiplier = 4;
                    if (statChoice == 5) multiplier = 6; // Int�� ���� ���� ����
                    break;
            }
            int increaseValue = item.Enhancement * multiplier;

            switch (statChoice)
            {
                case 1:
                    item.Hp += increaseValue;
                    Console.WriteLine($"HP�� {increaseValue} �����߽��ϴ�!");
                    break;
                case 2:
                    item.Mp += increaseValue;
                    Console.WriteLine($"MP�� {increaseValue} �����߽��ϴ�!");
                    break;
                case 3:
                    item.Ap += increaseValue;
                    Console.WriteLine($"AP�� {increaseValue} �����߽��ϴ�!");
                    break;
                case 4:
                    item.Atk += increaseValue;
                    Console.WriteLine($"���ݷ��� {increaseValue} �����߽��ϴ�!");
                    break;
                case 5:
                    item.Int += increaseValue;
                    Console.WriteLine($"������ {increaseValue} �����߽��ϴ�!");
                    break;
                case 6:
                    item.Def += increaseValue;
                    Console.WriteLine($"������ {increaseValue} �����߽��ϴ�!");
                    break;
                case 7:
                    item.Dodge += increaseValue;
                    Console.WriteLine($"ȸ������ {increaseValue} �����߽��ϴ�!");
                    break;
                case 8:
                    item.Critical += increaseValue;
                    Console.WriteLine($"ġ��Ÿ Ȯ���� {increaseValue} �����߽��ϴ�!");
                    break;
                case 9:
                    item.ActSpd += increaseValue;
                    Console.WriteLine($"�ൿ �ӵ��� {increaseValue} �����߽��ϴ�!");
                    break;
            }
        }
        //���� Ȱ��ȭ �ڵ�
        public void OpenShop(Player player, Inventory inventory)
        {
            int choice = 0;
            while (choice != 4)
            {
                DisplayShopItems();
                Console.WriteLine("1. ������ ����\n2. ������ �Ǹ�\n3. ������ ��ȭ\n4. ������");
                choice = Utility.GetValidInput(1,4);

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("������ ������ �̸��� �Է��ϼ���:");
                        int buy = Utility.GetValidInput(1, AvailableItems.Count);
                        BuyItem(player, inventory, buy);
                        break;

                    case 2:
                        Console.WriteLine("�Ǹ��� ������ �̸��� �Է��ϼ���:");
                        int sell = Utility.GetValidInput(1,inventory.GetItems().Count);
                        SellItem(player, inventory, sell);
                        break;
                    case 3:
                        UpgradeItem(player, inventory);
                        break;
                    case 4:
                        Console.WriteLine("������ �����ϴ�.");
                        break;
                }
            }
        }

    }
}