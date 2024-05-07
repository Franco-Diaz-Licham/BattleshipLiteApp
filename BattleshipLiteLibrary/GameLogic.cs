using BattleshipLiteLibrary.Models;
using System.Collections.Generic;

namespace BattleshipLiteLibrary
{
    public static class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel player)
        {
            List<string> rows = new List<string> 
            { 
                "A", 
                "B", 
                "C", 
                "D", 
                "E" 
            };

            List<int> cols = new List<int> 
            { 
                1, 
                2, 
                3, 
                4, 
                5 
            };

            foreach (var letter in rows)
            {
                foreach(var number in cols)
                {
                    AddGridSpot(player, letter, number);
                }
            }
        }

        public static void PlaceShip(PlayerInfoModel player, string row, int col)
        {
            GridSpotModel ship = new GridSpotModel()
            {
                SpotLetter = row,
                SpotNumber = col,
                Status = GridSpotStatus.Ship
            };

            player.ShipLocations.Add(ship);
        }

        public static bool RecordPlayerShot(PlayerInfoModel player, PlayerInfoModel opponent, string row, int col)
        {
            bool isShotValid = IdentifyShotResult(opponent, row, col);

            if (isShotValid)
            {
                UpdateShotGridStatus(player, GridSpotStatus.Hit, row, col);
                UpdateShipStatus(opponent, GridSpotStatus.Sunk, row, col);

                return true;
            }
            else
            {
                UpdateShotGridStatus(player, GridSpotStatus.Miss, row, col);

                return false;
            }
        }

        public static bool IsSpotOpen(PlayerInfoModel player, string row, int col)
        {
            foreach(var ship in player.ShipLocations)
            {
                if(ship.SpotLetter == row && ship.SpotNumber == col)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateGridLocation(string row, string col)
        {
            List<string> rows = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> cols = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };

            int.TryParse(col, out int numb);

            if (rows.Contains(row) && cols.Contains(numb))
            {
                return true;
            }

            return false;
        }

        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int col)
        {
            foreach (GridSpotModel shipSpot in opponent.ShipLocations)
            {
                if (shipSpot.SpotLetter == row && shipSpot.SpotNumber == col)
                {
                    return true;
                }
            }

            return false;
        }

        private static void AddGridSpot(PlayerInfoModel player, string row, int col)
        {
            GridSpotModel gridSpot = new GridSpotModel()
            {
                SpotLetter = row,
                SpotNumber = col,
                Status = GridSpotStatus.Empty
            };

            player.ShotGrid.Add(gridSpot);
        }

        private static void UpdateShipStatus(PlayerInfoModel player, GridSpotStatus gridStatus,string row, int col)
        {
            foreach (GridSpotModel shipSpot in player.ShipLocations)
            {
                if (shipSpot.SpotLetter == row && shipSpot.SpotNumber == col)
                {
                    shipSpot.Status = gridStatus;
                    return;
                }
            }
        }

        private static void UpdateShotGridStatus(PlayerInfoModel player, GridSpotStatus gridStatus,string row, int col)
        {
            foreach (GridSpotModel Spot in player.ShotGrid)
            {
                if (Spot.SpotLetter == row && Spot.SpotNumber == col)
                {
                    Spot.Status = gridStatus;
                    return;
                }
            }
        }

        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            bool isAlive = false;

            foreach (var ship in player.ShipLocations)
            {
                if(ship.Status != GridSpotStatus.Sunk)
                {
                    isAlive = true;
                }
            }
            return isAlive;
        }

        public static int GetShotCount(PlayerInfoModel winner)
        {
            int count = 0;

            foreach (var shot in winner.ShotGrid)
            {
                if(shot.Status == GridSpotStatus.Miss || shot.Status == GridSpotStatus.Hit)
                {
                    count += 1;
                }
            }

            return count;
        }
    }
}
