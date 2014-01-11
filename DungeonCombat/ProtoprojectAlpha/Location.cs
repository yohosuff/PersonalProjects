using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCombat
{
    class Location
    {
        public int row;
        public int column;

        public Location(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
}
