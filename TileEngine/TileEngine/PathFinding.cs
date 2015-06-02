using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    class PathFinding
    {
        #region private variables
        private TileMap map;
        private List<Tile> openList, closedList;
        private Tile current;
        #endregion

        #region properties
        private Tile start;
        public Tile Start
        {
            get { return start; }
        }

        private Tile end;
        public Tile End
        {
            get { return end; }
        }

        private bool noPath;
        public bool NoPath
        {
            get { return noPath; }
        }
        #endregion

        #region Constructor
        public PathFinding(TileMap map)
        {
            this.map = map;
            openList = new List<Tile>();
            closedList = new List<Tile>();
            start = map.Map[5, 5];
            end = map.Map[10, 10];
            
            AStar();
        }
        #endregion

        #region public methods
        //return a path = list of tiles between current start and end of pathfinder
        public List<Tile> CalculatePath()
        {
            ResetPathfinder();
            return AStar();
        }

        //return a path = list of tiles between 2 tiles
        public List<Tile> CalculatePath(Tile start, Tile end)
        {
            this.start = start;
            this.end = end;
            foreach (Tile tile in map.Map)
            {
                noPath = false;
                tile.Open = false;
                tile.Closed = false;
                tile.ParentTile = null;
                tile.F = 0;
                tile.G = 0;
                tile.H = 0;
                tile.Color = Color.White;
            }
            openList = new List<Tile>();
            closedList = new List<Tile>();

            return AStar();
        }

        //Change start tile and recalculate path
        public List<Tile> ChangeEnd(Tile end)
        {
            this.end = end;
            return CalculatePath();
        }

        //Change end tile and recalculate path
        public List<Tile> ChangeStart(Tile start)
        {
            this.start = start;
            return CalculatePath();
        }
        #endregion

        #region private methods
        //A* Algo
        private List<Tile> AStar()
        {
            current = start;
            openList.Add(start);
            start.Open = true;

            #region loop
            while (current != end)
            {
                //look for lowest F cost tile on the open list
                current = ChooseLowestFTile(openList);

                //switch it to the closed list
                openList.Remove(current);
                current.Open = false;
                closedList.Add(current);
                current.Closed = true;

                //for all neighbors
                foreach (Tile tile in GetNeighbors(current))
                {

                    //if its not walkable or on the closes list, ignore it
                    if (!tile.IsWalkable || tile.Closed)
                    {
                        //
                    }
                    //if its not on the open list, add it, make current tile its parent
                    //and recalculate scores
                    else if (!tile.Open)
                    {
                        openList.Add(tile);
                        tile.Open = true;
                        tile.ParentTile = current;
                        CalculateInfo(tile);
                    }
                    //if its on the open list already, check if that path to the tile is better
                    //if it is, change its parent to current and recalculate scores
                    else if (tile.Open)
                    {

                        if (getTileG(tile, current) < tile.G)
                        {
                            tile.ParentTile = current;
                            CalculateInfo(tile);
                        }
                    }
                }

                //if no path
                if (openList.Count == 0)
                {
                    noPath = true;
                    break;
                }
            }
            #endregion

            return ReturnPath();
        }

        //return list of tiles from start to end tile
        private List<Tile> ReturnPath()
        {
            List<Tile> path = new List<Tile>();
            Tile tile = End;
            path.Add(tile);
            while (tile != Start)
            {
                if (tile.ParentTile == null)
                    return path;

                tile = tile.ParentTile;
                path.Add(tile);
            }
            return path;
        }

        private void ResetPathfinder()
        {
            foreach (Tile tile in map.Map)
            {
                noPath = false;
                tile.Open = false;
                tile.Closed = false;
                tile.ParentTile = null;
                tile.F = 0;
                tile.G = 0;
                tile.H = 0;
                tile.Color = Color.White;
            }
            openList = new List<Tile>();
            closedList = new List<Tile>();
        }

        private Tile ChooseLowestFTile(List<Tile> tiles)
        {
            int fmax = int.MaxValue;
            Tile current = new Tile();

            foreach (Tile tile in tiles)
                if (tile.F < fmax)
                {
                    fmax = (int)tile.F;
                    current = tile;
                }

            return current;
        }

        private float getTileG(Tile tile, Tile parent)
        {
            // G is calculates from parent's G + (1 or sqrt2) depending on parent relative position
            if (tile.Position != Start.Position)
            {
                //tile's parent is vertical or horizontal
                if (tile.Position.X == parent.Position.X || tile.Position.Y == parent.Position.Y)
                {
                    return parent.G + 10 + tile.Cost;
                }
                //tile's parent is diagonal
                else
                    return parent.G + 14+tile.Cost ;
            }
            return 0;
        }

        private void CalculateInfo(Tile tile)
        {
            if (tile.ParentTile == null)
                tile.G = 0;
            else
            {
                if (tile.Position.X == tile.ParentTile.Position.X || tile.Position.Y == tile.ParentTile.Position.Y)
                    tile.G = tile.ParentTile.G + 10 + tile.Cost;
                else
                    tile.G = tile.ParentTile.G + 14 + tile.Cost;
            }

            tile.H = ManhattanDistance(tile, End);
            tile.F = tile.G + tile.H;

        }

        private int ManhattanDistance(Tile start, Tile end)
        {
            int x = (int)Math.Abs(start.Position.X - end.Position.X);
            int y = (int)Math.Abs(start.Position.Y - end.Position.Y);

            return 10 * (x + y);
        }

        private List<Tile> GetNeighbors(Tile tile)
        {
            List<Tile> neighbors = new List<Tile>();
            if (tile.Position.X > 0)
                neighbors.Add(map.Map[(int)tile.Position.X - 1, (int)tile.Position.Y]);
            if (tile.Position.X < map.Map.GetLength(0) - 1)
                neighbors.Add(map.Map[(int)tile.Position.X + 1, (int)tile.Position.Y]);
            if (tile.Position.Y > 0)
                neighbors.Add(map.Map[(int)tile.Position.X, (int)tile.Position.Y - 1]);
            if (tile.Position.Y < map.Map.GetLength(1) - 1)
                neighbors.Add(map.Map[(int)tile.Position.X, (int)tile.Position.Y + 1]);

            if (tile.Position.X > 0 && tile.Position.Y > 0)
                neighbors.Add(map.Map[(int)tile.Position.X - 1, (int)tile.Position.Y - 1]);
            if (tile.Position.X < map.Map.GetLength(0) - 1 && tile.Position.Y > 0)
                neighbors.Add(map.Map[(int)tile.Position.X + 1, (int)tile.Position.Y - 1]);
            if (tile.Position.X > 0 && tile.Position.Y < map.Map.GetLength(1) - 1)
                neighbors.Add(map.Map[(int)tile.Position.X - 1, (int)tile.Position.Y + 1]);
            if (tile.Position.X < map.Map.GetLength(0) - 1 && tile.Position.Y < map.Map.GetLength(1) - 1)
                neighbors.Add(map.Map[(int)tile.Position.X + 1, (int)tile.Position.Y + 1]);

            //foreach (Tile t in neighbors)
            //    CalculateInfo(t);

            return neighbors;
        }
        #endregion
    }
}
