using System;

namespace TextGameRPG
{
    public class Inventory
    {
        public int MaxContainable { get; private set; } //�κ� �ִ� ũ��
        private List<Item> Items; // ������ ���� ����
       

        public Inventory(int maxSize)
        {
            MaxContainable = maxSize;
            Items = new List<Item>();
        }

       //������ �߰� �޼���
        public bool AddItem(Item item)
        {
            if (Items.Count >= MaxContainable)  //���� �� Ȯ��
            {
                Console.WriteLine("�κ��丮�� ���� á���ϴ�.");
                return false;
            }

            Items.Add(item); // �ߺ� �����۵� ���� �߰�
            Console.WriteLine($"{item.Name} (+{item.Enhancement}) �߰���.");
            return true;
        }

        // ������ ���� �޼��� (��ü�� ���� ����)
        public bool RemoveItem(Item item)
        {
            if (!Items.Contains(item))
            {
                Console.WriteLine("�ش� �������� �����ϴ�.");
                return false;
            }

            Items.Remove(item);
            Console.WriteLine($"{item.Name} (+{item.Enhancement}) ���ŵ�.");
            return true;
        }
        //�κ� ǥ�� �޼���
        public void DisplayInventory(Player player)
        {
            Console.WriteLine("\n===== �κ��丮 =====\n");
            if (Items.Count == 0)
            {
                Console.WriteLine("��� ����");
                return;
            }

            int index = 1;
            foreach (var item in Items)     //���� �������� [E]ǥ��
            {
                string equippedMark = "";
                if (player.EquipedWeapon == item) equippedMark = "[E] ";
                if (player.EquipedArmor == item) equippedMark = "[E] ";

                string enhancementMark = item.Enhancement > 0 ? $"+{item.Enhancement}" : "";

                Console.Write($"- {index} {equippedMark}");
                item.DisplayItem();
                index++;
            }
        }

       //�κ� �ʱ�ȭ �޼���(�ε� �� ���)
        public void ClearInventory()
        {
            Items.Clear();
            Console.WriteLine("�κ��丮�� �ʱ�ȭ�Ǿ����ϴ�.");
        }
        //���� ���� ������ ����Ʈ�� ��ȯ
        public List<Item> GetItems()
        {
            return new List<Item>(Items);
        }
        //�κ��丮 ���� ȭ��
        public void OpenInventory(Player player, Inventory inventory)
        {
            int choice = 0;
            while (choice != 4)
            {
                Console.WriteLine("\n");
                DisplayInventory(player);
                Console.WriteLine("1. ������ ����\n2. ������ ����\n3. ������ ������\n4. ������");
                choice = Utility.GetValidInput(1, 4);

                switch (choice)
                {
                    case 1:
                        EquipItem(player);
                        break;
                    case 2:
                        UnequipItem(player);
                        break;
                    case 3:
                        DropItem(player);
                        break;
                    case 4:
                        Console.WriteLine("�κ��丮�� �ݽ��ϴ�.");
                        break;
                }
            }
        }
        //������ ����
        public void EquipItem(Player player)
        {
            Console.WriteLine("===== ���� ���� =====");
            DisplayInventory(player);

            Console.WriteLine("������ ������ ��ȣ�� �Է��ϼ��� (���: 0): ");
            int input = Utility.GetValidInput(0, Items.Count);

            if (input == 0)
            {
                Console.WriteLine("������ ����߽��ϴ�.");
                return;
            }

            Item selectedItem = Items[input - 1];

            if (selectedItem.ItemType == 0) // ����
            {
                if (player.EquipedWeapon == selectedItem)
                {
                    Console.WriteLine($"{selectedItem.Name}��(��) ���� �����߽��ϴ�.");
                    player.EquipedWeapon = null;
                }
                else
                {
                    if (player.EquipedWeapon != null)
                    {
                        Console.WriteLine($"{player.EquipedWeapon.Name}��(��) �����ϰ� {selectedItem.Name}��(��) �����߽��ϴ�.");
                    }
                    else
                    {
                        Console.WriteLine($"{selectedItem.Name}��(��) �����߽��ϴ�.");
                    }
                    player.EquipedWeapon = selectedItem;
                }
            }
            else if (selectedItem.ItemType == 1) // ��
            {
                if (player.EquipedArmor == selectedItem)
                {
                    Console.WriteLine($"{selectedItem.Name}��(��) ���� �����߽��ϴ�.");
                    player.EquipedArmor = null;
                }
                else
                {
                    if (player.EquipedArmor != null)
                    {
                        Console.WriteLine($"{player.EquipedArmor.Name}��(��) �����ϰ� {selectedItem.Name}��(��) �����߽��ϴ�.");
                    }
                    else
                    {
                        Console.WriteLine($"{selectedItem.Name}��(��) �����߽��ϴ�.");
                    }
                    player.EquipedArmor = selectedItem;
                }
            }
            else
            {
                Console.WriteLine("�߸��� �Է��Դϴ�.");
            }
        }
        //������ ����
        public void UnequipItem(Player player)
        {
            Console.WriteLine("������ �������� �����ϼ���:");
            Console.WriteLine("1. ���� ����");
            Console.WriteLine("2. �� ����");
            Console.WriteLine("3. ���");

            int input = Utility.GetValidInput(1, 3);
            if (input == 3)
            {
                Console.WriteLine("������ ����߽��ϴ�.");
                return;
            }

            if (input == 1 && player.EquipedWeapon != null)
            {
                Console.WriteLine($"{player.EquipedWeapon.Name}��(��) ���� �����߽��ϴ�.");
                player.UnequipItem(player.EquipedWeapon);
            }
            else if (input == 2 && player.EquipedArmor != null)
            {
                Console.WriteLine($"{player.EquipedArmor.Name}��(��) ���� �����߽��ϴ�.");
                player.UnequipItem(player.EquipedArmor);
            }
            else
            {
                Console.WriteLine("������ �������� �����ϴ�.");
            }
        }
        //������ ������
        public void DropItem(Player player)
        {
            Console.WriteLine("\n===== ������ ������ =====");
            DisplayInventory(player);

            if (Items.Count == 0)
            {
                Console.WriteLine("�κ��丮�� ���� �������� �����ϴ�.");
                return;
            }
            Console.Write("\n���� ������ ��ȣ�� �Է��ϼ��� (���: 0): ");
            int itemIndex = Utility.GetValidInput(0, Items.Count);
            if (itemIndex == 0) return;

            Item item = Items[itemIndex - 1]; // ������ ������

            RemoveItem(item);
            Console.WriteLine($" {item.Name} �� ���Ƚ��ϴ�.");
        }

    }
}