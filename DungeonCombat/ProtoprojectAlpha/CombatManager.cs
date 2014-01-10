using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace DungeonCombat
{
    enum CombatantState { Moving, Attacking };

    class CombatManager
    {
        public Dungeon dungeon = null;
        Queue<Combatant> combatants = null;
        List<DeathMarker> deathMarkers = null;
        public Cursor cursor = null;
        public CombatantState currentCombatantState = CombatantState.Moving;
        public bool drawWeaponButtons = false;
        Camera camera = null;

        public CombatManager(Camera camera)
        {
            this.camera = camera;

            combatants = new Queue<Combatant>();
            deathMarkers = new List<DeathMarker>();
            dungeon = new Dungeon(this);
            dungeon.LoadDungeonConfiguration1();
            cursor = new Cursor(this);
        }

        public void CreateCombatant(string name, Location location, string textureName)
        {
            Combatant newCombatant = new Combatant(name, textureName, this);

            newCombatant.SetMovementPerRound(5);
            newCombatant.ResetMovementLeft();
            newCombatant.SetLocation(location);
            newCombatant.SetHitPoints(10);
            newCombatant.AddWeapon(WeaponManager.GetRandomWeapon());
            newCombatant.AddWeapon(WeaponManager.GetRandomWeapon());
            newCombatant.AddWeapon(WeaponManager.GetRandomWeapon());
            newCombatant.AddWeapon(WeaponManager.GetRandomWeapon());
            combatants.Enqueue(newCombatant);

            RollInitiatives();

            camera.SetTarget(GetCurrentCombatant().location);
        }       

        public void DetermineBattleEntityVisibility()
        {
            bool[,] visibility = VisionCalculator.GetVisionArray();

            foreach (Combatant combatant in combatants)
                combatant.visible = visibility[combatant.location.row, combatant.location.column];
            foreach (DeathMarker deathMarker in deathMarkers)
                deathMarker.visible = visibility[deathMarker.location.row, deathMarker.location.column];
        }

        public void CreateCombatant(string name, string textureName)
        {
            CreateCombatant(name, dungeon.GetRandomEmptyLocation(), textureName);
        }

        public void CreateDeathMarker(string name, Location location)
        {
            deathMarkers.Add(new DeathMarker(name, location));
        }

        public int DistanceBetweenLocations(Location location1, Location location2)
        {
            location1 = new Location(location1.row, location1.column);
            location2 = new Location(location2.row, location2.column);

            int distance = 0;

            while (location1.row != location2.row || location1.column != location2.column)
            {
                if (location1.row != location2.row)
                    location1.row += (location1.row < location2.row) ? 1 : -1;
                if (location1.column != location2.column)
                    location1.column += (location1.column < location2.column) ? 1 : -1;
                distance++;
            }

            return distance;
        }

        public bool CombatantIsHere(Location location)
        {
            foreach (Combatant combatant in combatants)
                if (combatant.location.row == location.row && combatant.location.column == location.column)
                    return true;
            return false;
        }

        public bool CombatantNotHere(Location location)
        {
            return !CombatantIsHere(location);
        }

        public bool CombatantIsHere(int row, int column)
        {
            foreach (Combatant combatant in combatants)
                if (combatant.location.row == row && combatant.location.column == column)
                    return true;
            return false;
        }

        public bool DeathMarkerIsHere(int row, int column)
        {
            foreach (DeathMarker deathMarker in deathMarkers)
                if (deathMarker.location.row == row && deathMarker.location.column == column)
                    return true;
            return false;
        }

        public DeathMarker GetDeathMarkerAt(Location location)
        {
            foreach (DeathMarker deathMarker in deathMarkers)
                if (deathMarker.location.row == location.row && deathMarker.location.column == location.column)
                    return deathMarker;
            return null;
        }
        
        public Combatant GetCombatantAt(Location location)
        {
            foreach (Combatant combatant in combatants)
                if (combatant.location.row == location.row && combatant.location.column == location.column)
                    return combatant;
            return null;
        }

        public Combatant GetCombatantAt(int row, int column)
        {
            return GetCombatantAt(new Location(row, column));
        }

        public void RemoveCombatant(Combatant combatant)
        {
            List<Combatant> tempList = new List<Combatant>(combatants);
            tempList.Remove(combatant);
            combatants = new Queue<Combatant>(tempList);
        }

        public void RollInitiatives()
        {
            Random random = new Random();
            int randomIndex = 0;
            Combatant temp = null;
            List<Combatant> tempList = new List<Combatant>(combatants);
            for (int i = 0; i < combatants.Count; ++i)
            {
                randomIndex = random.Next(combatants.Count);

                temp = tempList[randomIndex];
                tempList[randomIndex] = tempList[i];
                tempList[i] = temp;
            }
            combatants = new Queue<Combatant>(tempList);
        }

        public Combatant GetCurrentCombatant()
        {
            if (combatants.Count > 0)
                return combatants.First();
            return null;
        }

        public List<Location> GetLocationsOfCombatants(List<Combatant> combatants)
        {
            List<Location> locations = new List<Location>();
            foreach (Combatant combatant in combatants)
            {
                locations.Add(new Location(combatant.location.row, combatant.location.column));
            }
            return locations;
        }

        

        public static bool LocationInListOfLocations(List<Location> locations, Location locationToCheck)
        {
            foreach (Location location in locations)
                if (location.row == locationToCheck.row && location.column == locationToCheck.column)
                    return true;
            return false;
        }

        public void Draw(Camera camera)
        {
            GL.Color4(Color.White);
            
            DetermineBattleEntityVisibility();
            dungeon.Draw(camera);

            foreach (DeathMarker deathMarker in deathMarkers)
                deathMarker.Draw(camera);

            foreach (Combatant combatant in combatants)
                combatant.Draw(camera);
            
            if (cursor.enabled)
                cursor.Draw(camera);

            switch (currentCombatantState)
            {
                case CombatantState.Moving:
                    if (this.GetCurrentCombatant().movementLeft > 0)
                        TileRenderer.RenderTiles("green", this.GetCurrentCombatant().GetOpenLocationsAroundMe(), camera);
                    break;
                case CombatantState.Attacking:
                    TileRenderer.RenderTiles("red", GetLocationsOfCombatants(GetCurrentCombatant().GetVisibleCombatantsInRangeOfMe(GetCurrentCombatant().equippedWeapon.range)), camera);
                    break;
            }

        }

        public void EndCurrentCombatantsTurn()
        {
            Combatant combatant = combatants.Dequeue();
            combatant.ResetMovementLeft();
            combatants.Enqueue(combatant);
        }
    }


}
