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
    class Kungen : Obj
    {
        KeyboardState keyboard;
        KeyboardState prevKeyboard;

        MouseState mouse;
        MouseState prevMouse;

        float spd;
        float bSpd = 20; //Bullet speed

        const int maxhp = 100;
        int hp;

        const int maxAmmo = 32;//Maxammo
        int ammo = 32;
        int rate = 20; //Firing speed
        int firingTimer = 0;

        int reloadTimer = 0;//Reload timer
        int reloadTime = 60 * 2;
        bool reloading = false;

        public static Kungen kungen;


        public Kungen(Vector2 pos)
            : base(pos)
        {

            postion = pos;
            spd = 5;
            spritename = "Kungen1";
            kungen = this;
            hp = maxhp;
        }
        public override void Update()
        {
            kungen = this;
            
            if (!alive) return;

            /// </Controller>


            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();

            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            if (gamepadState.IsButtonDown(Buttons.LeftThumbstickLeft) && !collision(new Vector2(-spd, 0), new Wall(new Vector2(0, 0))))
            {
                postion = new Vector2(postion.X - 5, postion.Y);
            }
            if (gamepadState.IsButtonDown(Buttons.LeftThumbstickRight) && !collision(new Vector2(spd, 0), new Wall(new Vector2(0, 0))))
            {
                postion = new Vector2(postion.X + 5, postion.Y);
            }
            if (gamepadState.IsButtonDown(Buttons.LeftThumbstickUp) && !collision(new Vector2(0, -spd), new Wall(new Vector2(0, 0))))
            {
                postion = new Vector2(postion.X, postion.Y - 5);
            }
            if (gamepadState.IsButtonDown(Buttons.LeftThumbstickDown) && !collision(new Vector2(0, spd), new Wall(new Vector2(0, 0))))
            {
                postion = new Vector2(postion.X, postion.Y + 5);
            }
            if (gamepadState.IsButtonDown(Buttons.X))
            {

                reloading = true;

            }
            CheckReload();


            if (gamepadState.IsButtonDown(Buttons.RightThumbstickUp))
            {
                postion = new Vector2(mouse.X, mouse.Y - 8);
            }
            if (gamepadState.IsButtonDown(Buttons.RightThumbstickLeft))
            {
                postion = new Vector2 (mouse.X - 8, mouse.Y);
            }
            if (gamepadState.IsButtonDown(Buttons.RightThumbstickRight))
            {
                rotation = point_direction(postion.X, postion.Y, mouse.X + 8, mouse.Y);
            }
            if (gamepadState.IsButtonDown(Buttons.RightThumbstickDown))
            {
                
            }
            firingTimer++;
            if (gamepadState.IsButtonDown(Buttons.RightShoulder))
            {
                CheckShooting();
            }

            KeyboardState KeyState = Keyboard.GetState();
            if ((KeyState.IsKeyDown(Keys.W) || KeyState.IsKeyDown(Keys.Up)) && !collision(new Vector2(0, -spd), new Wall(new Vector2(0, 0))))
            {
                postion.Y -= spd;
            }
            if ((KeyState.IsKeyDown(Keys.A) || KeyState.IsKeyDown(Keys.Left)) && !collision(new Vector2(-spd, 0), new Wall(new Vector2(0, 0))))
            {
                postion.X -= spd;
            }
            if ((KeyState.IsKeyDown(Keys.S) || KeyState.IsKeyDown(Keys.Down)) && !collision(new Vector2(0, spd), new Wall(new Vector2(0, 0))))
            {
                postion.Y += spd;
            }
            if ((KeyState.IsKeyDown(Keys.D) || KeyState.IsKeyDown(Keys.Right)) && !collision(new Vector2(spd, 0), new Wall(new Vector2(0, 0))))
            {
                postion.X += spd;
            }
            
                firingTimer++;
                if (mouse.LeftButton == ButtonState.Pressed && !reloading)
                {
                    CheckShooting();
                }


            if(keyboard.IsKeyDown(Keys.R))
            {
                reloading = true;
            }
            CheckReload();

            rotation = MathHelper.ToDegrees(point_direction(postion.X, postion.Y, mouse.X, mouse.Y));
                /// </Controller>

            if (hp <= 0)
            {
                Game1.Gamestate = "Exit";
            }


                prevKeyboard = keyboard;
                prevMouse = mouse;
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game1.font, "Ammo: " + ammo, Vector2.Zero, Color.Black);

            if (reloading)
            {
                spriteBatch.DrawString(Game1.font, "RELOADING:", new Vector2(0, Game1.font.LineSpacing), Color.Red);

            }

            spriteBatch.DrawString(Game1.font, "Health: " + hp + "/" + maxhp, new Vector2(0, Game1.font.LineSpacing*2), Color.DeepSkyBlue);

            base.Draw(spriteBatch);
        }

        private void CheckReload()//Reloading method
        {
            if (reloading)
                reloadTimer++;
          

            if (reloadTimer > reloadTime)
            {
                weapon1reload.Play();
                ammo = maxAmmo;
                reloadTimer = 0;
                reloading = false;
            }
        }


        private void CheckShooting()//This will check if you are actually shooting and can shoot
        {
            if (firingTimer > rate && ammo > 0)
            {
                firingTimer = 0;
                Shoot();

            }

        }

        private void Shoot()//This will shoot the bullet and lose 1 ammo
        {
            weapon1.Play(); ammo--;

            foreach (Obj Kula in Item.objList)
          {
            
                if(Kula.GetType() == typeof(Bullets) && !Kula.alive)
                {
                    Bullets b = (Bullets)Kula;

                    b.postion = postion;
                    b.UpdateArea();
                    b.rotation = rotation;
                    b.speed = bSpd;
                    b.alive = true;
                    b.damage = 3;
                    

                    break;
                }
          }
        }

        public void damage(int dmg)
        {
            hp -= dmg;
        }

        private float point_direction(float x, float y, float x2, float y2)//Mouse pointing direction
        {

    float diffx = x2 - x; //Get the difference between the X and Y of the player and the mouse
    float diffy = y2 - y; //Get the difference from the mouse position to the sprite position, this makes it point the right way
    float res = (float)Math.Atan2(diffy, diffx); //Perform math!
    return res;

        }
       }
   }

