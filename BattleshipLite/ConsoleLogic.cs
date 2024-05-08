using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;
using System;
using System.Linq;

namespace BattleshipLite
{
    public static class ConsoleLogic
    {
        public static void WelcomeMessage()
        {
            Console.WriteLine("~~~ Welcome to battleship lite ~~~");
            Console.WriteLine("created by Franco Diaz");
            Console.WriteLine("-----------------------------------");
        }

        public static void GoodByeMessage()
        {
            PrintMessage("~~~ Press any key to exit app ~~~");
            Console.ReadLine();
        }

        public static (string row, string col) ParseResponse(
                string response)
        {
            string row = "";
            string col = "";

            var gridSpot = response.ToList();

            if(gridSpot.Count > 1)
            {
                row = gridSpot[0].ToString().ToUpper();
                col = gridSpot[1].ToString().ToUpper();
            }

            return (row, col);
        }

        public static PlayerInfoModel AskForPlayerDetails(
                string playerTitle)
        {
            PrintMessage($"Player information for: {playerTitle}");
            string name = AskForPlayerInput("What is your name: ");

            var output = new PlayerInfoModel() { UserName = name };
            GameLogic.InitializeGrid(output);

            return output;
        }

        public static void AskForShipPlacements(
                PlayerInfoModel player)
        {
            string row;
            string col;
            bool isLocationValid;
            bool isSpotOpen;

            do
            {
                do
                {
                    string location = AskForPlayerInput($"Location for ship {player.ShipLocations.Count + 1}: ");

                    var data = ParseResponse(location);
                    row = data.row;
                    col = data.col;

                    isLocationValid = GameLogic.ValidateGridLocation(row, col);

                    if (isLocationValid == false)
                        PrintMessage("Location Invalid...");

                } while (isLocationValid == false);

                isSpotOpen = GameLogic.IsSpotOpen(player, row, int.Parse(col));

                if(isSpotOpen == true)
                    GameLogic.PlaceShip(player, row, int.Parse(col));
                else
                    PrintMessage("Ship already exists in that location...");

            } while(player.ShipLocations.Count < 5);

            ClearScreen();
        }

        public static void TakePlayerShotTurn(
                PlayerInfoModel player, 
                PlayerInfoModel opponent)
        {
            string row;
            string col;
            bool isLocationValid;
            PrintMessage($"Turn for: {player.UserName}");

            do
            {
                string location = AskForPlayerInput($"Shot location: ");

                var data = ParseResponse(location);
                row = data.row;
                col = data.col;

                isLocationValid = GameLogic.ValidateGridLocation(row, col);
                
                if (isLocationValid == false)
                    PrintMessage("Location Invalid...");

            } while (isLocationValid == false);

            bool isShotValid = GameLogic.RecordPlayerShot(player, opponent, row, int.Parse(col));

            if (isShotValid)
                PrintMessage("Result: SHOT! :)");
            else
                PrintMessage("Result: MISS  :(");

            AskForPlayerInput("Press any key to continue game...");
        }

        public static void ClearScreen()
        {
            Console.Clear();
        }

        public static void PrintMessage(
                string message)
        {
            Console.WriteLine();
            Console.Write($"{message}");
        }

        public static string AskForPlayerInput(
                string message)
        {
            PrintMessage($"{message}");
            string data = Console.ReadLine();

            return data;
        }

        public static void DisplayShotGrid(
                PlayerInfoModel player)
        {
            ClearScreen();

            string currentRow = player.ShotGrid[0].SpotLetter;

            foreach (var gridSpot in player.ShotGrid)
            {
                if(gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                if(gridSpot.Status == GridSpotStatus.Empty)
                    Console.Write($"  {gridSpot.SpotLetter}{gridSpot.SpotNumber}  ");
                else if(gridSpot.Status == GridSpotStatus.Hit)
                    Console.Write("  X   ");
                else if (gridSpot.Status == GridSpotStatus.Miss)
                    Console.Write("  O   ");
                else
                    Console.Write("  ?   ");
            }

            Console.WriteLine();
        }

        public static void IdentifiyWinner(
                PlayerInfoModel winner)
        {
            PrintMessage($"Congratulations to { winner.UserName } for winning!");
            PrintMessage($"{winner.UserName} took {GameLogic.GetShotCount(winner)} shots to win!");
        }
    }
}