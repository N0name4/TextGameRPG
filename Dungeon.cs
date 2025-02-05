using System;

namespace TextGameRPG
{
    public class Dungeon
    {
        public int CurrentFloor { get; private set; }   // 현재 층 정보
        public int MaxFloor { get; private set; }       // 최대 층 수 제한(쉽게 층 수를 바꿀 수 있다.)
        private Dictionary<int, Room[]> Floors;         // 층별 방의 상태를 저장하기 위함
        private Random random;                          
        private int CurrentRoom;                        //현재 어느 방에 있는지 확인하는 용도

        public Dungeon(int maxFloor)
        {
            MaxFloor = maxFloor;
            CurrentFloor = 1;
            CurrentRoom = 0;
            Floors = new Dictionary<int, Room[]>();
            random = new Random();
            GenerateFloors();
        }

        private void GenerateFloors()       //방을 랜덤하게 생성
        {
            for (int i = 1; i <= MaxFloor; i++)
            {
                Room[] rooms = new Room[8];
                for (int j = 0; j < 8; j++)
                {
                    bool hasEnemy = random.Next(100) < 65; // 65% 확률로 적 등장
                    bool hasItem = !hasEnemy && random.Next(100) < 25; // 적이 없을 경우 25% 확률로 아이템 등장
                    rooms[j] = new Room(hasEnemy, hasItem);
                }
                Floors[i] = rooms;
            }
        }
 

        public void EnterDungeon(Player player, Inventory inventory)        //던전 입장 시 선택지 표시
        {
            Console.WriteLine("던전에 입장합니다. 어떻게 진행하겠습니까?");
            Console.WriteLine("1. 정찰(==탐험) (자동 클리어)\n2. 방 탐험\n3. 층 이동\n4. 마을로 돌아가기");
            int input = Utility.GetValidInput(1, 4);
            switch (input)
            {
                case 1:
                    AutoClearFloor(player);
                    MoveToNextFloor();
                    break;
                case 2:
                    ExploreFloor(player,inventory);
                    break;
                case 3:
                    SelectFloor();
                    break;
                case 4:
                    Console.WriteLine("마을로 돌아갑니다");
                    break;
            }
        }

        public void ExploreFloor(Player player, Inventory inventory)        //던전 탐색 선택 시 방 이동 진행을 맏는 코드
        {
            if (!Floors.ContainsKey(CurrentFloor)) return;

            Console.WriteLine($"던전 {CurrentFloor}층을 탐험합니다...");
            CurrentRoom = 0; // 방 탐험 시작 시 첫 번째 방부터 시작

            while (CurrentRoom < Floors[CurrentFloor].Length)
            {
                Console.WriteLine($"현재 방: {CurrentRoom + 1} / {Floors[CurrentFloor].Length}");
                Floors[CurrentFloor][CurrentRoom].ResolveRoom(player, inventory, CurrentFloor);
                CurrentRoom++;
            }

            Console.WriteLine($"{CurrentFloor}층 탐험 완료!");
            MoveToNextFloor();
        }

        public void AutoClearFloor(Player player)       //정찰 선택시 방어력, 공격력을 비교하여 체력, 결과를 가져온다. 이는 직접 탐색하였을 때보다 보상이 매우 적다.
        {
            if (!Floors.ContainsKey(CurrentFloor)) return;
            int defenseFactor = player.MaxDef;
            int damageTaken = Math.Max(1, 10 - defenseFactor / 5);
            player.CurrentHp -= damageTaken;

            int rewardFactor = player.MaxAtk;
            int goldReward = rewardFactor * 2;
            player.Gold += goldReward;

            Console.WriteLine($"{CurrentFloor}층 정찰 완료! 체력 -{damageTaken}, 골드 +{goldReward}");
        }
        public void MoveToNextFloor()       //다음층으로 이동하는 메소드
        {
            if (CurrentFloor < MaxFloor)
            {
                CurrentFloor++;
                Console.WriteLine($"다음 층으로 이동합니다! 현재 층: {CurrentFloor}층");
            }
            else
            {
                Console.WriteLine("현재 최고층입니다.");
            }
        }
        public void SelectFloor()       //이동할 층 선택하는 메소드
        {
            Console.WriteLine($"현재 층: {CurrentFloor}층");
            Console.WriteLine("이동할 층을 선택하세요 (1~" + MaxFloor + "): ");
            int targetFloor = Utility.GetValidInput(1, MaxFloor);

            if (targetFloor == CurrentFloor)
            {
                Console.WriteLine("이미 현재 층에 있습니다.");
                return;
            }

            MoveToFloor(targetFloor);
        }

        public void MoveToFloor(int targetFloor)        //입력 받은 값으로 이동하는 코드. 위와 합치고 싶은데 세이브에 이미 쓰였네;;;;     
        {
            if (targetFloor < 1 || targetFloor > MaxFloor)
            {
                Console.WriteLine("이동할 수 없는 층입니다.");
                return;
            }

            Console.WriteLine($"{targetFloor}층으로 이동합니다...");
            CurrentFloor = targetFloor;
            CurrentRoom = 0; // 새로운 층에서는 첫 번째 방부터 시작
        }
    }
    public class Room
    {
        public bool HasEnemy { get; private set; }      //적이 있는지
        public bool HasItem { get; private set; }       //아이템이 드롭되었는지

        public Room(bool hasEnemy, bool hasItem)
        {
            HasEnemy = hasEnemy;
            HasItem = hasItem;
        }

        public void ResolveRoom(Player player, Inventory inventory, int currentFloor)       //조우 시 배틀
        {
            if (HasEnemy)
            {
                Enemy enemy = Enemy.RandomGen(currentFloor);
                Console.WriteLine($"{enemy.Name}과 조우했습니다!");

                Battle battle = new Battle(player, enemy);
                battle.WholeBattle();
            }
            else if (HasItem)           //아니면 아이템 랜덤 생성 후 획득
            {
                Console.WriteLine("아이템을 발견했습니다!");
                Item item = ItemDatabase.Instance.RandomItemGen(currentFloor);
                inventory.AddItem(item);
                Console.WriteLine($"'{item.Name}'을(를) 획득했습니다!");
            }
            else
            {
                Console.WriteLine("빈 방이었습니다.");
            }
            Console.WriteLine();
        }
    }
}