using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace DungeonCombat
{
    enum TileType { OpenFloorSpace, Pillar, EmptyTile };
    
    class Tile
    {
        static public int size = 64;
        public TileType tileType;
        public int row;
        public int column;

        public bool visible = true;
        
        //0 - open floor space
        //1 - wall

        public Tile(int row, int column, TileType tileType = TileType.OpenFloorSpace, bool visible = true)
        {
            this.tileType = tileType;
            this.row = row;
            this.column = column;
            this.visible = visible;
            
        }

        public void Draw()
        {
            if (visible)
            {
                switch (tileType)
                {
                    case TileType.OpenFloorSpace:
                        TileRenderer.RenderTile("floor", column * size, row * size);
                        break;
                    case TileType.Pillar:
                        TileRenderer.RenderTile("pillar", column * size, row * size);
                        break;
                    default:
                        TileRenderer.RenderTile("bricks", column * size, row * size);
                        break;
                }
            }
            
        }
        
    }
}
