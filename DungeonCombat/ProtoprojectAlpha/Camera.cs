using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;

namespace DungeonCombat
{
    class Camera
    {
        public int targetRow;
        public int targetColumn;
        public int radius;
        public Location target;
        bool panToTargetEnabled = false;

        public Camera(int radius = 4)
        {
            this.radius = radius;
            target = new Location(0, 0);
        }

        public void SetTarget(Location location)
        {
            target = location;
        }

        public void PanToTarget()
        {
            if (panToTargetEnabled)
            {
                targetRow = target.row;
                targetColumn = target.column;
            }
        }

        public void TogglePanToTargetEnabled()
        {
            if (panToTargetEnabled)
                panToTargetEnabled = false;
            else
                panToTargetEnabled = true;
        }

        public void SetPanToTargetEnabled(bool value)
        {
            panToTargetEnabled = value;
        }

        public int GetWidth()
        {
            return (radius * 2 + 1)*Tile.size;
        }

        public int GetHeight()
        {
            return GetWidth();
        }

        public void ExpandFieldOfView()
        {
            radius++;
        }

        public void ShrinkFieldOfView()
        {
            radius--;
        }

        public void PanLeft()
        {
            targetColumn--;
        }

        public void PanRight()
        {
            targetColumn++;
        }

        public void PanUp()
        {
            targetRow--;
        }

        public void PanDown()
        {
            targetRow++;
        }

        public Location WorldToCurrentViewLocation(Location worldLocation)
        {
            return new Location(
                worldLocation.row + (this.radius - this.targetRow),
                worldLocation.column + (this.radius - this.targetColumn));
        }

        public Location CurrentViewToDungeonLocation(Location currentViewLocation)
        {
            return new Location(
                currentViewLocation.row - (this.radius - this.targetRow),
                currentViewLocation.column - (this.radius - this.targetColumn));
        }

        public bool LocationInView(Location location)
        {
            int row = location.row;
            int column = location.column;

            if (row >= this.targetRow - this.radius
            && row <= this.targetRow + this.radius
            && column >= this.targetColumn - this.radius
            && column <= this.targetColumn + this.radius)
            {
                return true;
            }
            return false;
            
        }

        public Tile[,] GetViewOfDungeon(Dungeon dungeon)
        {
            Tile[,] view = new Tile[radius * 2 + 1, radius * 2 + 1];

            for (int row = targetRow - radius, newRow = 0; row <= targetRow + radius; ++row, ++newRow)
            {
                for (int column = targetColumn - radius, newColumn = 0; column <= targetColumn + radius; ++column, ++newColumn)
                {
                    if (row >= 0 && row < dungeon.dungeonFloor.GetLength(0) && column >= 0 && column < dungeon.dungeonFloor.GetLength(1))
                    {
                        view[newRow, newColumn] = new Tile(newRow, newColumn, dungeon.dungeonFloor[row, column].tileType, dungeon.dungeonFloor[row, column].visible);
                    }
                    else
                    {
                        //this tile is not part of the dungeon array
                        view[newRow, newColumn] = new Tile(newRow, newColumn, TileType.EmptyTile);
                    }
                }
            }

            return view;
        }
        
    }
}
