

namespace TextGameRPG
{

    public static class Utility
    {
        public static int GetValidInput(int min, int max)
        {
            while (true)
            {
                string? inputStr = Console.ReadLine();
                if (string.IsNullOrEmpty(inputStr))
                {
                    Console.WriteLine("입력이 없습니다. 유효한 입력을 해주세요.");
                    continue;
                }
                if (!int.TryParse(inputStr, out int input) || input < min || input > max)
                {
                    Console.WriteLine($"유효하지 않은 입력입니다. {min}~{max} 사이의 숫자를 입력해주세요.");
                    continue;
                }
                Console.WriteLine();
                return input;
            }
        }
    }

    public class HomeScreen
    {
        private static HomeScreen? staticInstance;

        private HomeScreen() { }

        public static HomeScreen Instance()
        {
            if (staticInstance == null)
            {
                staticInstance = new HomeScreen();
            }
            return staticInstance;
        }

        public void Gamestart(SaveLoad saveLoad, Player player, Inventory playerInventory, Dungeon dungeon)
        {
            int input = 0;
            while (input != 3)
            {
                Console.WriteLine("1. 새로 시작하기\n2. 불러오기\n3. 나가기");
                input = Utility.GetValidInput(1, 3);
                Console.WriteLine();
                switch (input)
                {
                    case 1:
                        Console.WriteLine("새 게임을 시작합니다.");
                        MakePlayer(player);
                        ActionSelect(saveLoad, player, playerInventory, dungeon);
                        break;
                    case 2:
                        Console.WriteLine("게임을 불러옵니다.");
                        saveLoad.Load(1);
                        ActionSelect(saveLoad, player,playerInventory, dungeon);
                        break;
                    case 3:
                        Console.WriteLine("게임을 종료합니다.");
                        break;
                }
            }
        }

        public void MakePlayer(Player player)
        {
            Console.WriteLine("플레이어 이름을 입력하세요");
            string name = Console.ReadLine();
            player.Name = string.IsNullOrEmpty(name) ? "Noname" : name;

            Console.WriteLine("원하시는 직업을 선택하세요");
            Console.WriteLine("1. 전사, 2. 마법사, 3. 도적, 4. 사제");
            int result = Utility.GetValidInput(1, 4);
           
            player.Class = result;
            player.ApplyClassStats(result);
            Console.WriteLine($"\n[선택 완료] {player.Name} 님은 {(player.Class == 1 ? "전사" : player.Class == 2 ? "마법사" : player.Class == 3 ? "도적" : "사제")} 입니다!");
           
        }

        public void ActionSelect(SaveLoad saveLoad, Player player, Inventory playerInventory, Dungeon dungeon)
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            int input = 0;
            while (input != 8)   //1. 상태, 2. 인벤토리, 3. 상점, 4. 던전, 5. 휴식, 6. 세이브, 7. 로드, 8. 종료
            {
                Shop shop = new Shop(8);
                Console.WriteLine("마을에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine("1. 상태 보기\n2. 인벤토리\n3. 상점\n4. 던전 입장\n5. 휴식하기\n6. 세이브\n7. 로드\n8. 종료");
                input = Utility.GetValidInput(1, 8);
                switch (input)
                {
                    case 1:
                        Console.WriteLine("현재 플레이어 상태를 확인합니다.");
                        player.DisplayStats();
                        break;
                    case 2:
                        Console.WriteLine("인벤토리를 확인합니다.");
                        playerInventory.OpenInventory(player, playerInventory);
                        break;
                    case 3:
                        Console.WriteLine("상점을 확인합니다.");
                        shop.OpenShop(player, playerInventory);
                        break;
                    case 4:
                        dungeon.EnterDungeon(player, playerInventory);
                        break;
                    case 5:
                        Console.WriteLine("500원을 내고 휴식을 취하시겠습니까? 1. 예, 2. 아니오");
                        int restChoice = Utility.GetValidInput(1, 2);
                        if (restChoice == 1)
                        {
                            player.Rest();
                        }
                        else
                        {
                            Console.WriteLine("휴식을 취하지 않았습니다.");
                        }
                        break;
                    case 6:
                        saveLoad.Save(1);
                        break;
                    case 7:
                        saveLoad.Load(1);
                        break;
                    case 8:
                        Console.WriteLine("시작 화면으로 돌아갑니다.");
                        break;
                }
            }
        }        
    }


}