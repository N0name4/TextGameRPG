

using System.Reflection.Emit;

namespace TextGameRPG
{
    public class Character
    {
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int Ap { get; set; }
        public int Int { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Dodge { get; set; }
        public int Critical { get; set; }
        public int ActSpd { get; set; }

        protected int currentHp;
        public virtual int CurrentHp 
        { 
            get => currentHp;
            set => currentHp = value > Hp ? Hp : (value > 0 ? value : 0);
        }

        protected int currentMp;
        public virtual int CurrentMp
        {
            get => currentMp;
            set => currentMp = value > Mp ? Mp : (value > 0 ? value : 0);
        }

        protected int currentAp;
        public virtual int CurrentAp
        {
            get => currentAp;
            set => currentAp = value > Ap ? Ap : (value > 0 ? value : 0);
        }

        // 캐릭터 생성자
        public Character(string name, int hp, int mp, int ap, int intel, int atk, int def, int dodge, int critical, int actSpd)
        {
            Name = name;
            Hp = hp;
            CurrentHp = hp;
            Mp = mp;
            CurrentMp = mp;
            Ap = ap;
            CurrentAp = ap;
            Int = intel;
            Atk = atk;
            Def = def;
            Dodge = dodge;
            Critical = critical;
            ActSpd = actSpd;
            
        }

        // 기본 스탯 보여주기
        public virtual void DisplayStats()
        {
            Console.WriteLine($"이름: {Name}");
            Console.WriteLine($"체력: {currentHp}/{Hp}, 마나: {currentMp}/{Mp}, 행동력: {currentAp}/{Ap}");
            Console.WriteLine($"공격력: {Atk}, 방어력: {Def}, 마력: {Int}, 치명타율: {Critical}");
            Console.WriteLine($" 회피율: {Dodge}, 행동 속도: {ActSpd}");
            Console.WriteLine();
        }
    }

    public class Player : Character
    {
        private int gold;

        public int Gold
        {
            get => gold;
            set
            {
                if (value < 0) { gold = 0; }
                else { gold = value; }
            }
        }
        private int level;
        public int Level
        {
            get => level;
            set => level = value < 1 ? 1 : value;
        }
        //현재 직업
        public int Class { get; set; }      //1 : 전사, 2 : 마법사, 3 : 도적, 4 : 사제

        //경험치 통 한계

        private int expLimit;
        public int ExpLimit
        {
            get => expLimit;
            set => expLimit = value > 0 ? value : 10;
        }

        private int currentExp;
        //현재까지 얻은 경험치
        public int CurrentExp
        {
            get => currentExp;
            set
            {
                currentExp = value;
                if (currentExp >= ExpLimit)
                {
                    LevelUp();
                }
            }
        }

        public Item? EquipedWeapon { get; set; }        //현재 장비한 무기
        public Item? EquipedArmor { get; set; }         //현재 장비한 방어구

        public int MaxHp => Hp + (EquipedWeapon?.Hp ?? 0) + (EquipedArmor?.Hp ?? 0);
        public int MaxMp => Mp + (EquipedWeapon?.Mp ?? 0) + (EquipedArmor?.Mp ?? 0);
        public int MaxAp => Ap + (EquipedWeapon?.Ap ?? 0) + (EquipedArmor?.Ap ?? 0);

        public int MaxAtk => Atk + (EquipedWeapon?.Atk ?? 0) + (EquipedArmor?.Atk ?? 0);

        public int MaxDef => Def + (EquipedWeapon?.Def ?? 0) + (EquipedArmor?.Def ?? 0);

        public int MaxInt => Int + (EquipedWeapon?.Int ?? 0) + (EquipedArmor?.Int ?? 0);

        public int Maxdodge => Dodge + (EquipedWeapon?.Dodge ?? 0) + (EquipedArmor?.Dodge ?? 0);

        public int MaxCritical => Critical + (EquipedWeapon?.Critical ?? 0) + (EquipedArmor?.Critical ?? 0);

        public int MaxActSpd => ActSpd + (EquipedWeapon?.ActSpd ?? 0) + (EquipedArmor?.ActSpd ?? 0);

        public override int CurrentHp
        {
            get => currentHp;
            set => currentHp = value > MaxHp ? MaxHp : (value > 0 ? value : 0);
        }

