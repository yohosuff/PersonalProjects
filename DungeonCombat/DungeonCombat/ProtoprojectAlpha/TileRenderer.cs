using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace DungeonCombat
{
    class TileRenderer
    {
        static List<TextureInfo> textures = null;

        static public void Initialize()
        {
            textures = new List<TextureInfo>();
            
            LoadTileTexture(@"Tiles\floorTile.png", "floor");
            LoadTileTexture(@"Tiles\pillar.png", "pillar");
            LoadTileTexture(@"Tiles\dirt.png", "dirt");
            LoadTileTexture(@"Tiles\ninja.png", "ninja");
            LoadTileTexture(@"Tiles\beserker.png", "beserker");
            LoadTileTexture(@"Tiles\warrior.png", "warrior");
            LoadTileTexture(@"Tiles\cursor.png", "cursor");
            LoadTileTexture(@"Tiles\bricks.png", "bricks");
            LoadTileTexture(@"Tiles\elf.png", "elf");
            LoadTileTexture(@"Tiles\skull.png", "skull");
            LoadTileTexture(@"Tiles\green.png", "green");
            LoadTileTexture(@"Tiles\red.png", "red");

        }

        static public void LoadTileTexture(string fileName, string tileName)
        {
            TextureInfo textureInfo = new TextureInfo(fileName, tileName);
            textureInfo.id = LoadTexture(fileName);
            textures.Add(textureInfo);
        }

        static public void RenderTiles(string name, List<Location> locations, Camera camera)
        {
            foreach (Location location in locations)
            {
                RenderTile(name, camera.WorldToCurrentViewLocation(location));
            }
        }

        static public void RenderTile(string name, Location location)
        {
            RenderTile(name, location.column * Tile.size, location.row * Tile.size);
        }

        static public void RenderTile(string name, int x, int y)
        {
            foreach (TextureInfo textureInfo in textures)
            {
                if (textureInfo.name.Equals(name))
                {
                    RenderTile(textureInfo.id, x, y);
                    break;
                }
            }
        }
        
        static public void RenderTile(int textureIndex, int x, int y)
        {
            
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textureIndex);
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex2(x, y);

            GL.TexCoord2(1, 0);
            GL.Vertex2(x + Tile.size, y);

            GL.TexCoord2(1, 1);
            GL.Vertex2(x + Tile.size, y + Tile.size);

            GL.TexCoord2(0, 1);
            GL.Vertex2(x, y + Tile.size);

            GL.End();
            //GL.Disable(EnableCap.Texture2D);

        }

        static private int LoadTexture(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);

            int id = GL.GenTexture();
            
            //Bind a named texture to a texturing target. 
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap(filename);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            // We haven't uploaded mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // On newer video cards, we can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return id;
        }
    }
}
