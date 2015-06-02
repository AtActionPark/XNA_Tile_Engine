using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TileEngine
{
    public static class Art
    {
        public static Texture2D WhiteTile;
        public static Texture2D BlackTile;
        public static Texture2D GreyTile;
        public static Texture2D Cursor;
        public static Texture2D Cursor2;
        public static Texture2D Selected;
        public static Texture2D WhiteTileBorder;
        public static Texture2D BlackTileBorder;
        public static Texture2D GreenTileBorder;
        public static Texture2D BlueTileBorder;
        public static SpriteFont Font;

        public static void Load(ContentManager Content)
        {
            WhiteTile = Content.Load<Texture2D>(@"Tiles\WhiteTile");
            BlackTile = Content.Load<Texture2D>(@"Tiles\BlackTile");
            GreyTile = Content.Load<Texture2D>(@"Tiles\GreyTile");
            Selected = Content.Load<Texture2D>(@"Tiles\Selected");
            WhiteTileBorder = Content.Load<Texture2D>(@"Tiles\WhiteTileBorder");
            BlackTileBorder = Content.Load<Texture2D>(@"Tiles\BlackTileBorder");
            GreenTileBorder = Content.Load<Texture2D>(@"Tiles\GreenTileBorder");
            BlueTileBorder = Content.Load<Texture2D>(@"Tiles\BlueTileBorder");
            Cursor = Content.Load<Texture2D>("Cursor");
            Cursor2 = Content.Load<Texture2D>("Cursor2");
            Font = Content.Load<SpriteFont>("Font");
        }
    }
}
