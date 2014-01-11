using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace DungeonCombat
{
    class Weapon
    {
        public string name = null;
        public int damage = 0;
        public int range = 0;
        public Combatant combatant = null;
        public string soundName = null;
        

        public Weapon(Combatant combatant, string name, int damage, string soundName, int range)
        {
            this.name = name;
            this.damage = damage;
            this.soundName = soundName;
            this.range = range;
            this.combatant = combatant;
        }

        public Weapon(string name, int damage, int range, string soundName)
        {
            this.name = name;
            this.damage = damage;
            this.range = range;
            this.soundName = soundName;
        }

        public Weapon(Weapon weapon)
        {
            this.name = weapon.name;
            this.damage = weapon.damage;
            this.soundName = weapon.soundName;
            this.range = weapon.range;
            this.combatant = weapon.combatant;
        }

        public void Attack(Combatant combatant)
        {
            if (this.combatant != combatant &&
                this.combatant.battleManager.DistanceBetweenLocations(this.combatant.location, combatant.location) <= this.range)
            {
                combatant.TakeDamage(GetDamage());
                AudioManager.Play(soundName);
            }
            else
            {
                Console.WriteLine("Out of range!");
            }
        }

        public int GetDamage()
        {
            return Die.random.Next(1, damage + 1);
        }
    }
}
