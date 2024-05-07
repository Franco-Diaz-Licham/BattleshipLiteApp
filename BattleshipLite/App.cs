using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;

namespace BattleshipLite
{
    public static class App
    {
        public static void Run()
        {
            ConsoleLogic.WelcomeMessage();

            PlayerInfoModel winner = null;

            PlayerInfoModel active = ConsoleLogic.AskForPlayerDetails("Player 1");
            ConsoleLogic.AskForShipPlacements(active);

            PlayerInfoModel opponent = ConsoleLogic.AskForPlayerDetails("Player 2");
            ConsoleLogic.AskForShipPlacements(opponent);

            do
            {
                ConsoleLogic.DisplayShotGrid(active);
                ConsoleLogic.TakePlayerShotTurn(active, opponent);

                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

                if (doesGameContinue)
                {
                    (active, opponent) = (opponent, active);
                }
                else
                {
                    winner = active;
                }

            } while (winner is null);

            ConsoleLogic.IdentifiyWinner(winner);
            ConsoleLogic.GoodByeMessage();
        }
    }
}
