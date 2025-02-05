using System;

namespace TextGameRPG
{
    public class Inventory
    {
        public int MaxContainable { get; private set; } //인벤 최대 크기
        private List<Item> Items; // 아이템 개수 관리
       

        public Inventory(int maxSize)
        {
            MaxContainable = maxSize;
            Items = new List<Item>();
        }

       //아이템 추가 메서드
        public bool AddItem(Item item)
        {
            if (Items.Count >= MaxContainable)  //가득 참 확인
            {
                Console.WriteLine("인벤토리가 가득 찼습니다.");
                return false;
            }

            Items.Add(item); // 중복 아이템도 개별 추가
            Console.WriteLine($"{item.Name} (+{item.Enhancement}) 추가됨.");
            return true;
        }

        // 아이템 제거 메서드 (객체로 직접 제거)
        public bool RemoveItem(Item item)
        {
            if (!Items.Contains(item))
            {
                Console.WriteLine("해당 아이템이 없습니다.");
                return false;
            }

            Items.Remove(item);
            Console.WriteLine($"{item.Name} (+{item.Enhancement}) 제거됨.");
            return true;
        }
        //인벤 표시 메서드
        public void DisplayInventory(Player player)
        {
            Console.WriteLine("\n===== 인벤토리 =====\n");
            if (Items.Count == 0)
            {
                Console.WriteLine("비어 있음");
                return;
            }

            int index = 1;
            foreach (var item in Items)     //장비된 아이템은 [E]표시
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

       //인벤 초기화 메서드(로드 시 사용)
        public void ClearInventory()
        {
            Items.Clear();
            Console.WriteLine("인벤토리가 초기화되었습니다.");
        }
        //현재 보유 아이템 리스트를 반환
        public List<Item> GetItems()
        {
            return new List<Item>(Items);
        }
        //인벤토리 관리 화면
        public void OpenInventory(Player player, Inventory inventory)
        {
            int choice = 0;
            while (choice != 4)
            {
                Console.WriteLine("\n");
                DisplayInventory(player);
                Console.WriteLine("1. 아이템 장착\n2. 아이템 해제\n3. 아이템 버리기\n4. 나가기");
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
                        Console.WriteLine("인벤토리를 닫습니다.");
                        break;
                }
            }
        }
        //아이템 장착
        public void EquipItem(Player player)
        {
            Console.WriteLine("===== 장착 관리 =====");
            DisplayInventory(player);

            Console.WriteLine("장착할 아이템 번호를 입력하세요 (취소: 0): ");
            int input = Utility.GetValidInput(0, Items.Count);

            if (input == 0)
            {
                Console.WriteLine("장착을 취소했습니다.");
                return;
            }

            Item selectedItem = Items[input - 1];

            if (selectedItem.ItemType == 0) // 무기
            {
                if (player.EquipedWeapon == selectedItem)
                {
                    Console.WriteLine($"{selectedItem.Name}을(를) 장착 해제했습니다.");
                    player.EquipedWeapon = null;
                }
                else
                {
                    if (player.EquipedWeapon != null)
                    {
                        Console.WriteLine($"{player.EquipedWeapon.Name}을(를) 해제하고 {selectedItem.Name}을(를) 장착했습니다.");
                    }
                    else
                    {
                        Console.WriteLine($"{selectedItem.Name}을(를) 장착했습니다.");
                    }
                    player.EquipedWeapon = selectedItem;
                }
            }
            else if (selectedItem.ItemType == 1) // 방어구
            {
                if (player.EquipedArmor == selectedItem)
                {
                    Console.WriteLine($"{selectedItem.Name}을(를) 장착 해제했습니다.");
                    player.EquipedArmor = null;
                }
                else
                {
                    if (player.EquipedArmor != null)
                    {
                        Console.WriteLine($"{player.EquipedArmor.Name}을(를) 해제하고 {selectedItem.Name}을(를) 장착했습니다.");
                    }
                    else
                    {
                        Console.WriteLine($"{selectedItem.Name}을(를) 장착했습니다.");
                    }
                    player.EquipedArmor = selectedItem;
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
        //아이템 해제
        public void UnequipItem(Player player)
        {
            Console.WriteLine("해제할 아이템을 선택하세요:");
            Console.WriteLine("1. 무기 해제");
            Console.WriteLine("2. 방어구 해제");
            Console.WriteLine("3. 취소");

            int input = Utility.GetValidInput(1, 3);
            if (input == 3)
            {
                Console.WriteLine("해제를 취소했습니다.");
                return;
            }

            if (input == 1 && player.EquipedWeapon != null)
            {
                Console.WriteLine($"{player.EquipedWeapon.Name}을(를) 장착 해제했습니다.");
                player.UnequipItem(player.EquipedWeapon);
            }
            else if (input == 2 && player.EquipedArmor != null)
            {
                Console.WriteLine($"{player.EquipedArmor.Name}을(를) 장착 해제했습니다.");
                player.UnequipItem(player.EquipedArmor);
            }
            else
            {
                Console.WriteLine("장착된 아이템이 없습니다.");
            }
        }
        //아이템 버리기
        public void DropItem(Player player)
        {
            Console.WriteLine("\n===== 아이템 버리기 =====");
            DisplayInventory(player);

            if (Items.Count == 0)
            {
                Console.WriteLine("인벤토리에 버릴 아이템이 없습니다.");
                return;
            }
            Console.Write("\n버릴 아이템 번호를 입력하세요 (취소: 0): ");
            int itemIndex = Utility.GetValidInput(0, Items.Count);
            if (itemIndex == 0) return;

            Item item = Items[itemIndex - 1]; // 선택한 아이템

            RemoveItem(item);
            Console.WriteLine($" {item.Name} 을 버렸습니다.");
        }

    }
}