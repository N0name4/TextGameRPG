using System;

namespace TextGameRPG
{
    public class Dungeon
    {
        public int CurrentFloor { get; private set; }   // ���� �� ����
        public int MaxFloor { get; private set; }       // �ִ� �� �� ����(���� �� ���� �ٲ� �� �ִ�.)
        private Dictionary<int, Room[]> Floors;         // ���� ���� ���¸� �����ϱ� ����
        private Random random;                          
        private int CurrentRoom;                        //���� ��� �濡 �ִ��� Ȯ���ϴ� �뵵

        public Dungeon(int maxFloor)
        {
            MaxFloor = maxFloor;
            CurrentFloor = 1;
            CurrentRoom = 0;
            Floors = new Dictionary<int, Room[]>();
            random = new Random();
            GenerateFloors();
        }

        private void GenerateFloors()       //���� �����ϰ� ����
        {
            for (int i = 1; i <= MaxFloor; i++)
            {
                Room[] rooms = new Room[8];
                for (int j = 0; j < 8; j++)
                {
                    bool hasEnemy = random.Next(100) < 65; // 65% Ȯ���� �� ����
                    bool hasItem = !hasEnemy && random.Next(100) < 25; // ���� ���� ��� 25% Ȯ���� ������ ����
                    rooms[j] = new Room(hasEnemy, hasItem);
                }
                Floors[i] = rooms;
            }
        }
 

        public void EnterDungeon(Player player, Inventory inventory)        //���� ���� �� ������ ǥ��
        {
            Console.WriteLine("������ �����մϴ�. ��� �����ϰڽ��ϱ�?");
            Console.WriteLine("1. ����(==Ž��) (�ڵ� Ŭ����)\n2. �� Ž��\n3. �� �̵�\n4. ������ ���ư���");
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
                    Console.WriteLine("������ ���ư��ϴ�");
                    break;
            }
        }

        public void ExploreFloor(Player player, Inventory inventory)        //���� Ž�� ���� �� �� �̵� ������ ���� �ڵ�
        {
            if (!Floors.ContainsKey(CurrentFloor)) return;

            Console.WriteLine($"���� {CurrentFloor}���� Ž���մϴ�...");
            CurrentRoom = 0; // �� Ž�� ���� �� ù ��° ����� ����

            while (CurrentRoom < Floors[CurrentFloor].Length)
            {
                Console.WriteLine($"���� ��: {CurrentRoom + 1} / {Floors[CurrentFloor].Length}");
                Floors[CurrentFloor][CurrentRoom].ResolveRoom(player, inventory, CurrentFloor);
                CurrentRoom++;
            }

            Console.WriteLine($"{CurrentFloor}�� Ž�� �Ϸ�!");
            MoveToNextFloor();
        }

        public void AutoClearFloor(Player player)       //���� ���ý� ����, ���ݷ��� ���Ͽ� ü��, ����� �����´�. �̴� ���� Ž���Ͽ��� ������ ������ �ſ� ����.
        {
            if (!Floors.ContainsKey(CurrentFloor)) return;
            int defenseFactor = player.MaxDef;
            int damageTaken = Math.Max(1, 10 - defenseFactor / 5);
            player.CurrentHp -= damageTaken;

            int rewardFactor = player.MaxAtk;
            int goldReward = rewardFactor * 2;
            player.Gold += goldReward;

            Console.WriteLine($"{CurrentFloor}�� ���� �Ϸ�! ü�� -{damageTaken}, ��� +{goldReward}");
        }
        public void MoveToNextFloor()       //���������� �̵��ϴ� �޼ҵ�
        {
            if (CurrentFloor < MaxFloor)
            {
                CurrentFloor++;
                Console.WriteLine($"���� ������ �̵��մϴ�! ���� ��: {CurrentFloor}��");
            }
            else
            {
                Console.WriteLine("���� �ְ����Դϴ�.");
            }
        }
        public void SelectFloor()       //�̵��� �� �����ϴ� �޼ҵ�
        {
            Console.WriteLine($"���� ��: {CurrentFloor}��");
            Console.WriteLine("�̵��� ���� �����ϼ��� (1~" + MaxFloor + "): ");
            int targetFloor = Utility.GetValidInput(1, MaxFloor);

            if (targetFloor == CurrentFloor)
            {
                Console.WriteLine("�̹� ���� ���� �ֽ��ϴ�.");
                return;
            }

            MoveToFloor(targetFloor);
        }

        public void MoveToFloor(int targetFloor)        //�Է� ���� ������ �̵��ϴ� �ڵ�. ���� ��ġ�� ������ ���̺꿡 �̹� ������;;;;     
        {
            if (targetFloor < 1 || targetFloor > MaxFloor)
            {
                Console.WriteLine("�̵��� �� ���� ���Դϴ�.");
                return;
            }

            Console.WriteLine($"{targetFloor}������ �̵��մϴ�...");
            CurrentFloor = targetFloor;
            CurrentRoom = 0; // ���ο� �������� ù ��° ����� ����
        }
    }
    public class Room
    {
        public bool HasEnemy { get; private set; }      //���� �ִ���
        public bool HasItem { get; private set; }       //�������� ��ӵǾ�����

        public Room(bool hasEnemy, bool hasItem)
        {
            HasEnemy = hasEnemy;
            HasItem = hasItem;
        }

        public void ResolveRoom(Player player, Inventory inventory, int currentFloor)       //���� �� ��Ʋ
        {
            if (HasEnemy)
            {
                Enemy enemy = Enemy.RandomGen(currentFloor);
                Console.WriteLine($"{enemy.Name}�� �����߽��ϴ�!");

                Battle battle = new Battle(player, enemy);
                battle.WholeBattle();
            }
            else if (HasItem)           //�ƴϸ� ������ ���� ���� �� ȹ��
            {
                Console.WriteLine("�������� �߰��߽��ϴ�!");
                Item item = ItemDatabase.Instance.RandomItemGen(currentFloor);
                inventory.AddItem(item);
                Console.WriteLine($"'{item.Name}'��(��) ȹ���߽��ϴ�!");
            }
            else
            {
                Console.WriteLine("�� ���̾����ϴ�.");
            }
            Console.WriteLine();
        }
    }
}