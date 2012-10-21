using System;
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
    class Bullets : Obj
    {
        private int dmg;

        public Bullets(Vector2 pos)
            : base(pos)
        {
            postion = pos;
            spritename = "9mm1";
            dmg = 0;
            
        }

        public int damage
        {
            set { dmg = value; }
            get { return dmg; }

        }

        public override void Update()
        {
            if (!alive) return;

            //collison with walls
            if (collision(Vector2.Zero, new Wall(new Vector2(0, 0)))) // colision
            {
                alive = false;
            }

            //collison with enemys
            Obj kula = collision(new Enemy(new Vector2(0, 0)));
            if (kula.GetType() == typeof(Enemy))
            {
                alive = false;
                Enemy e = (Enemy)kula;//convert it to an enemy
                e.damage(3);//set the amount of dmg the bullet will do

            }

            if (postion.X < 0 || postion.Y < 0 || postion.X > Game1.room.Width || postion.Y > Game1.room.Height) // kular dör
            {
                alive = false;
            }

            base.Update();
        }
    }
}
