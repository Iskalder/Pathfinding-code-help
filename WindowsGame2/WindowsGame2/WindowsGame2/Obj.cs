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
    class Obj
    {
        public Vector2 postion;
        public  float rotation = 0.0f;
        public Texture2D spriteIndex;
        public float speed = 0.0f;
        public float scale = 1.0f;
        public string spritename;
        public bool alive = true;
        public Rectangle area;
        public bool solid = false;
        public SoundEffect weapon1;
        public SoundEffect weapon1reload;
        

        public Obj(Vector2 pos)
        {
        postion = pos;
        rotation = 0.0f;
        }

        public Obj()
        {
            // TODO: Complete member initialization
        }

        public virtual void Update()
        {
            if (!alive) return;

            UpdateArea();

             if (alive) pushto(speed, rotation);
            
        }

        public virtual void LoadContent(ContentManager content)
        {

            spriteIndex = content.Load<Texture2D>("Bild\\" + this.spritename);
            area = new Rectangle(0, 0, spriteIndex.Width, spriteIndex.Height);
            weapon1 = content.Load<SoundEffect>("M4A1");
            weapon1reload = content.Load<SoundEffect>("reload");
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!alive) return;

                    Vector2 center = new Vector2(spriteIndex.Width / 2, spriteIndex.Height / 2);
           
            spriteBatch.Draw(spriteIndex, postion, null, Color.White, MathHelper.ToRadians(rotation), center, scale, SpriteEffects.None, 0 );
        }

        public bool collision(Vector2 pos, Obj obj)
        {

            Rectangle newArea = new Rectangle(area.X, area.Y, area.Width, area.Height);
            newArea.X += (int)pos.X;
            newArea.Y += (int)pos.Y;

            foreach(Obj kula in Item.objList)
            {
                if(kula.GetType() == obj.GetType() && kula.solid)
                
                    if (kula.area.Intersects(newArea))
                    
                        return true;
                    
                
            }
            return false;
        }

        public Obj collision( Obj obj)
        {

            foreach (Obj kula in Item.objList)
            {
                if (kula.GetType() == obj.GetType() && kula.alive)

                    if (kula.area.Intersects(area))

                        return kula;


            }
            return new Obj();
        }


        public void UpdateArea()
        {
            area.X = (int)postion.X - (spriteIndex.Width/ 2);
            area.Y = (int)postion.Y - (spriteIndex.Height/ 2);
            
        }

        public virtual void pushto(float pix, float dir)
        {
            float newX = (float)Math.Cos(MathHelper.ToRadians(dir));
            float newY = (float)Math.Sin(MathHelper.ToRadians(dir));
            postion.X += pix * (float)newX; //Set the new X position
            postion.Y += pix * (float)newY; //Set the new Y position
        }
    }
}
