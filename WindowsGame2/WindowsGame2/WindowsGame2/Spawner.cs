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
    class Spawner : Obj
    {
        private int spawnTimer;
        private int spawnTime = 60*3;

        public Spawner(Vector2 pos)
            : base(pos)
        {
            postion = pos;
            spritename = "Aftonbladet hus";
            
        }

        public override void Update()
        {
            IncrementTimers();

            if (spawnTimer > spawnTime)
            {
                spawnTimer = 0;

                foreach (Obj kula in Item.objList)
                {
                    if (kula.GetType() == typeof(Enemy) && !kula.alive)
                    {
                        kula.alive = true;
                        kula.postion = postion;

                        break;

                    }
                }
            }

        }

        private void IncrementTimers()
        {
            spawnTimer++;
        }
    }
}
