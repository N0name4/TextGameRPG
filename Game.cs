using System;

namespace TextGameRPG
{
    public class Game
    {

        public static void RestartGame()
        {
            Console.Clear(); // 화면 초기화
            Main(); // 메인 함수 다시 실행 → 처음 화면으로 돌아감
        }

        public static void Main() //게임 시작 코드
        {
            Player player = new Player();
            Inventory playerInventory = new Inventory(10);
            Dungeon dungeon = new Dungeon(8);
            SaveLoad saveLoad = new SaveLoad(player,playerInventory,dungeon);
            HomeScreen game = HomeScreen.Instance();
            game.Gamestart(saveLoad,player, playerInventory,dungeon);
        }
    }
}