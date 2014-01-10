using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DungeonCombat
{
    class Cursor
    {
        public Location location;
        public bool enabled = false;
        CombatManager battleManager = null;
        
        public Cursor(CombatManager battleManager)
        {
            location = new Location(-1, 0);
            this.battleManager = battleManager;
        }
        public void Toggle(Camera camera)
        {
            if (enabled)
            {
                TurnOff(camera, battleManager);
            }
            else
            {
                enabled = true;
                this.location = new Location(battleManager.GetCurrentCombatant().location.row, battleManager.GetCurrentCombatant().location.column);
                camera.target = location;
            }
        }
        public void TurnOff(Camera camera, CombatManager battleManager)
        {
            enabled = false;
            camera.target = battleManager.GetCurrentCombatant().location;
            this.location.row = -1;
        }
        public Combatant GetCombatantAtLocation()
        {
            if (battleManager.CombatantIsHere(location) && battleManager.GetCombatantAt(location).visible)
                return battleManager.GetCombatantAt(location);
            return null;
        }
        public void Draw(Camera camera)
        {
            if(camera.LocationInView(location))
            {
                TileRenderer.RenderTile("cursor", camera.WorldToCurrentViewLocation(location)); 
            }
        }
        public void RenderInformationAtLocation(Camera camera)
        {
            if(battleManager.CombatantIsHere(this.location))
            {
                Combatant combatant = battleManager.GetCombatantAt(this.location);

                if (combatant.visible)
                {
                    TextRenderer.DrawTextBesideLocation(combatant.name, combatant.location, 0);
                    combatant.RenderHealthBar(camera);
                }
            }
            else if(battleManager.DeathMarkerIsHere(location.row, location.column))
            {
                DeathMarker deathMarker = battleManager.GetDeathMarkerAt(this.location);
                if (deathMarker.visible)
                    TextRenderer.DrawTextBesideLocation(deathMarker.name + "'s corpse", deathMarker.location, 0);
            }
        }
        public void Move(string direction)
        {
            int row = location.row;
            int column = location.column;

            switch (direction)
            {
                case "Left":
                    if (column > 0 && battleManager.dungeon.dungeonFloor[row, column - 1].tileType== 0)
                    {
                        location.column--;
                    }
                    break;
                case "Right":
                    if (column < battleManager.dungeon.dungeonFloor.GetLength(1) - 1 && battleManager.dungeon.dungeonFloor[row, column + 1].tileType== 0)
                    {
                        location.column++;
                    }
                    break;
                case "Up":
                    if (row > 0 && battleManager.dungeon.dungeonFloor[row - 1, column].tileType== 0)
                    {
                        location.row--;
                    }
                    break;
                case "Down":
                    if (row < battleManager.dungeon.dungeonFloor.GetLength(0) - 1 && battleManager.dungeon.dungeonFloor[row + 1, column].tileType== 0)
                    {

                        location.row++;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid movement command...FIX IT NOW!!!!!! THE HORROR!!!!!");
                    break;
            }
        }
        public void MoveLeft()
        {
            Move("Left");
        }
        public void MoveRight()
        {
            Move("Right");
        }
        public void MoveUp()
        {
            Move("Up");
        }
        public void MoveDown()
        {
            Move("Down");
        }
    }
}
