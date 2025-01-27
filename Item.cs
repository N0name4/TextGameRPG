
public class Item
{
    public int itemType{get; set;}
    public string Name{get; set;}
    public int Enhanced{get; set;}
    public int Hp{get; set;}
    public int Mp{get; set;}
    public int Ap{get; set;}
    public int Int{get; set;}
    public int Def{get; set;}
    public int Hit{get; set;}
    public int Dodge{get; set;}
    public int Critical{get; set;}
    public int ActSpd{get; set;}
    
    public int Price{get; set;}
    public string Description{get; set;}
    
    
    //기본 아이템 생성자
    public Item(int type, string name, int Enhanced, int Hp, int Mp, int Ap, int Int, int Def, int Hit, int Dodge, int Critical, int ActSpd, int Price, string Description)
    {
        this.itemType = type;
        this.Name = name;
        this.Enhanced = Enhanced;
        this.Hp = Hp;
        this.Ap = Ap;
        this.Int = Int;
        this.Def = Def;
        this.Hit = Hit;
        this.Dodge = Dodge;
        this.Critical = Critical;
        this.ActSpd = ActSpd;
        this.Price = Price;
        this.Description = Description;
    }

    public void displayItem()       //스탯 보여주기
    {

    }

    

    


}