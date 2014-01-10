using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCombat
{
    class TextureInfo
    {
        public int id = 0;
        public string name = "";
        public string path = "";

        public TextureInfo(string path, string name)
        {
            this.path = path;
            this.name = name;

        }
    }
}
