using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace DungeonCombat
{
    class Dungeon
    {
        public Tile[,] dungeonFloor;
        public CombatManager battleManager;
        
        public Dungeon(CombatManager battleManager)
        {
            InitializeTiles(10, 10);
            this.battleManager = battleManager;
        }

        public int GetWidth()
        {
            return dungeonFloor.GetLength(1) * Tile.size;
        }

        public int GetHeight()
        {
            return dungeonFloor.GetLength(0) * Tile.size;
        }

        public Location GetFirstEmptySpace()
        {
            for (int row = 0; row < dungeonFloor.GetLength(0); ++row)
            {
                for (int column = 0; column < dungeonFloor.GetLength(1); ++column)
                {
                    if (dungeonFloor[row, column].tileType == 0 && !battleManager.CombatantIsHere(row, column))
                    {
                        return new Location(row, column);
                    }
                }
            }
            return null;
        }

        public Location GetLastEmptySpace()
        {
            for (int row = dungeonFloor.GetLength(0) - 1; row >= 0 ; --row)
            {
                for (int column = dungeonFloor.GetLength(1) - 1; column >= 0 ; --column)
                {
                    if (dungeonFloor[row, column].tileType == 0 && !battleManager.CombatantIsHere(row, column))
                    {
                        return new Location(row, column);
                    }
                }
            }
            return null;
        }

        public Location GetRandomEmptyLocation()
        {
            int row = 0;
            int column = 0;
            
            do
            {
                row = Die.random.Next(dungeonFloor.GetLength(0));
                column = Die.random.Next(dungeonFloor.GetLength(1));
            } while (dungeonFloor[row, column].tileType != 0 || battleManager.CombatantIsHere(row, column));

            return new Location(row, column);
        }

        public void AddPillar(int row, int column)
        {
            if (row < dungeonFloor.GetLength(0) && column < dungeonFloor.GetLength(1))
            {
                dungeonFloor[row, column] = new Tile(row, column, TileType.Pillar);
            }
        }

        public void AddEmptySpace(int row, int column)
        {
            if (row < dungeonFloor.GetLength(0) && column < dungeonFloor.GetLength(1))
            {
                dungeonFloor[row, column] = new Tile(row, column);
            }
        }

        public void InitializeTiles(int rows, int columns)
        {
            dungeonFloor = new Tile[rows, columns];

            for (int row = 0; row < dungeonFloor.GetLength(0); ++row)
            {
                for (int column = 0; column < dungeonFloor.GetLength(1); ++column)
                {
                    dungeonFloor[row, column] = new Tile(row, column);
                }
            }
        }

        public void DetermineCurrentFloorVisibility()
        {
            bool[,] visibility = VisionCalculator.GetVisionArray();
            for (int row = 0; row < dungeonFloor.GetLength(0); ++row)
            {
                for (int column = 0; column < dungeonFloor.GetLength(1); ++column)
                {
                    dungeonFloor[row, column].visible = visibility[row, column];
                }
            }
        }

        public void Draw(Camera camera)
        {
            DetermineCurrentFloorVisibility();
            Tile[,] view = camera.GetViewOfDungeon(this);

            for (int row = 0; row < view.GetLength(0); ++row)
            {
                for (int column = 0; column < view.GetLength(1); ++column)
                {
                    view[row, column].Draw();
                }
            }

        }

        public void LoadDungeonConfiguration1()
        {
            AddPillar(1, 1);
            AddPillar(1, 8);
            AddPillar(8, 8);
            AddPillar(8, 1);
            AddPillar(2, 4);
            AddPillar(2, 5);
            AddPillar(7, 4);
            AddPillar(7, 5);
            AddPillar(4, 0);
            AddPillar(4, 1);
            AddPillar(4, 2);
            AddPillar(4, 3);
            AddPillar(4, 6);
            AddPillar(4, 7);
            AddPillar(4, 8);
            AddPillar(4, 9);
            AddPillar(5, 0);
            AddPillar(5, 1);
            AddPillar(5, 2);
            AddPillar(5, 3);
            AddPillar(5, 6);
            AddPillar(5, 7);
            AddPillar(5, 8);
            AddPillar(5, 9);
        }

        public bool LocationInDungeon(Location location)
        {
            if (location.row >= 0 &&
                location.column >= 0 &&
                location.row < this.dungeonFloor.GetLength(0) &&
                location.column < this.dungeonFloor.GetLength(1))
                return true;
            return false;
        }

        public bool LocationInDungeon(int row, int column)
        {
            return LocationInDungeon(new Location(row, column));
        }
        
        public bool LocationIsOpenSpace(Location location)
        {
            if (this.dungeonFloor[location.row, location.column].tileType == TileType.OpenFloorSpace)
                return true;
            return false;
        }

        public bool LocationIsOpenSpace(int row, int column)
        {
            return LocationIsOpenSpace(new Location(row, column));
        }
    }
}
