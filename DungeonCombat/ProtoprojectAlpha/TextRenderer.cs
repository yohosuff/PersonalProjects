using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using QuickFont;
using OpenTK;
using OpenTK.Graphics;

namespace DungeonCombat
{
    class TextRenderer
    {
        static public QFont qFont = null;
        static public Camera camera = null;

        static public void Initialize(Camera camera)
        {
            qFont = new QFont(new Font(FontFamily.GenericSansSerif, 16));
            TextRenderer.camera = camera;
        }

        static public void DrawText(string text, int x, int y)
        {
            QFont.Begin();
            
            qFont.Print(text, new Vector2(x, y));
            
            QFont.End();
        }

        static public void AddTextToSidePanel(string text)
        {
            
        }

        public static void DrawTextBesideLocation(string text, Location location, int line)
        {
            Location relativeLocation = camera.WorldToCurrentViewLocation(location);
            DrawText(text, (relativeLocation.column + 1) * Tile.size + 5, relativeLocation.row * Tile.size + line * (int)qFont.Measure("T").Height);
        }

        static public void SetColor(Color4 color)
        {
            qFont.Options.Colour = color;
        }

        static public void ResetColor()
        {
            qFont.Options.Colour.A = 1;
            qFont.Options.Colour.B = 1;
            qFont.Options.Colour.G = 1;
            qFont.Options.Colour.R = 1;
        }

        static public void DrawTextAtLine(string text, int x, int line, Color color)
        {
            
            QFont.Begin();
            SetColor(color);
            qFont.Print(text, new Vector2(x, line * qFont.Measure("T").Height));
            QFont.End();
            
        }

    }
}