        public override int CurrentMp
        {
            get => currentMp;
            set => currentMp = value > MaxMp ? MaxMp : (value > 0 ? value : 0);
        }

        public override int CurrentAp
        {
            get => currentAp;
            set => currentAp = value > MaxAp ? MaxAp : (value > 0 ? value : 0);
        }

        public Player() : base("Unknown", 100, 50, 30, 10, 15, 5, 5, 10, 10)
        {
            Level = 1;
            Class = 1;
            ExpLimit = 100;
            CurrentExp = 0;
            EquipedWeapon = null;
            EquipedArmor = null;
            gold = 1000;
        }

        //플레이어 정보 초기 설정하기
        public Player(string name, int characterClass, int level, int hp, int mp, int ap, int intelligence, int atk, int def, int dodge, int critical, int actSpd, int expLimit, Item equipedWeapon, Item equipedArmor, int gold)
            : base(name, hp, mp, ap, intelligence, atk, def, dodge, critical, actSpd)
        {
            Level = level;
            Class = characterClass;
            ExpLimit = expLimit;
            CurrentExp = 0;
            EquipedWeapon = equipedWeapon;
            EquipedArmor = equipedArmor;
            Gold = gold;

        }

        //레벨업 시 불러올 코드
        private void LevelUp()
        {
            while (CurrentExp >= ExpLimit)
            {
                CurrentExp -= ExpLimit;
                ExpLimit += 3 * Level; // 레벨업마다 경험치 요구량 증가
                Level++;
                Hp += 2;
                Atk += 1;
                Def += 2;
                Int += 1;
                Console.WriteLine($"{Name}가 레벨업 하였습니다. 다음 레벨까지: {ExpLimit - CurrentExp}");
            }
        }
        //장비를 입을 때 사용되는 함수
        public void EquipItem(Item item)
        {
            if (item.ItemType == 0) // 무기 장착
            {
                if (EquipedWeapon != null)
                {
                    Console.WriteLine($"{EquipedWeapon.Name}을(를) 해제하고 {item.Name}을(를) 장착했습니다.");
                }
                else
                {
                    Console.WriteLine($"{item.Name}을(를) 장착했습니다.");
                }
                EquipedWeapon = item;
            }
            else if (item.ItemType == 1) // 방어구 장착
            {
                if (EquipedArmor != null)
                {
                    Console.WriteLine($"{EquipedArmor.Name}을(를) 해제하고 {item.Name}을(를) 장착했습니다.");
                }
                else
                {
                    Console.WriteLine($"{item.Name}을(를) 장착했습니다.");
                }
                EquipedArmor = item;
            }
        }
        //장비를 뺄 때 호출되는 함수.
        public void UnequipItem(Item item)
        {
            if (EquipedWeapon == item)
            {
                Console.WriteLine($"{item.Name}을(를) 장착 해제했습니다.");
                EquipedWeapon = null;
            }
            else if (EquipedArmor == item)
            {
                Console.WriteLine($"{item.Name}을(를) 장착 해제했습니다.");
                EquipedArmor = null;
            }
        }

