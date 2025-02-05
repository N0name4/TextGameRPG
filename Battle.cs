

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

        //플레이어면 아이템 적용된 스탯을, 적이면 기본 스탯을 가져온다.
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
                Console.WriteLine($"{defender.Name}가 공격을 회피했습니다!");
                return;
            }

            bool isCritical = rand.Next(1, 101) <= CriticalP;
            int damage = attackP - defenseP;

            if (isCritical){damage = (int)(damage * 2);}

            damage = Math.Max(damage, 1);

            defender.CurrentHp -= damage;

            Console.WriteLine($"{attacker.Name}가 {defender.Name}에게 {damage}의 피해를 입혔습니다! {(isCritical ? "(치명타 발생!)" : "")}");
            Console.WriteLine($"{defender.Name}의 남은 체력: {defender.CurrentHp}");
        }

        public void skillAttack(Character attacker, Character target, string skillName, int HpConsump, int MpConsump, int ApConsump, int AtkDamage, int IntDamage)
        {
            if (attacker.CurrentHp < HpConsump)
            {
                Console.WriteLine($"{attacker.Name}의 Hp가 부족하여 {skillName} 발동에 실패하였습니다.");
                return;
            }
            if (attacker.CurrentMp < MpConsump)
            {
                Console.WriteLine($"{attacker.Name}의 Mp가 부족하여 {skillName} 발동에 실패하였습니다.");
                return;
            }
            if (attacker.CurrentAp < ApConsump)
            {
                Console.WriteLine($"{attacker.Name}의 Ap가 부족하여 {skillName} 발동에 실패하였습니다.");
                return;
            }
            attacker.CurrentHp -= HpConsump;
            attacker.CurrentMp -= MpConsump;
            attacker.CurrentAp -= ApConsump;

            int dodgeP = GetDodge(target);

            if(rand.Next(1, 151) <= dodgeP)
            {
                Console.WriteLine($"{target.Name}(이)가 {skillName}(를)을 회피하였습니다.");
                return;
            }
            int AtkP = GetAtk(attacker);
            int IntP = GetInt(attacker);
            int DefP = GetDef(attacker);

            int damage = (int)(AtkP * AtkDamage + IntP * IntDamage);

            bool isCritical = rand.Next(1, 101) <= GetCritical(attacker);
            if (isCritical) { damage = damage * 2; }

            damage = Math.Max(damage, 1);
            target.CurrentHp -= damage;

            Console.WriteLine($"{attacker.Name}가 {skillName}을(를) 사용하여 {target.Name}에게 {damage}의 피해를 입혔습니다! {(isCritical ? "(치명타 발생!)" : "")}");
            Console.WriteLine($"{target.Name}의 남은 체력: {target.CurrentHp}");
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

            Console.WriteLine($"{character.Name}가 방어 태세를 취합니다! 다음 턴까지 방어력이 1.5배가 됩니다.");
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
            Console.WriteLine($"{character.Name}가 회피 태세를 취합니다! 다음 턴까지 회피율이 2배 증가합니다.");
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
            Console.WriteLine($"{character.Name}가 집중합니다! 다음 턴까지 크리율이 2배 증가합니다.");
        }
        //턴 끝날 때 가중치 초기화하는 함수
        public void endTurn()
        {
            playerCriticalBonus = 0;
            playerDodgeBonus = 0;
            playerDefenseBonus = 0;
            enemyCriticalBonus = 0;
            enemyDodgeBonus = 0;
            enemyDefenseBonus = 0;
        }

        //전체 배틀 함수
        public void WholeBattle()
        {
            Console.WriteLine($"전투 시작! {player.Name} vs {enemy.Name}");
            int playerSpeed = GetActSpd(player);
            int enemySpeed = GetActSpd(enemy);
            int totalTurn = playerSpeed + enemySpeed;

            while (player.CurrentHp > 0 && enemy.CurrentHp > 0)
            {
                bool result = rand.Next(0, totalTurn) <= playerSpeed;
                bool firstturn = true;
                if (result)
                {
                    Console.WriteLine($"선공은 {player.Name}");
                    do
                    {
                        if (player.CurrentHp > 0) { PlayerTurn(); }
                        if (enemy.CurrentHp <= 0) { break; }
                        if(!firstturn){Console.WriteLine($"{player.Name}의 추가 턴!");}
                        firstturn = false;
                        Console.WriteLine();
                    } while (rand.Next(0, totalTurn) <= playerSpeed);
                    do
                    {
                        if (enemy.CurrentHp > 0) { EnemyTurn(); }
                        if (player.CurrentHp <= 0) { break; }
                        if (!firstturn) { Console.WriteLine($"{player.Name}의 추가 턴!"); }
                        firstturn = false;
                        Console.WriteLine();
                    } while (rand.Next(0, totalTurn) <= enemySpeed);
                }
                else
                {
                    Console.WriteLine($"선공은 {enemy.Name}");
                    do
                    {
                        if (enemy.CurrentHp > 0) { EnemyTurn(); }
                        if (player.CurrentHp <= 0) { break; }
                    } while (rand.Next(0, totalTurn) <= enemySpeed);
                    do
                    {
                        if (player.CurrentHp > 0) { PlayerTurn(); }
                        if (enemy.CurrentHp <= 0) { break; }
                    } while (rand.Next(0, totalTurn) <= playerSpeed);
                }
                player.CurrentAp += (int)(player.MaxAp * 0.2);
                endTurn();
                Console.WriteLine();
            }
            EndBattle();


        }

       
        //플레이어 턴 행동 관련 함수.
        public void PlayerTurn()
        {
            while (true)
            {
                Console.WriteLine("1: 기본 공격  2: 방어  3: 회피  4: 집중  5: 스킬 공격  6: 적 정보 확인");
                string choice = Console.ReadLine();

                if (choice == "6")
                {
                    enemy.DisplayStats();
                    continue;
                }
                switch (choice)
                {
                    case "1":
                        BaseAttack(player, enemy);
                        break;
                    case "2":
                        DefenseCommand(player);
                        break;
                    case "3":
                        DodgeCommand(player);
                        break;
                    case "4":
                        CriticalCommand(player);
                        break;
                    case "5":
                        switch(player.Class)
                        {
                            case 1:
                                skillAttack(player, enemy, "《파괴의 일격》", 0, 0, 20, 2, 0);
                                break;
                            case 2:
                                skillAttack(player, enemy, "《파이어볼》", 0, 10, 0, 0, 3);
                                break;
                            case 3:
                                skillAttack(player, enemy, "《그림자 찌르기》", 0, 10, 15, 2, 1);
                                break;
                            case 4:
                                skillAttack(player, enemy, " 《신성》", 0, 15, 0, 1, 1);
                                int heal = (int)(player.MaxHp * player.MaxInt / 100);
                                player.CurrentHp += heal;
                                break;
                        }
                        break;
                }
                break;
            }
        }
        //적 행동 함수
        public void EnemyTurn()
        {
            Console.WriteLine($"{enemy.Name}의 턴!");
            if (rand.Next(1, 101) <= 65) // 65% 확률로 공격
            {
                BaseAttack(enemy, player);
            }
            else // 35% 확률로 방어
            {
                DefenseCommand(enemy);
            }
        }
        //적이던 나던 체력이 0이되면 사실상 실행된다.
        public void EndBattle()
        {
            if (player.CurrentHp <= 0)
            {
                Console.WriteLine("게임 오버! 플레이어가 패배하였습니다.");
                Console.WriteLine("처음 화면으로 돌아갑니다.");

                Console.ReadKey(); // 사용자가 입력할 때까지 대기

                Game.RestartGame(); // 프로그램 루프를 다시 실행
            }
            else if(enemy.CurrentHp <= 0)   
            {
                enemy.DefeatEnemy(player);
                if(rand.Next(1,101) <= enemy.ItemReward)
                {
                    Console.WriteLine("적에게서 아이템이 드롭됩니다!");
                }
            }
        }
    }
}