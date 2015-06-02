using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
    class Tile
    {
        #region properties
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private Texture2D texture;
        public Texture2D Texture
        {
            get { if ((Closed) && Static.DrawInfo) return Art.GreyTile; else return texture; }
            set { texture = value; }
        }

        private Color color; 
        public Color Color
        {
            get {return color;}
            set { color = value; }
        }
        #endregion

        #region PathFinding properties
        private Tile parentTile;
        public Tile ParentTile
        {
            get { return parentTile; }
            set { parentTile = value; }
        }

        public float F { get; set; }
        public float G { get; set; }
        public float H { get; set; }

        private int cost = 0;
        public int Cost
        {
            get{return cost;}
        }

        private bool isWalkable = true;
        public bool IsWalkable
        {
            get { return isWalkable; }
        }

        //open and closed as bool to avoid checking if tile is on open or closed list
        private bool open = false;
        public bool Open
        {
            get { return open; }
            set { open = value; }
        }

        private bool closed = false;
        public bool Closed
        {
            get { return closed; }
            set { open = value; }
        }
        #endregion

        #region Constructors
        public Tile()
        {
        }

        public Tile(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
            color = Color.White;
            F = 0;
            G = 0;
            H = 0;
        }

        public Tile(Vector2 position, Texture2D texture, int Cost)
        {
            this.position = position;
            this.texture = texture;
            this.cost = Cost;
            color = Color.White;
            F = 0;
            G = 0;
            H = 0;
        }

        public Tile(Vector2 position, Texture2D texture, int Cost, bool isWalkable)
        {
            this.position = position;
            this.texture = texture;
            this.cost = Cost;
            this.isWalkable = isWalkable;
            color = Color.White;
            F = 0;
            G = 0;
            H = 0;
        }
        #endregion
    }
}
