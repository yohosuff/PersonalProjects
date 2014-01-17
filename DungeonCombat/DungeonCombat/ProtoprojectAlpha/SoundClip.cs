using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCombat
{
    class SoundClip
    {
        public string path = null;
        public string name = null;

        public SoundClip(string path, string name)
        {
            this.path = path;
            this.name = name;
        }
    }
}
