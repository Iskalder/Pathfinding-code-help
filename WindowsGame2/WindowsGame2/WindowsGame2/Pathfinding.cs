﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame2
{
    class Pathfinding
    {
        private Grid[,] map;
        public static int queue = 0;
        public static int Qtimer = 0;
        public const int Qtime = 1;
        public const int gridSize = 32;

        public Pathfinding(bool[,] bMap)
        {
            //read map
            map = new Grid[bMap.GetLength(0), bMap.GetLength(1)];

            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    map[r, c] = new Grid();
                    map[r, c].walkable = bMap[r, c];
                }
            }
        }

        public static bool[,] writeMap(params Obj[] objEx)
        {
            bool[,] cMap = new bool[Convert.ToInt16(Game1.room.Height / Pathfinding.gridSize),
                 Convert.ToInt16(Game1.room.Width / Pathfinding.gridSize)];

            //loop through rows
            for (int r = 0; r < cMap.GetLength(0); r++)
            {
                //loop through collums
                for (int c = 0; c < cMap.GetLength(1); c++)
                {
                    Rectangle rec = new Rectangle();
                    rec.X = c * Pathfinding.gridSize; rec.Y = r * Pathfinding.gridSize;
                    rec.Width = rec.Height = Pathfinding.gridSize;

                    //If collision with grid then grid is unwalkable
                    foreach (Obj kula in Item.objList)
                    {
                        if (kula.area.Intersects(rec) && kula.alive && kula.solid && !objEx.Contains<Obj>(kula)) { cMap[r, c] = false; break; } else { cMap[r, c] = true; }

                        //set all borders to walls
                        if (r == 0 || c == 0 || r == cMap.GetLength(0) - 1 || c == cMap.GetLength(1) - 1)
                        {
                            cMap[r, c] = false; break;
                        }
                    }
                }
            }

            return cMap;
        }

        public List<Point> findPath(Vector2 pos, Vector2 dest)
        {
            resetMap();
            List<Point> path = new List<Point>();
            //List<Point> openList = new List<Point>();
            List<Point> openListB = new List<Point>();
            openListB.Add(new Point(0, 0));
            List<Point> closedList = new List<Point>();

            //Get start and end positions
            Point startPos = new Point(Convert.ToInt16(Math.Floor((double)(pos.Y / gridSize))),
                Convert.ToInt16(Math.Floor((double)(pos.X / gridSize))));
            Point endPos = new Point(Convert.ToInt16(Math.Floor((double)(dest.Y / gridSize))),
                Convert.ToInt16(Math.Floor((double)(dest.X / gridSize))));

            Point curPos = startPos;

            map[curPos.X, curPos.Y].closed = true;
            closedList.Add(curPos);

            while (!closedList.Contains(endPos))
            {
                /* add all adjescent nodes to the open list, 
                 * set the parent of all adjacent nodes to the cur node, 
                 * calculate F score for all adjacent node */
                foreach (Point v in getAdj(curPos))
                {
                    if (map[v.X, v.Y].walkable && !map[v.X, v.Y].closed)
                    {
                        if (!openListB.Contains(v))
                        {
                            //Check if cutting corner
                            bool cutCorner = false;
                            if (getG(v, curPos) == 14)
                            {
                                foreach (Point p in getAdj(v))
                                {
                                    if (!map[p.X, p.Y].walkable && getH(curPos, p) == 10)
                                    {
                                        cutCorner = true;
                                    }
                                }
                            }
                            if (!cutCorner)
                            {
                                openListB.Add(v);
                                map[v.X, v.Y].parent = curPos;
                                map[v.X, v.Y].G = getG(v, curPos);
                                map[v.X, v.Y].H = getH(v, endPos);
                                map[v.X, v.Y].F = map[v.X, v.Y].G + map[v.X, v.Y].H;

                                int m = openListB.Count - 1;
                                while (m != 1) //if node has not gone to top (m=1)
                                {
                                    //If objLIstB[m] fcost <= objListB[m/2] fcost
                                    if (map[openListB[m].X, openListB[m].Y].F <= map[openListB[m / 2].X, openListB[m / 2].Y].F)
                                    {
                                        //swap positions
                                        Point temp = openListB[m / 2];
                                        openListB[m / 2] = openListB[m];
                                        openListB[m] = temp;
                                        m /= 2;
                                    }
                                    else { break; }
                                }
                            }
                        }
                        //else if Cur Node G score + New G score < prev G score
                        else if (map[curPos.X, curPos.Y].G + getG(v, curPos) < map[curPos.X, curPos.Y].G)
                        {
                            map[v.X, v.Y].parent = curPos;
                            map[v.X, v.Y].G = getG(v, curPos);
                            map[v.X, v.Y].H = getH(v, endPos);
                            map[v.X, v.Y].F = map[v.X, v.Y].G + map[v.X, v.Y].H;

                            int m = openListB.IndexOf(v);
                            while (m != 1) //if node has not gone to top (m=1)
                            {
                                //If objLIstB[m] fcost <= objListB[m/2] fcost
                                if (map[openListB[m].X, openListB[m].Y].F <= map[openListB[m / 2].X, openListB[m / 2].Y].F)
                                {
                                    //swap positions
                                    Point temp = openListB[m / 2];
                                    openListB[m / 2] = openListB[m];
                                    openListB[m] = temp;
                                    m /= 2;
                                }
                                else { break; }
                            }
                        }
                    }
                }

                if (!closedList.Contains(endPos) && openListB.Count == 1 && closedList.Count >= 1) { return null; }
                //Choose the node with the lowest F score
                curPos = openListB[1];

                //remove cur node from open list and add to closed list
                map[curPos.X, curPos.Y].closed = true;
                closedList.Add(curPos);
                openListB[1] = openListB[openListB.Count - 1];
                openListB.RemoveAt(openListB.Count - 1);
                int vv = 1;
                while (true)
                {
                    int uu = vv;
                    if (2 * uu + 1 < openListB.Count)
                    {
                        if (map[openListB[uu].X, openListB[uu].Y].F >= map[openListB[uu * 2].X, openListB[uu * 2].Y].F) { vv = uu * 2; }
                        if (map[openListB[vv].X, openListB[vv].Y].F >= map[openListB[uu * 2 + 1].X, openListB[uu * 2 + 1].Y].F) { vv = uu * 2 + 1; }
                    }
                    else if (2 * uu < openListB.Count)
                    {
                        if (map[openListB[uu].X, openListB[uu].Y].F >= map[openListB[uu * 2].X, openListB[uu * 2].Y].F) { vv = uu * 2; }
                    }

                    if (uu != vv)
                    {
                        Point temp = openListB[uu];
                        openListB[uu] = openListB[vv];
                        openListB[vv] = temp;
                    }
                    else { break; }
                }

                //if endPos unreachable return null
                if (!closedList.Contains(endPos) && openListB.Count == 1 && closedList.Count > 1) { return null; }
            }

            Point curNode = endPos;
            path.Add(new Point((curNode.Y * gridSize) - (gridSize / 2),
                    (curNode.X * gridSize) - (gridSize / 2)));
            while (curNode != startPos)
            {
                curNode = map[curNode.X, curNode.Y].parent;
                path.Add(new Point((curNode.Y * gridSize) + (gridSize / 2),
                    (curNode.X * gridSize) + (gridSize / 2)));
            }

            path.Reverse();

            return path;
        }

        private int getH(Point v, Point endPos)
        {
            Point diff = new Point(v.X - endPos.X, v.Y - endPos.Y);
            if (diff.X < 0) { diff.X *= -1; }
            if (diff.Y < 0) { diff.Y *= -1; }

            return Convert.ToInt16(diff.X + diff.Y);
        }

        private int getG(Point v, Point curPos)
        {
            Point diff = new Point(v.X - curPos.X, v.Y - curPos.Y);
            if (diff.Y == 0 || diff.X == 0)
            {
                return 10;
            }
            else
            {
                return 14;
            }
        }

        private void resetMap()
        {
            //read map
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    map[r, c].reset();
                }
            }
        }

        private List<Point> getAdj(Point curPos)
        {
            List<Point> adjList = new List<Point>();

            adjList.Add(new Point(curPos.X - 1, curPos.Y - 1));
            adjList.Add(new Point(curPos.X, curPos.Y - 1));
            adjList.Add(new Point(curPos.X + 1, curPos.Y - 1));
            adjList.Add(new Point(curPos.X - 1, curPos.Y));
            adjList.Add(new Point(curPos.X - 1, curPos.Y + 1));
            adjList.Add(new Point(curPos.X + 1, curPos.Y + 1));
            adjList.Add(new Point(curPos.X, curPos.Y + 1));
            adjList.Add(new Point(curPos.X + 1, curPos.Y));

            return adjList;
        }

        public static void update()
        {
            /*Qtimer++;
            if (Qtimer > Qtime)
            {
                queue = 0;
                Qtimer = 0;
            }*/
        }
    }
}

