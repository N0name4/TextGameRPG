namespace TextGameRPG
{
    public class SaveLoad
    {
        private const string SaveDirectory = "Saves";
        private const string SaveFilePattern = "save_{0}.csv";
        private const int MaxSlots = 5; // 최대 5개의 슬롯 지원 하고 싶다.

        public Player Player { get; set; }
        public Inventory Inventory { get; set; }
        public Dungeon Dungeon { get; set; }

        //생성자
        public SaveLoad(Player player, Inventory inventory, Dungeon dungeon)
        {
            this.Player = player;
            this.Inventory = inventory;
            this.Dungeon = dungeon;
            //저장 디렉터리가 없으면 새로 생성
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }

        //세이브 함수
        public void Save(int slotNum)
        {
            if (slotNum < 1 || slotNum > MaxSlots)
            {
                Console.WriteLine("올바른 슬롯 번호를 입력하세요! (1~5)");
                return;
            }
            //경로 저장
            string filePath = Path.Combine(SaveDirectory, string.Format(SaveFilePattern, slotNum));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // 플레이어 전체 정보 저장
                writer.WriteLine($"{Player.Name},{Player.Class},{Player.Level},{Player.Gold},{Player.CurrentHp},{Player.CurrentMp},{Player.CurrentAp},{Player.Atk},{Player.Def},{Player.Int},{Player.Dodge},{Player.Critical},{Player.ActSpd},{Player.ExpLimit},{Player.CurrentExp}");

                // 장착 아이템 저장
                writer.WriteLine(Player.EquipedWeapon != null ? $"Weapon,{Player.EquipedWeapon.Name},{Player.EquipedWeapon.Enhancement},{Player.EquipedWeapon.Hp},{Player.EquipedWeapon.Mp},{Player.EquipedWeapon.Ap},{Player.EquipedWeapon.Atk},{Player.EquipedWeapon.Def},{Player.EquipedWeapon.Int},{Player.EquipedWeapon.Dodge},{Player.EquipedWeapon.Critical},{Player.EquipedWeapon.ActSpd},{Player.EquipedWeapon.Price},{Player.EquipedWeapon.Description}" : "Weapon,None");
                writer.WriteLine(Player.EquipedArmor != null ? $"Armor,{Player.EquipedArmor.Name},{Player.EquipedArmor.Enhancement},{Player.EquipedArmor.Hp},{Player.EquipedArmor.Mp},{Player.EquipedArmor.Ap},{Player.EquipedArmor.Atk},{Player.EquipedArmor.Def},{Player.EquipedArmor.Int},{Player.EquipedArmor.Dodge},{Player.EquipedArmor.Critical},{Player.EquipedArmor.ActSpd},{Player.EquipedArmor.Price},{Player.EquipedArmor.Description}" : "Armor,None");

                // 던전 층 정보 저장
                writer.WriteLine(Dungeon.CurrentFloor);

                // 인벤토리 저장 (아이템 전체 정보 포함)
                foreach (var item in Inventory.GetItems())
                {
                    writer.WriteLine($"{item.ItemType},{item.Name},{item.Enhancement},{item.Hp},{item.Mp},{item.Ap},{item.Atk},{item.Def},{item.Int},{item.Dodge},{item.Critical},{item.ActSpd},{item.Price},{item.Description}");
                }
            }
            Console.WriteLine($"게임이 저장되었습니다. (슬롯 {slotNum})");
        }
        //로드 함수
        public void Load(int slotNum)
        {
            if (slotNum < 1 || slotNum > MaxSlots)
            {
                Console.WriteLine("올바른 슬롯 번호를 입력하세요! (1~5)");
                return;
            }
            //파일 경로 설정
            string filePath = Path.Combine(SaveDirectory, string.Format(SaveFilePattern, slotNum));
            if (!File.Exists(filePath))
            {
                Console.WriteLine("해당 슬롯에 저장된 파일이 없습니다.");
                return;
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                // 플레이어 정보 로드
                string[] playerData = reader.ReadLine().Split(',');
                Player.Name = playerData[0];
                Player.Class = int.Parse(playerData[1]);
                Player.Level = int.Parse(playerData[2]);
                Player.Gold = int.Parse(playerData[3]);
                Player.CurrentHp = int.Parse(playerData[4]);
                Player.CurrentMp = int.Parse(playerData[5]);
                Player.CurrentAp = int.Parse(playerData[6]);
                Player.Atk = int.Parse(playerData[7]);
                Player.Def = int.Parse(playerData[8]);
                Player.Int = int.Parse(playerData[9]);
                Player.Dodge = int.Parse(playerData[10]);
                Player.Critical = int.Parse(playerData[11]);
                Player.ActSpd = int.Parse(playerData[12]);
                Player.ExpLimit = int.Parse(playerData[13]);
                Player.CurrentExp = int.Parse(playerData[14]);

                // 장착 아이템 로드
                string[] weaponData = reader.ReadLine().Split(',');
                if (weaponData[1] != "None")
                    Player.EquipedWeapon = new Item(0, weaponData[1], int.Parse(weaponData[2]), int.Parse(weaponData[3]), int.Parse(weaponData[4]), int.Parse(weaponData[5]), int.Parse(weaponData[6]), int.Parse(weaponData[7]), int.Parse(weaponData[8]), int.Parse(weaponData[9]), int.Parse(weaponData[10]), int.Parse(weaponData[11]), int.Parse(weaponData[12]), weaponData[13]);

                string[] armorData = reader.ReadLine().Split(',');
                if (armorData[1] != "None")
                    Player.EquipedArmor = new Item(1, armorData[1], int.Parse(armorData[2]), int.Parse(armorData[3]), int.Parse(armorData[4]), int.Parse(armorData[5]), int.Parse(armorData[6]), int.Parse(armorData[7]), int.Parse(armorData[8]), int.Parse(armorData[9]), int.Parse(armorData[10]), int.Parse(armorData[11]), int.Parse(armorData[12]), armorData[13]);

                // 던전 층 정보 로드
                Dungeon.MoveToFloor(int.Parse(reader.ReadLine()));

                // 인벤토리 로드
                Inventory.ClearInventory();
                while (!reader.EndOfStream)
                {
                    string[] itemData = reader.ReadLine().Split(',');
                    if (itemData[0] == "1" || itemData[0] == "0")
                    {
                        Item newItem = new Item(int.Parse(itemData[0]), itemData[1], int.Parse(itemData[2]), int.Parse(itemData[3]), int.Parse(itemData[4]), int.Parse(itemData[5]), int.Parse(itemData[6]), int.Parse(itemData[7]), int.Parse(itemData[8]), int.Parse(itemData[9]), int.Parse(itemData[10]), int.Parse(itemData[11]), int.Parse(itemData[12]), itemData[13]);
                        Inventory.AddItem(newItem);
                    }
                }
            }
            Console.WriteLine($"게임이 로드되었습니다. (슬롯 {slotNum})");
        }
    }
}