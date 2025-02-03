

namespace TextGameRPG
{
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

        public void Gamestart()
        {
            int input = 0;
            while (input != 3)
            {
                Console.WriteLine("1. 새로 시작하기\n2. 불러오기\n3. 나가기");
                input = GetValidInput(1, 3);
                switch (input)
                {
                    case 1:
                        Console.WriteLine("새 게임을 시작합니다.");
                        
                        break;
                    case 2:
                        Console.WriteLine("게임을 불러옵니다.");
                        //게임 로드 로직
                        break;
                    case 3:
                        Console.WriteLine("게임을 종료합니다.");
                        break;
                }
            }
        }

        public void MakePlayer()
        {
            Console.WriteLine("원하시는 직업을 선택하세요");
            Console.WriteLine("1. 전사, 2. 마법사, 3. 도적, 4. 사제, 5. 직업 설명");



        }

        public void ActionSelect()
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            int input = 0;
            while (input != 8)   //1. 상태, 2. 인벤토리, 3. 상점, 4. 던전, 5. 휴식, 6. 세이브, 7. 로드, 8. 종료
            {
                Console.WriteLine("마을에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine("1. 상태 보기\n2. 인벤토리\n3. 상점\n4. 던전 입장\n5. 휴식하기\n6. 세이브\n7. 로드\n8. 종료");
                input = GetValidInput(1, 8);
                switch (input)
                {
                    case 1:
                        Console.WriteLine("현재 플레이어 상태를 확인합니다.");
                        //플레이어 확인 창
                        break;
                    case 2:
                        Console.WriteLine("인벤토리를 확인합니다.");
                        //인벤토리 확인 창
                        break;
                    case 3:
                        Console.WriteLine("상점을 확인합니다.");
                        //상점 정보 창. 구매, 판매, 강화 등의 요소 진행 가능
                        break;
                    case 4:
                        //던전 관련 코드
                        break;
                    case 5:
                        //휴식 코드
                        break;
                    case 6:
                        //세이브 코드
                        break;
                    case 7:
                        //로드 코드
                        break;
                    case 8:
                        //전 화면 돌아가기
                        break;
                }
            }
        }

        //유효 확인 메서드
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
                return input;
            }
        }
    }
}