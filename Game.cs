using System;

namespace TextGameRPG
{
    public class Game
    {
        public static void Main()
        {
            HomeScreen game = HomeScreen.Instance();
            game.Gamestart();

        }
    }
}