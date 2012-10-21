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
    class Muspekare : Obj
    {
        MouseState mouse;

        public Muspekare(Vector2 pos)
            : base(pos)
        {
            postion = pos;
            spritename = "sikte test";
            
        }

        public override void Update()
        {
            mouse = Mouse.GetState();
            postion = new Vector2(mouse.X, mouse.Y);

            base.Update();
        }
    }
}
