using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace DungeonCombat
{
    class TextureInfo
    {
        public int id = 0;
        public string name = "";
        public string path = "";
        public Bitmap bitmap = null;

        public TextureInfo(string path, string name)
        {
            this.path = path;
            this.name = name;
        }

        public TextureInfo(Bitmap bitmap, string name)
        {
            this.name = name;
            this.bitmap = bitmap;
        }
    }
}
