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
    class Item
    {
        public static List<Obj> objList = new List<Obj>();

        public static void Initilize()
        {
            for (int i = 0; i <64; i++)
            {
                Obj Kula = new Bullets(new Vector2(0, 0));
                Kula.alive = false;
                objList.Add(Kula);

            }

            objList.Add(new Wall(new Vector2(500, 15)));//adding new vectors
            objList.Add(new Wall(new Vector2(500, 50)));
            objList.Add(new Wall(new Vector2(500, 85)));
            objList.Add(new Wall(new Vector2(500, 120)));
            objList.Add(new Wall(new Vector2(500, 155)));
            objList.Add(new Wall(new Vector2(500, 190))); //6 walls

            objList.Add(new Wall(new Vector2(500, 710)));
            objList.Add(new Wall(new Vector2(500, 745)));
            objList.Add(new Wall(new Vector2(500, 780)));
            objList.Add(new Wall(new Vector2(500, 815)));
            objList.Add(new Wall(new Vector2(500, 850)));
            objList.Add(new Wall(new Vector2(500, 885))); //6 walls

            objList.Add(new Kungen(new Vector2(300, 400)));
            objList.Add(new Muspekare(new Vector2(0, 0)));

            objList.Add(new Spawner(new Vector2(400, 400)));

            for (int i = 0; i < 16; i++)
            {
                Enemy e = new Enemy(new Vector2(500, 500));
                e.alive = false;

                objList.Add(e);
            }
        }

        public static void Reset()
      {
          foreach (Obj Kula in objList)
          {
              Kula.alive = false;
          }

      }
    }
}
