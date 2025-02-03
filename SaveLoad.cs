namespace TextGameRPG
{
    public class SaveLoad
    {
        private const string SaveDirectory = "Saves";
        private const string SaveFilePattern = "save_{0}.csv";
       
        public Player Player { get; set; }

        public Inventory Inventory { get; set; }

        public Dungeon Dungeon { get; set; }

       SaveLoad(Player player, Inventory inventory, Dungeon dungeon)
       {
            this.Player = player;
            this.Inventory = inventory;
            this.Dungeon = dungeon;

            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
       }

        public void Save(int SlotNum)
        {
            string filePath;
        }

        public void Load()
        {

        }

    }

}