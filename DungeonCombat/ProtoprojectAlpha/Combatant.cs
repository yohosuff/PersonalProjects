using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using QuickFont;
using OpenTK.Input;

namespace DungeonCombat
{
    class Combatant
    {
        public Location location;
        public int movementPerRound;
        public int movementLeft;
        public string name;
        public string textureName;
        public bool canMove = true;
        public bool visible = false;
        public CombatManager battleManager = null;
        public int currentHitPoints;
        public int maxHitPoints;
        public string equippedWeaponName = null;
        public Weapon equippedWeapon = null;

        public List<Weapon> weapons = null;

        public Combatant(string name, string textureName, CombatManager battleManager)
        {
            this.name = name;
            this.textureName = textureName;
            this.battleManager = battleManager;
            this.weapons = new List<Weapon>();
            AddWeapon(WeaponManager.GetWeaponByName("Fists"));
            EquipWeapon("Fists");
        }

        public void EquipWeapon(string weaponName)
        {
            if (weaponName == null)
                return;
            
            foreach (Weapon weapon in weapons)
            {
                if (weaponName == weapon.name)
                {
                    weapon.combatant = this;
                    equippedWeapon = weapon;
                    return;
                }
            }


        }

        public bool HasMovementLeft()
        {
            return movementLeft > 0;
        }

        public Keys[] GeneratePossibleAttackInputKeys()
        {
            Keys[] keys = new Keys[weapons.Count()];
            foreach (Weapon weapon in weapons)
            {
                switch (weapons.IndexOf(weapon))
                {
                    case 0: keys[0] = Keys.D1; break;
                    case 1: keys[1] = Keys.D2; break;
                    case 2: keys[2] = Keys.D3; break;
                    case 3: keys[3] = Keys.D4; break;
                    case 4: keys[4] = Keys.D5; break;
                    case 5: keys[5] = Keys.D6; break;
                    case 6: keys[6] = Keys.D7; break;
                    case 7: keys[7] = Keys.D8; break;
                    case 8: keys[8] = Keys.D9; break;
                    case 9: keys[9] = Keys.D0; break;
                    default: break;
                }
            }
            return keys;
        }

        public void ProcessAttackInput(Keys key, Combatant combatant)
        {
            if (this != combatant && GeneratePossibleAttackInputKeys().Contains(key))
            {
                switch (key)
                {
                    case Keys.D1: weapons[0].Attack(combatant); break;
                    case Keys.D2: weapons[1].Attack(combatant); break;
                    case Keys.D3: weapons[2].Attack(combatant); break;
                    case Keys.D4: weapons[3].Attack(combatant); break;
                    case Keys.D5: weapons[4].Attack(combatant); break;
                    case Keys.D6: weapons[5].Attack(combatant); break;
                    case Keys.D7: weapons[6].Attack(combatant); break;
                    case Keys.D8: weapons[7].Attack(combatant); break;
                    case Keys.D9: weapons[8].Attack(combatant); break;
                    case Keys.D0: weapons[9].Attack(combatant); break;
                }
            }
        }

        public void AddWeapon(string name, int damage, string soundName, int range)
        {
            weapons.Add(new Weapon(this, name, damage, soundName, range));
        }

        public void AddWeapon(Weapon weapon)
        {
            AddWeapon(weapon.name, weapon.damage, weapon.soundName, weapon.range);
        }

        public void RenderHealthBar(Camera camera)
        {
            Location relativeLocation = camera.WorldToCurrentViewLocation(location);

            GL.Disable(EnableCap.Texture2D);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(Color.LightGreen);
            GL.Vertex2(relativeLocation.column * 64, relativeLocation.row * 64);
            GL.Vertex2(relativeLocation.column * 64 + 64 * GetHealthPercentage(), relativeLocation.row * 64);
            GL.Vertex2(relativeLocation.column * 64 + 64 * GetHealthPercentage(), relativeLocation.row * 64 + 2);
            GL.Vertex2(relativeLocation.column * 64, relativeLocation.row * 64 + 2);

            GL.End();

        }

        public float GetHealthPercentage()
        {
            return (float)currentHitPoints / maxHitPoints;
        }

        public string GetHealthConditionString()
        {
            float healthPercentage = GetHealthPercentage();

            if (healthPercentage < 25)
                return "Almost dead";
            if (healthPercentage < 50)
                return "Severely wounded";
            if (healthPercentage < 75)
                return "Wounded";
            return "Healthy";
        }

        public void SetLocation(Location location)
        {
            if (VisionCalculator.IndexInBounds(location.row, location.column, battleManager.dungeon.dungeonFloor))
                this.location = location;
        }

        public void SetHitPoints(int value)
        {
            currentHitPoints = maxHitPoints = value;
        }

        public void SetMovementPerRound(int movement)
        {
            movementPerRound = movement;
        }

        public void ResetMovementLeft()
        {
            movementLeft = movementPerRound;
        }

        public List<Location> GetOpenLocationsAroundMe()
        {
            List<Location> locations = new List<Location>();
            for (int row = location.row - 1; row <= location.row + 1; ++row)
            {
                for (int column = location.column - 1; column <= location.column + 1; ++column)
                {
                    if (row == location.row && column == location.column)
                        continue;

                    Location locationToAdd = new Location(row, column);
                    if (battleManager.dungeon.LocationInDungeon(locationToAdd) &&
                        battleManager.dungeon.LocationIsOpenSpace(locationToAdd) &&
                        battleManager.CombatantNotHere(locationToAdd))
                    {
                        locations.Add(locationToAdd);
                    }
                }
            }

            return locations;
        }

