using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;// makes so that you can use threads, (program class)
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame2
{
    class Enemy : Obj
    {

        int health;
        const int maxhealth = 10;
        float spd = 2;
        private Vector2 dest;
        private bool[,] map;
        private List<Point> path = new List<Point>();
        private int pathindex = 0;
        private int hitTimer = 0;
        private int hitTime = 60;
        private bool finding = false;
        private Thread t;

        private int dmg;

        public Enemy(Vector2 pos)
            : base(pos)
        {
            postion = pos;
            spritename = "nyc";
            dest = postion;
            health = maxhealth;
            dmg = 5;//the amount of dmg the enemy will do

        }

        public override void Update()
        {
            if (!alive) return;

            Incrementtimer();
            //Method
            MoveToDestination();
            trytohitplayer();


            if (health < 0)
            {
                alive = false;
                health = maxhealth;

            }

            base.Update();
        }

        private void trytohitplayer()
        {
            if(pointdist(postion.X, postion.Y, Kungen.kungen.postion.X, Kungen.kungen.postion.Y) < Pathfinding.gridSize)//if player is whiting reaching distans it will try to hit the player
            {
                if (hitTimer > hitTime)//every second the enemy will try to hit the player
                {
                    hitTimer = 0;//reset the timer
                    Kungen.kungen.damage(dmg);

                }
            }
        }


        public void findPath()
        {
            map = Pathfinding.writeMap(); //check the whole map and divide it into grids based on the size and the grid size and check if there is any collsion.
            Pathfinding finder;
            finder = new Pathfinding(map);
            path = finder.findPath(postion, dest);// It takes the information bool map and takes that information and after you given its postion your at and the destination you want to go to, or want to find a path, it takes that and it finds an array or an list of points. del 1/2
        }

        public void setPath()
        {

            if (Pathfinding.queue < 1 || finding)
            {
                if (!finding)
                {
                    t = new Thread(findPath);// execute that on this thread when it done it sets it paths del 2/2
                    t.Start(); //Starts a new thread
                    finding = true;
                }

                //threads
                if (!t.IsAlive)
                {
                    t.Abort();// checking that its not queued anymore, it has allready find the path, and it will set queued to 0 to reset the path.
                    finding = false;
                pathindex = 0;
                }
            }
        }


        private void MoveToDestination()
        {
            if (path == null)
            {
                pathindex = 0;
                dest = Kungen.kungen.postion;
                setPath();

                return;
            }
            if (pathindex < path.Count)
            {
                if (stepToPoint(path[pathindex]))
                {
                    pathindex++;
                }
            }
            else if (path.Count >= 0)
            {
                path = null;
                pathindex = 0;
                dest = Kungen.kungen.postion;
                setPath();
            }
        }

        private bool stepToPoint(Point point)
        {
            if (pointdist(postion.X, postion.Y, point.X, point.Y) < Pathfinding.gridSize/4) { speed = 0; return true; }

            //face destination
            rotation = point_direction(postion.X, postion.Y, point.X, point.Y);
            speed = spd;

            return false;

        }


        public override void pushto(float pix, float dir)
        {
            float newX = (float)Math.Cos(MathHelper.ToRadians(dir));
            float newY = (float)Math.Sin(MathHelper.ToRadians(dir));
            newX *= pix+5;
            newY *= pix+5;

           // if (!collision(new Vector2(newX, newY), new Wall(Vector2.Zero)))
            //{
                base.pushto(pix, dir);
            //}
        }

        public static float pointdist(float x1, float y1, float x2, float y2)
        {
            float xRect = (x1 - x2 * (x1 - x2));
            float yRect = (y1 - y2 * (y1 - y2));
                double hRect = xRect + yRect;

            float dist = (float)Math.Sqrt(hRect);

            return dist;
        }



        private float point_direction(float x, float y, float x2, float y2)//Mouse pointing direction
        {

            float diffx = x2 - x; //Get the difference between the X and Y of the player and the mouse
            float diffy = y2 - y; //Get the difference from the mouse position to the sprite position, this makes it point the right way
            float res = (float)Math.Atan2(diffy, diffx); //Perform math!
            return res;

        }

        public void damage(int dmg)//dmg method
        {
            health -= dmg;
            //add some blood effects
        }
        private void Incrementtimer()
        {
            hitTimer++;
        }
    }
}
