namespace TextGameRPG
{
    public class Inventory
    {
        public int MaxContainable { get; set; }
        public List<Item> Items { get; set; }

        public Inventory(int maxSize)
        {
            MaxContainable = maxSize;
            Items = new List<Item>();
        }

        public bool AddItem(Item item)
        {
            if(Items.Count >= MaxContainable)
            {
                Console.WriteLine("인벤토리가 가득 찼습니다. 아이템을 추가할 수 없습니다.");
                return false;
            }

            Items.Add(item);
            Console.WriteLine($"{item.Name}을 인벤토리에 추가하였습니다.");
            return true;

        }

        public bool RemoveItem(Item item)
        {
            if(Items.Remove(item))
            {
                Console.WriteLine($"{item.Name}을(를) 인벤토리에서 삭제하였습니다.");
                return true;
            }
            Console.WriteLine("아이템이 인벤토리에 없습니다.");
            return false;
        }

        public void DisplayITems()
        {
            Console.WriteLine();
        }

        public void EquipItem(Player player, Item item)
        {

        }
    }
}