        public List<Combatant> GetCombatantsInRangeOfMe(int range)
        {
            List<Combatant> combatants = new List<Combatant>();
            for (int row = location.row - range; row <= location.row + range; ++row)
            {
                for (int column = location.column - range; column <= location.column + range; ++column)
                {
                    if (row == location.row && column == location.column)
                        continue;

                    if (battleManager.dungeon.LocationInDungeon(row, column) &&
                        battleManager.dungeon.LocationIsOpenSpace(row, column) &&
                        battleManager.CombatantIsHere(row, column))
                    {
                        combatants.Add(battleManager.GetCombatantAt(row, column));
                    }
                }
            }
            return combatants;
        }

        public List<Combatant> GetVisibleCombatantsInRangeOfMe(int range)
        {
            List<Combatant> combatants = new List<Combatant>();
            for (int row = location.row - range; row <= location.row + range; ++row)
            {
                for (int column = location.column - range; column <= location.column + range; ++column)
                {
                    if (row == location.row && column == location.column)
                        continue;

                    if (battleManager.dungeon.LocationInDungeon(row, column) &&
                        battleManager.dungeon.LocationIsOpenSpace(row, column) &&
                        battleManager.CombatantIsHere(row, column) &&
                        CanSeeCombatant(battleManager.GetCombatantAt(row, column)))
                    {
                        combatants.Add(battleManager.GetCombatantAt(row, column));
                    }
                }
            }
            return combatants;
            
            //List<Combatant> combatants = GetCombatantsInRangeOfMe(range);
            //foreach (Combatant combatant in combatants)
            //    if (CanSeeCombatant(combatant))
            //        combatants.Remove(combatant);
            //return combatants;
        }

        public bool CanSeeCombatant(Combatant combatant)
        {
            bool[,] visionArray = VisionCalculator.GetVisionArray();
            return visionArray[combatant.location.row, combatant.location.column];
        }

        public void MeleeAttack(Combatant combatant)
        {
            if (this != combatant && InRange(combatant))
            {
                combatant.TakeDamage(Die.random.Next(1, 7));
                AudioManager.Play("slash");
            }
        }

        public void RangedAttack(Combatant combatant)
        {
            if (this != combatant && battleManager.DistanceBetweenLocations(this.location, combatant.location) <= 10)
            {
                combatant.TakeDamage(Die.random.Next(1, 7));
                AudioManager.Play("bow");
            }
        }

        public bool InRange(Combatant combatant)
        {
            int top = location.row - 1;
            int bottom = location.row + 1;
            int left = location.column - 1;
            int right = location.column + 1;

            if (combatant.location.row == top && combatant.location.column == left
            || combatant.location.row == top && combatant.location.column == location.column
            || combatant.location.row == top && combatant.location.column == right
            || combatant.location.row == location.row && combatant.location.column == left
            || combatant.location.row == location.row && combatant.location.column == right
            || combatant.location.row == bottom && combatant.location.column == left
            || combatant.location.row == bottom && combatant.location.column == location.column
            || combatant.location.row == bottom && combatant.location.column == right)
            {
                return true;
            }

            return false;
        }

        public void BecomeDead()
        {
            battleManager.CreateDeathMarker(this.name, this.location);

            textureName = "skull";
            canMove = false;
        }

        public void TakeDamage(int damage)
        {
            if (currentHitPoints > 0)
            {
                currentHitPoints -= damage;
                if (currentHitPoints <= 0)
                {
                    currentHitPoints = 0;
                    AudioManager.Play("death2");
                    battleManager.RemoveCombatant(this);
                    BecomeDead();
                }
            }
        }

        public bool CanMoveToLocation(Location location)
        {
            return battleManager.dungeon.LocationInDungeon(location) &&
                battleManager.dungeon.LocationIsOpenSpace(location) &&
                battleManager.CombatantNotHere(location);
        }

        public void Move(string direction)
        {
            if (!canMove)
                return;

            int row = location.row;
            int column = location.column;
            Location newLocation = null;

            if (movementLeft > 0)
            {
                switch (direction)
                {
                    case "Left": newLocation = new Location(row, column - 1); break;
                    case "Right": newLocation = new Location(row, column + 1); break;
                    case "Up": newLocation = new Location(row - 1, column); break;
                    case "Down": newLocation = new Location(row + 1, column); break;
                    case "UpLeft": newLocation = new Location(row - 1, column - 1); break;
                    case "DownLeft": newLocation = new Location(row + 1, column - 1); break;
                    case "UpRight": newLocation = new Location(row - 1, column + 1); break;
                    case "DownRight": newLocation = new Location(row + 1, column + 1); break;
                    default: Console.WriteLine("Invalid movement command...FIX IT NOW!!!!!! THE HORROR!!!!!"); break;
                }

                if (CanMoveToLocation(newLocation))
                {
                    AudioManager.PlayRandomStepSound();
                    movementLeft--;
                    location.row = newLocation.row;
                    location.column = newLocation.column;
                }

            }
            else
            {
                Console.WriteLine("No movement left.");
            }


        }
        
        public void MoveDirectlyToLocation(Location location)
        {
            this.location = new Location(location.row, location.column);
            movementLeft--;
            AudioManager.PlayRandomStepSound();
        }
        
        public void Draw(Camera camera)
        {
            if (!visible)
                return;

            if (camera.LocationInView(location))
                TileRenderer.RenderTile(textureName, camera.WorldToCurrentViewLocation(location));

            RenderHealthBar(camera);
        }

    }
}