        //플레이어 스탯을 확인할 수 있는 코드. 
        public override void DisplayStats()
        {
            Console.WriteLine($"이름: {Name} (Lv. {Level})");
            Console.WriteLine($"직업: {(Class == 1 ? "전사" : Class == 2 ? "마법사" : Class == 3 ? "도적" : "사제")}");

            void DisplayAll(string label, int baseValue, int bonus, int max)
            {
                Console.Write($"{label}: {baseValue}");
                if (bonus > 0) Console.Write($" (+{bonus})");
                Console.Write($"/{max}");
                Console.Write(", ");
            }

            void DisplayStat(string label, int baseValue, int bonus)
            {
                Console.Write($"{label}: {baseValue}");
                if (bonus > 0) Console.Write($" (+{bonus})");
                Console.Write(", ");
            }

            // 아이템 보너스 계산
            int bonusHp = (EquipedWeapon?.Hp ?? 0) + (EquipedArmor?.Hp ?? 0);
            int bonusMp = (EquipedWeapon?.Mp ?? 0) + (EquipedArmor?.Mp ?? 0);
            int bonusAp = (EquipedWeapon?.Ap ?? 0) + (EquipedArmor?.Ap ?? 0);
            int bonusAtk = (EquipedWeapon?.Atk ?? 0) + (EquipedArmor?.Atk ?? 0);
            int bonusDef = (EquipedWeapon?.Def ?? 0) + (EquipedArmor?.Def ?? 0);
            int bonusInt = (EquipedWeapon?.Int ?? 0) + (EquipedArmor?.Int ?? 0);
            int bonusCritical = (EquipedWeapon?.Critical ?? 0) + (EquipedArmor?.Critical ?? 0);
            int bonusDodge = (EquipedWeapon?.Dodge ?? 0) + (EquipedArmor?.Dodge ?? 0);
            int bonusActSpd = (EquipedWeapon?.ActSpd ?? 0) + (EquipedArmor?.ActSpd ?? 0);

            // 현재 스탯 출력
            DisplayAll("체력", CurrentHp, bonusHp, MaxHp);
            DisplayAll("마나", CurrentMp, bonusMp, MaxMp);
            DisplayAll("행동력", CurrentAp, bonusAp, MaxAp);
            Console.WriteLine();
            DisplayStat("공격력", Atk, bonusAtk);
            DisplayStat("방어력", Def, bonusDef);
            DisplayStat("마력", Int, bonusInt);
            Console.WriteLine();
            DisplayStat("치명타율", Critical, bonusCritical);
            DisplayStat("회피율", Dodge, bonusDodge);
            DisplayStat("행동 속도", ActSpd, bonusActSpd);
            Console.WriteLine();
            Console.Write($"경험치: {CurrentExp}/{ExpLimit}, ");
            Console.Write($"골드: {Gold}");
            Console.WriteLine();

            Console.WriteLine("\n[장착 아이템]");
            if (EquipedWeapon != null)
            {
                Console.WriteLine("- 무기:");
                EquipedWeapon.DisplayItem();
            }
            if (EquipedArmor != null)
            {
                Console.WriteLine("- 방어구:");
                EquipedArmor.DisplayItem();
            }
            Console.WriteLine();
        }

        //휴식 코드. 골드를 확인하며 최대 체력의 반만큼 회복
        public void Rest()
        {
            const int cost = 500;

            if (Gold <= cost)
            {
                Console.WriteLine("골드가 부족하여 휴식을 취할 수 없습니다.");
                return;
            }

            Gold -= cost;
            CurrentHp = CurrentHp + (MaxHp / 2);
            CurrentMp = CurrentMp + (MaxMp / 2);

            Console.WriteLine("휴식을 취하여 체력, 마나가 50% 회복되었습니다.");
            Console.WriteLine($"남은 골드: {Gold} G");
        }

        //전직 시 초기 스탯을 전직과 어울리게 반영
        public void ApplyClassStats(int charclass)
        {
            switch (charclass)
            {
                case 1: //전사
                    Hp += 50;
                    Def += 10;
                    break;
                case 2: //마법사
                    Mp += 50;
                    Int += 10;
                    break;
                case 3: //도적
                    Atk += 10;
                    Critical += 5;
                    ActSpd += 3;
                    break;
                case 4: //사제
                    Mp += 30;
                    Int += 8;
                    Def += 7;
                    break;
            }
        }
    }

