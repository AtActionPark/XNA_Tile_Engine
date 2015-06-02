using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TileEngine
{
    class Static
    {
        public static Vector2 ScreenSize = new Vector2(800, 480);
        public static Random random = new Random();
        public static Vector2 MapSize = new Vector2(100, 100);

        public static int tileSize = 32;
        public static bool DrawInfo = false;
    }
}
