

namespace TextGameRPG
{
    public class Battle
    {
        public Player player;
        public Enemy enemy;
        private readonly Random rand = new();


        private int playerDodgeBonus = 0;
        private int playerDefenseBonus = 0;
        private int playerCriticalBonus = 0;
        private int enemyDodgeBonus = 0;
        private int enemyDefenseBonus = 0;
        private int enemyCriticalBonus = 0;
        public Battle(Player player, Enemy enemy)
        {
            this.player = player;
            this.enemy = enemy;
        }

        private int GetAtk(Character character){return character is Player player ? player.MaxAtk : character.Atk;}
        private int GetDef(Character character)
        {
            int baseDef = character is Player player ? player.MaxDef: character.Def;
            int defenseBonus = character is Player ? playerDefenseBonus : enemyDefenseBonus;
            return baseDef + defenseBonus;
        }
        private int GetInt(Character character) { return character is Player player ? player.MaxInt : character.Int; }
        private int GetDodge(Character character)
        {
            int baseDodge = character is Player player ? player.Maxdodge: character.Dodge;
            int dodgeBonus = character is Player ? playerDodgeBonus : enemyDodgeBonus;
            return baseDodge + dodgeBonus;
        }
        private int GetCritical(Character character) 
        {
            int baseCritical = character is Player player ? player.MaxCritical: character.Critical;
            int criticalBonus = character is Player ? playerCriticalBonus : enemyCriticalBonus;
            return baseCritical + criticalBonus;
        }
        private int GetActSpd(Character character) { return character is Player player ? player.MaxActSpd : character.ActSpd; }

        public void BaseAttack(Character attacker, Character defender)
        {
            int attackP = GetAtk(attacker);
            int CriticalP = GetCritical(attacker);
            int defenseP = GetDef(defender);
            int dodgeP = GetDodge(defender);

            if (rand.Next(1,101) <= dodgeP)
            {
                Console.WriteLine($"{defender.Name}�� ������ ȸ���߽��ϴ�!");
                return;
            }

            bool isCritical = rand.Next(1, 101) <= CriticalP;
            int damage = attackP - defenseP;

            if (isCritical){damage = (int)(damage * 2);}

            damage = Math.Max(damage, 1);

            defender.CurrentHp -= damage;

            Console.WriteLine($"{attacker.Name}�� {defender.Name}���� {damage}�� ���ظ� �������ϴ�! {(isCritical ? "(ġ��Ÿ �߻�!)" : "")}");
            Console.WriteLine($"{defender.Name}�� ���� ü��: {defender.CurrentHp}");
        }

        public void skillAttack(Character attacker, Character target, string skillName, int HpConsump, int MpConsump, int ApConsump, int AtkDamage, int IntDamage)
        {
            if (attacker.CurrentHp < HpConsump)
            {
                Console.WriteLine($"{attacker.Name}�� Hp�� �����Ͽ� {skillName} �ߵ��� �����Ͽ����ϴ�.");
                return;
            }
            if (attacker.CurrentMp < MpConsump)
            {
                Console.WriteLine($"{attacker.Name}�� Mp�� �����Ͽ� {skillName} �ߵ��� �����Ͽ����ϴ�.");
                return;
            }
            if (attacker.CurrentAp < ApConsump)
            {
                Console.WriteLine($"{attacker.Name}�� Ap�� �����Ͽ� {skillName} �ߵ��� �����Ͽ����ϴ�.");
                return;
            }

            int dodgeP = GetDodge(target);

            if(rand.Next(1, 151) <= dodgeP)
            {
                Console.WriteLine($"{target.Name}(��)�� {skillName}(��)�� ȸ���Ͽ����ϴ�.");
                return;
            }
            int AtkP = GetAtk(target);
            int IntP = GetInt(target);
            int DefP = GetDef(target);

            int damage = (int)(AtkP * AtkDamage + IntP * IntDamage);

            bool isCritical = rand.Next(1, 101) <= GetCritical(attacker);
            if (isCritical) { damage = damage * 2; }

            damage = Math.Max(damage, 1);
            target.CurrentHp -= damage;

            Console.WriteLine($"{attacker.Name}�� {skillName}��(��) ����Ͽ� {target.Name}���� {damage}�� ���ظ� �������ϴ�! {(isCritical ? "(ġ��Ÿ �߻�!)" : "")}");
            Console.WriteLine($"{target.Name}�� ���� ü��: {target.CurrentHp}");
        }

        public void DefenseCommand(Character character)
        {
            int defBonus = character is Player ? (int)(player.MaxDef / 2) : (int)(enemy.Def / 2);
            if(character is Player)
            {
                playerDefenseBonus = defBonus;
            }
            else
            {
                enemyDefenseBonus = defBonus;
            }

            Console.WriteLine($"{character.Name}�� ��� �¼��� ���մϴ�! ���� �ϱ��� ������ 1.5�谡 �˴ϴ�.");
        }

        public void DodgeCommand(Character character)
        {
            int dodgeBonus = character is Player ? player.Maxdodge : enemy.Def;
            if (character is Player)
            {
                playerDodgeBonus = dodgeBonus;
            }
            else
            {
                enemyDodgeBonus = dodgeBonus;
            }
            Console.WriteLine($"{character.Name}�� ȸ�� �¼��� ���մϴ�! ���� �ϱ��� ȸ������ 2�� �����մϴ�.");
        }

        public void CriticalCommand(Character character)
        {
            int criticalBonus = character is Player ? player.MaxCritical : enemy.Def;
            if (character is Player)
            {
                playerCriticalBonus = criticalBonus;
            }
            else
            {
                enemyCriticalBonus = criticalBonus;
            }
            Console.WriteLine($"{character.Name}�� �����մϴ�! ���� �ϱ��� ũ������ 2�� �����մϴ�.");
        }

        public void endTurn()
        {
            playerCriticalBonus = 0;
            playerDodgeBonus = 0;
            playerDefenseBonus = 0;
            enemyCriticalBonus = 0;
            enemyDodgeBonus = 0;
            enemyDefenseBonus = 0;
        }
    }
}