using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame2
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Vector2 postion;
        public static SpriteFont font;
        public static Rectangle room;
        public Texture2D textureoverlay;
        Song royal;
        Song The;
        Song blargh3;
        int playqueue = 1;
        public static string Gamestate = "Game";

        Kungen Kungen1 = new Kungen(new Vector2(100, 100));
        Muspekare Muspekare = new Muspekare(new Vector2(0, 0));


        public Game1()
        {
            
            Window.Title = "Kungens Återkommst";
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            graphics.IsFullScreen = false;
            
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            Item.Initilize();
            room = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("SpriteFont1");
            textureoverlay = Content.Load<Texture2D>("test backrund");
            royal = Content.Load<Song>("royal");
            The = Content.Load<Song>("The");
            MediaPlayer.IsRepeating = true; 


            foreach (Obj Kula in Item.objList)
            {
                Kula.LoadContent(this.Content);
            }
        }


        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard;
            keyboard = Keyboard.GetState();
            KeyboardState KeyState = Keyboard.GetState();

        
            if (MediaPlayer.State.Equals(MediaState.Paused))
            {
                if (playqueue == 1)
                {
                    MediaPlayer.Play(royal);
                    playqueue = 2;
                }
                else if (playqueue == 2)
                {
                    MediaPlayer.Play(The);
                    playqueue = 3;
                }
                else if (playqueue == 3)
                {
                    //etc. etc. etc.;
                }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Gamestate == "Exit")
                this.Exit();

            if (KeyState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            foreach (Obj Kula in Item.objList)
            {
                Kula.Update();
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(textureoverlay, new Vector2(0f, 0f), Color.White);

            
            foreach (Obj Kula in Item.objList)
            {
                Kula.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
