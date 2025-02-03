

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
        }
    }

    public class Player : Character
    {
        public int Gold{get; set;}
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

        public Item? EquipedWeapon { get; set; }
        public Item? EquipedArmor { get; set; }

        public int MaxHp => Hp + (EquipedWeapon?.Hp ?? 0) + (EquipedArmor?.Hp ?? 0);
        public int MaxMp => Mp + (EquipedWeapon?.Mp ?? 0) + (EquipedArmor?.Mp ?? 0);
        public int MaxAp => Ap + (EquipedWeapon?.Ap ?? 0) + (EquipedArmor?.Ap ?? 0);

        public int MaxAtk => Atk + (EquipedWeapon?.Atk ??0) + (EquipedArmor?.Atk ?? 0);

        public int MaxDef => Def + (EquipedWeapon?.Def ?? 0) + (EquipedArmor?.Def ?? 0);

        public int MaxInt => Int + (EquipedWeapon?.Def ?? 0) + (EquipedArmor?.Int ?? 0);

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


        //플레이어 정보 초기 설정하기
        public Player(string name, int characterClass, int level, int hp, int mp, int ap, int intelligence, int atk, int def, int dodge, int critical, int actSpd, int expLimit, Item equipedWeapon, Item equipedArmor)
            : base(name, hp, mp, ap, intelligence, atk, def, dodge, critical, actSpd)
        {
            Level = level;
            Class = characterClass;
            ExpLimit = expLimit;
            CurrentExp = 0;
            EquipedWeapon = equipedWeapon;
            EquipedArmor = equipedArmor;

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

        public override void DisplayStats()
        {
            Console.WriteLine($"이름: {Name} (Lv. {Level})");
            Console.WriteLine($"직업: {Class}");

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
            DisplayStat("체력", CurrentHp, bonusHp);
            DisplayStat("마나", CurrentMp, bonusMp);
            DisplayStat("행동력", CurrentAp, bonusAp);
            Console.WriteLine();
            DisplayStat("공격력", Atk, bonusAtk);
            DisplayStat("방어력", Def, bonusDef);
            DisplayStat("마력", Int, bonusInt);
            Console.WriteLine();
            DisplayStat("치명타율", Critical, bonusCritical);
            DisplayStat("회피율", Dodge, bonusDodge);
            DisplayStat("행동 속도", ActSpd, bonusActSpd);

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
        }


    }

    public class Enemy : Character
    {
        public int ExpReward { get; set; }
        public int ItemReward { get; set; }

        public Enemy(string name, int hp, int mp, int ap, int intelligence, int atk, int def, int dodge, int critical, int actSpd, int expReward, int itemReward)
            : base(name, hp, mp, ap, intelligence, atk, def, dodge, critical, actSpd)
        {
            ExpReward = expReward;
            ItemReward = itemReward;
        }

    }

}