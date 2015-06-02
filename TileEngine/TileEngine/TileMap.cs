using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    class TileMap
    {
        #region private variables
        private int width;
        private int height;
        private Input input;
        private Color pathColor = Color.PaleGreen;
        private Tile tileUnderMouse;
        private int x0, x1, y0, y1; //bounds for culling

        #endregion

        #region properties
        private Tile[,] map;
        public Tile[,] Map
        {
            get { return map; }
        }

        private PathFinding pathFinding;
        public PathFinding PathFinding
        {
            get { return pathFinding; }
        }
        #endregion

        #region Constructor
        public TileMap(Input input, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.input = input;
            map = new Tile[width, height];

            InitializeMap(); 

            tileUnderMouse = Map[0, 0];

            pathFinding = new PathFinding(this);
            DrawPath(pathFinding.CalculatePath());
        }
        #endregion

        #region public methods
        public void Update()
        {
            #region culling
            float tilesOnScreenX = Static.ScreenSize.X / Static.tileSize / Camera.Instance.Zoom;
            float tilesOnScreenY = Static.ScreenSize.Y / Static.tileSize / Camera.Instance.Zoom;

            x0 = (int)(Camera.Instance.Position.X / Static.tileSize - tilesOnScreenX / 2);
            x1 = (int)(Camera.Instance.Position.X / Static.tileSize + tilesOnScreenX / 2 + 1);

            y0 = (int)(Camera.Instance.Position.Y / Static.tileSize - tilesOnScreenY / 2);
            y1 = (int)(Camera.Instance.Position.Y / Static.tileSize + tilesOnScreenY / 2) + 1;

            if (x0 < 0)
                x0 = 0;
            if (x1 > width)
                x1 = width;

            if (y0 < 0)
                y0 = 0;
            if (y1 > height)
                y1 = height;
            #endregion

            #region Input
            //for all tiles on screen
            for (int x = x0; x < x1; x++)
                for (int y = y0; y < y1; y++)
                {
                    //check "selected" tile (tile under mouse)
                    if (input.MouseToGrid == new Vector2(x, y))
                    {
                        tileUnderMouse = Map[x, y];

                        //left clic to change start tile
                        if (input.WasLeftButtonDown && !input.IsKeyDown(Keys.LeftShift) && Map[x, y].IsWalkable)
                            DrawPath(pathFinding.ChangeStart(Map[x, y]));

                        //shift left clic to change end tile
                        else if (input.WasLeftButtonDown && input.IsKeyDown(Keys.LeftShift) && Map[x, y].IsWalkable)
                            DrawPath(pathFinding.ChangeEnd(Map[x, y]));

                        //if shift right click, erase tile
                        else if (input.IsRightButtonDown && input.IsKeyDown(Keys.LeftShift))
                        {
                            Map[x, y] = new Tile(new Vector2(x, y), Art.WhiteTileBorder, 0, true);
                            if (Map[x, y] == pathFinding.End || Map[x, y] == pathFinding.Start)
                                return;
                            DrawPath(pathFinding.CalculatePath());
                        }

                         //if right click, add tile
                        else if (input.IsRightButtonDown)
                        {
                            //ctrl-right click = new water tile
                            if (input.IsRightButtonDown && input.IsKeyDown(Keys.LeftControl) && Map[x, y] != pathFinding.End)
                            {
                                Map[x, y] = new Tile(new Vector2(x, y), Art.BlueTileBorder, 50, true);
                                if (pathFinding.NoPath)
                                    return;
                            }
                            //right click = new wall tile
                            else if (input.IsRightButtonDown && Map[x, y] != pathFinding.End)
                            {
                                Map[x, y] = new Tile(new Vector2(x, y), Art.BlackTileBorder, 0, false);
                                if (pathFinding.NoPath)
                                    return;
                            }
                            DrawPath(pathFinding.CalculatePath());
                        }
                        
                    }
                }
            #endregion  
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = x0; x < x1; x++)
                for (int y = y0; y < y1; y++)
                {
                    Vector2 position = new Vector2(x * Static.tileSize, y * Static.tileSize);

                    //draw tile at position
                    spriteBatch.Draw(Map[x, y].Texture,position , Map[x, y].Color);

                    //colorize tile if under mouse
                    if (tileUnderMouse.Position == new Vector2(x, y))
                        spriteBatch.Draw(Map[x, y].Texture, position, Color.PaleVioletRed);

                        //colorize tile if start or end of path
                    else if (pathFinding.Start.Position == new Vector2(x, y))
                        spriteBatch.Draw(Map[x, y].Texture, position, Color.PaleTurquoise);
                    else if (pathFinding.End.Position == new Vector2(x, y))
                        spriteBatch.Draw(Map[x, y].Texture, position, Color.LightSalmon);

                    if (Static.DrawInfo)
                        DrawPathFindingInfo(spriteBatch, x, y);
                }
        }
        #endregion

        #region private methods
        private void DrawPathFindingInfo(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.DrawString(Art.Font, "G:" + Map[x, y].G, new Vector2(x, y) * Static.tileSize, Color.Red);
            spriteBatch.DrawString(Art.Font, "H:" + Map[x, y].H, new Vector2(x, y + 0.3f) * Static.tileSize, Color.Red);
            spriteBatch.DrawString(Art.Font, "F:" + Map[x, y].F, new Vector2(x, y + 0.6f) * Static.tileSize, Color.Red);
        }

        private void DrawPath(List<Tile> path)
        {
            foreach (Tile t in path)
                t.Color = Color.PaleGreen;
        }

        private void InitializeMap()
        {
            for (int x = 0; x < Static.MapSize.X; x++)
                for (int y = 0; y < Static.MapSize.Y; y++)
                    Map[x, y] = new Tile(new Vector2(x, y), Art.WhiteTileBorder);
        }
        #endregion

    }
}
