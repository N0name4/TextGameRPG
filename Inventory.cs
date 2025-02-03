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
                Console.WriteLine("�κ��丮�� ���� á���ϴ�. �������� �߰��� �� �����ϴ�.");
                return false;
            }

            Items.Add(item);
            Console.WriteLine($"{item.Name}�� �κ��丮�� �߰��Ͽ����ϴ�.");
            return true;

        }

        public bool RemoveItem(Item item)
        {
            if(Items.Remove(item))
            {
                Console.WriteLine($"{item.Name}��(��) �κ��丮���� �����Ͽ����ϴ�.");
                return true;
            }
            Console.WriteLine("�������� �κ��丮�� �����ϴ�.");
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