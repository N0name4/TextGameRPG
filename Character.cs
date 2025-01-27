using System.Security.Cryptography.X509Certificates;

public class Character
{
    public string Name { get; set; }
    public int Hp { get; set; }
    public int Mp { get; set; }
    public int Ap { get; set; }
    public int Int { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public int Hit { get; set; }
    public int Dodge { get; set; }
    public int Critical { get; set; }
    public int Exp { get; set; }
    public int ActSpd { get; set; }

    public int currentHp{ get => currentHp; set => currentHp = value > 0 ? value : 0;}

    // 캐릭터 생성자
    public Character(string name, int hp, int mp, int ap, int intel, int atk, int def, int hit, int dodge, int critical, int actSpd)
    {
        Name = name;
        Hp = hp;
        Mp = mp;
        Ap = ap;
        Int = intel;
        Atk = atk;
        Def = def;
        Hit = hit;
        Dodge = dodge;
        Critical = critical;
        ActSpd = actSpd;
    }

    // 기본 스탯 보여주기
    public void DisplayStats()
    {
        Console.WriteLine($"이름: {Name}");
        Console.WriteLine($"체력: {Hp}, 마나: {Mp}, 행동력: {Ap}");
        Console.WriteLine($"공격력: {Atk}, 방어력: {Def}, 마력: {Int}, 치명타율: {Critical}");
        Console.WriteLine($"명중률: {Hit}, 회피율: {Dodge}");
        Console.WriteLine($"행동 속도: {ActSpd}");
    }
}

public class Player : Character
{
    public int Level
    {
        get => Level;
        set => Level = value < 1 ? 1 : value;
    }
    //현재 직업
    public int Class{get;set;}      //1 : 전사, 2 : 마법사, 3 : 도적, 4 : 사제

    //경험치 통 한계
    public int ExpLimit{
        get => ExpLimit;
        set => ExpLimit = value > 0 ? value : 10;
    }

    //현재 장비 아이템
    public Item EquipedItem
    {
        get => EquipedItem;
        set => EquipedItem = value != null ? value : new Item(0,"",0,0,0,0,0,0,0,0,0,0,0, "");    
    }

    //현재까지 얻은 경험치
    public int CurrentExp
    {
        get => CurrentExp;
        set
        {
            CurrentExp = value;
            if (CurrentExp >= ExpLimit)
            {
                LevelUp();
            }
        }
    }


    //플레이어 정보 초기 설정하기
    public Player(string name, int characterClass, int level, int hp, int mp, int ap, int intelligence, int atk, int def, int hit, int dodge, int critical, int exp, int actSpd, int expLimit)
        : base(name, hp, mp, ap, intelligence, atk, def, hit, dodge, critical, actSpd)
    {
        Level = level;
        Class = characterClass;
        ExpLimit = expLimit;
        CurrentExp = 0;
    }

    //레벨업 시 불러올 코드
    private void LevelUp()
    {
        CurrentExp -= ExpLimit;
        ExpLimit += 3*Level; // 레벨업마다 경험치 요구량 증가
        Level++;
        Hp += 2;
        Atk += 1;
        Def += 2;
        Console.WriteLine($"{Name}가 레벨업 하였습니다. 다음 레벨 까지: {CurrentExp}");
    }

    public void DisplayStats()      //장비까지 포함하여 출력하기
    {

    }
    
   
}

public class Enemy : Character
{
    public int ExpReward { get; set; }
    public int ItemReawrd {get; set; }

    public Enemy(string name, int hp, int mp, int ap, int intelligence, int atk, int def, int hit, int dodge, int critical, int actSpd, int expReward, int itemReward)
        : base(name, hp, mp, ap, intelligence, atk, def, hit, dodge, critical, actSpd)
    {
        ExpReward = expReward;
        ItemReawrd = itemReward;
    }

}
