using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCombat
{
    class DeathMarker
    {
        public Location location = null;
        public string name = "";
        public bool visible = false;

        public DeathMarker(string name, Location location)
        {
            this.name = name;
            this.location = location;
        }

        public void Draw(Camera camera)
        {
            if (!visible)
                return;

            if (camera.LocationInView(location))
                TileRenderer.RenderTile("skull", camera.WorldToCurrentViewLocation(location));
        }
    }
}