    public class Enemy : Character
    {
        public int ExpReward { get; set; }
        public int KillGold { get; set; }
        public int ItemReward { get; set; }
        //적은 아이템 경험치 드롭, 처치 수당, 아이템 드롭 확률로 나눈다.
        public Enemy(string name, int hp, int mp, int ap, int intelligence, int atk, int def, int dodge, int critical, int actSpd, int expReward, int itemReward, int killGold)
            : base(name, hp, mp, ap, intelligence, atk, def, dodge, critical, actSpd)
        {
            ExpReward = expReward;
            KillGold = killGold;
            ItemReward = itemReward;
        }
        //처치할 경우 어떻게 작동하는지
        public void DefeatEnemy(Player player)
        {
            Console.WriteLine($"{Name}을(를) 처치하였습니다!");
            player.CurrentExp += ExpReward;
            player.Gold += KillGold;
            Console.WriteLine($"경험치 +{ExpReward}, 골드 +{KillGold}");
        }
        //적 정보를 담은 Dict. 생성만 하면 되어서 따로 클래스를 만들지는 않았다. Dict의 int는 floor에 어떤 적이 등장하는지 결정한다..
        private static Dictionary<int, List<Enemy>> enemyByFloor = new Dictionary<int, List<Enemy>>
        {
            { 1, new List<Enemy>{
                new Enemy("약한 슬라임", 50, 0, 10, 1, 5, 2, 5, 1, 10, 10, 10, 50),
                new Enemy("약한 고블린", 70, 0, 12, 2, 6, 3, 6, 2, 12, 12, 10, 70),
                new Enemy("새끼 늑대", 60, 0, 14, 1, 7, 3, 8, 4, 12, 15, 10, 80)
            }},
            { 2, new List<Enemy> {
                new Enemy("슬라임", 80, 0, 15, 2, 8, 4, 7, 2, 15, 15, 15,100),
                new Enemy("고블린 전사", 100, 0, 18, 3, 10, 5, 8, 3, 18, 18, 15, 120),
                new Enemy("성난 멧돼지", 110, 0, 16, 2, 12, 6, 9, 3, 17, 20, 15, 130)
            } },
            { 3, new List<Enemy> {
                new Enemy("오크", 140, 0, 25, 4, 14, 8, 12, 4, 25, 25, 20,200),
                new Enemy("스켈레톤 병사", 120, 0, 20, 3, 12, 6, 10, 3, 20, 20, 20, 180),
                new Enemy("흉폭한 늑대", 130, 0, 22, 3, 16, 7, 14, 5, 22, 22, 20,190)
            } },
            { 4, new List<Enemy> {
                new Enemy("강한 오크", 180, 0, 30, 5, 18, 10, 14, 5, 30, 30, 30, 250),
                new Enemy("강한 스켈레톤", 150, 0, 25, 4, 14, 8, 11, 4, 22, 22, 30, 220),
                new Enemy("트롤 전사", 200, 0, 28, 5, 20, 10, 12, 5, 28, 28, 30, 260)
            } },
            { 5, new List<Enemy> {
                new Enemy("오우거", 250, 0, 40, 6, 22, 12, 16, 6, 35, 35, 50, 300),
                new Enemy("드래곤", 400, 0, 70, 8, 35, 18, 25, 7, 60, 60, 50, 700),
                new Enemy("암흑 마법사", 220, 100, 50, 10, 28, 12, 10, 8, 40, 45, 50, 350)
            } },
             { 6, new List<Enemy> {
                new Enemy("암석 골렘", 500, 0, 50, 5, 40, 30, 10, 3, 25, 75, 55, 800),
                new Enemy("화염 정령", 350, 150, 60, 12, 50, 20, 15, 6, 50, 80, 55, 900),
                new Enemy("거대 거미", 300, 0, 40, 6, 35, 25, 30, 10, 45, 65, 55, 750)
            } },
              { 7, new List<Enemy> {
                new Enemy("암흑 기사", 600, 50, 80, 8, 55, 35, 20, 8, 55, 100, 70, 1200),
                new Enemy("서리 마법사", 400, 200, 70, 15, 45, 30, 10, 12, 60, 90, 70, 1100),
                new Enemy("뱀파이어", 550, 100, 85, 10, 60, 28, 25, 10, 65, 110, 70, 1300)
            } },
               { 8, new List<Enemy> {
                new Enemy("마왕", 1200, 300, 120, 20, 80, 50, 30, 15, 80, 200, 100, 5000),
                new Enemy("고대 용", 1500, 0, 130, 18, 90, 55, 35, 12, 90, 250, 100, 6000),
                new Enemy("악몽의 군주", 1300, 250, 125, 22, 85, 45, 28, 14, 85, 220, 100, 5500)
            } }
        };
        //적을 랜덤하게 생성. 
        public static Enemy RandomGen(int floor)
        {
            Random random = new Random();
            int maxFloor = Math.Min(floor, enemyByFloor.Keys.Max());
            return enemyByFloor[maxFloor][random.Next(enemyByFloor[maxFloor].Count)];
        }
        
    }

}