using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCombat
{
    class WeaponManager
    {
        public static List<Weapon> weapons = null;

        public static void Initialize()
        {
            weapons = new List<Weapon>();
            
        }

        public static void CreateWeapon(string name, int damage, int range, string soundName)
        {
            weapons.Add(new Weapon(name, damage, range, soundName));
        }

        public static Weapon GetRandomWeapon()
        {
            return new Weapon(weapons.ElementAt(Die.random.Next(weapons.Count())));
        }

        public static Weapon GetWeaponByName(string weaponName)
        {
            foreach (Weapon weapon in weapons)
                if (weapon.name == weaponName)
                    return new Weapon(weapon);
            return null;
        }

        public static void LoadWeaponsFromFile(string path)
        {
            /*
             * weapon file format:
             * name of weapon
             * damage
             * range
             * sound name
             */
            
            List<string> lines = new List<string>(System.IO.File.ReadAllLines(path));
            
            lines.RemoveAll((string line) => 
            {
                if (line.Length <= 0)
                    return true;
                return false;
            });

            for(int i = 0; i < lines.Count; i += 4)
            {
                CreateWeapon(lines[i], int.Parse(lines[i + 1]), int.Parse(lines[i + 2]), lines[i+3]);
            }
            
        }

        public static void LoadWeaponsFromFile(byte[] file)
        {
            /*
             * weapon file format:
             * name of weapon
             * damage
             * range
             * sound name
             */
            string fileString = System.Text.Encoding.UTF8.GetString(file);
            fileString = fileString.Replace("\r","");
            List<string> lines = new List<string>(fileString.Split('\n'));
            
            lines.RemoveAll((string line) =>
            {
                if (line.Length <= 0)
                    return true;
                return false;
            });

            for (int i = 0; i < lines.Count; i += 4)
            {
                CreateWeapon(lines[i], int.Parse(lines[i + 1]), int.Parse(lines[i + 2]), lines[i + 3]);
            }

        }

        public static void DisplayWeaponList()
        {
            foreach (Weapon weapon in weapons)
            {
                Console.WriteLine("---{0}\t{1}\t{2}", weapon.name, weapon.damage, weapon.range);
            }
        }
    }
}
