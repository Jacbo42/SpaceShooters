using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceShooter2
{
    public class Game1 : Game
    {

        //TODO, GÖR SÅ ATT FIENDER BÖRJAR LÄNGRE BORT FRÅN SPELAREN
        //GÖR SÅ ATT FIENDER KAN SKAPAS UNDER SPELETS GÅNG ISTÄLLET FÖR ATT DET BARA FINNS DE I BÖRJAN


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Player player;

        List<Enemy> enemies;
        List<GoldCoin> goldCoins;
        Texture2D goldCoinSprite;


        PrintText printText;
        Vector2 ship_vector; //Skeppets position
        Vector2 ship_speed; //Skeppets hastighet
        Vector2 ship_speed2;
        Vector2 ship_vector2;

        Texture2D ship_texture; //Skeppets texture

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

           goldCoins = new List<GoldCoin>();

           

            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player(Content.Load<Texture2D>("ship"), 380, 400, 2.5f, 4.5f, Content.Load<Texture2D>("images/player/bullet"));

            enemies = new List<Enemy>();
            Random random = new Random();
            Texture2D tmpSprite=Content.Load<Texture2D>("mine");

            for (int i = 0; i < 10; i++)
            {
                int rndX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
                int rndy = random.Next(0, Window.ClientBounds.Height / 2);
                Mine temp = new Mine(tmpSprite, rndX, rndy);
                enemies.Add(temp); //Lägg till i listan
            }

            tmpSprite = Content.Load<Texture2D>("tripod");
            for (int i = 0; i < 5 ; i++)
            {
                int rndX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
                int rndY = random.Next(0, Window.ClientBounds.Height / 2);
                Tripod temp = new Tripod(tmpSprite, rndX, rndY);
                enemies.Add(temp);
            }

            goldCoinSprite = Content.Load<Texture2D>("images/powerups/coin");

            printText = new PrintText(Content.Load<SpriteFont>("myFont"));

            

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(Window, gameTime);

            foreach(Enemy e in enemies)
            {
                e.Update(Window);
            }

            // gå igenom alla fiender

            foreach (Enemy e in enemies.ToList())
            {

                foreach (Bullet b in player.Bullets)
                {
                    if (e.CheckCollision(b))
                    {
                        e.IsAlive = false;
                        player.Points++;
                    }
                }


                if (e.IsAlive) // Kontrollera om fienden lever
                {
                    if (e.CheckCollision(player))
                    {
                        /*
                        ============================================
                        GODMODE ENABLE
                        ============================================
                        */
                        //this.Exit();
                    }
                    e.Update(Window); //Flytta på dem
                }
                else // Ta bort fienden för den är död
                {
                    enemies.Remove(e);
                }
            }

            //Guldmynten ska uppstå slumpmässigt, en chans på 200

            Random random = new Random();
            int newCoin = random.Next(1, 200);
            if (newCoin == 1) //Ok, nytt guldmynt ska uppstå
            {
                //var ska guldmyntet ska uppstå:
                int rndX = random.Next(0, Window.ClientBounds.Width - goldCoinSprite.Width);
                int rndY = random.Next(0, Window.ClientBounds.Height - goldCoinSprite.Height);

                goldCoins.Add(new GoldCoin(goldCoinSprite, rndX, rndY, gameTime));
            }
            //Gå igenom hela listan med existerande guldmynt
            foreach (GoldCoin gc in goldCoins.ToList())
            {
                if (gc.IsAlive)// Kontrollerar om guldmyntet lever
                {
                    //gc.Update() kollar om guldmyntet har blivit för gammalt
                    //för att få leva vidare
                    gc.Update(gameTime);

                    //Kontrollera om de kolliderat med spelaren
                    if (gc.CheckCollision(player))
                    {
                        // Ta bort myntet vid kollision:
                        goldCoins.Remove(gc);
                        player.Points++; // och ge spelaren poäng
                    }

                }
                else // ta bort guldmyntet för det är dött
                {
                    goldCoins.Remove(gc);
                }
            }
            


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            player.Draw(_spriteBatch);
            // printText.Print("Points: 0, din latmask", _spriteBatch, new Vector2(0, 0), Color.Black);
            
            foreach (Enemy e in enemies)
            {
                e.Draw(_spriteBatch);
            }
            //printText.Print("antal fiender: " + enemies.Count, _spriteBatch, new Vector2(0, 0),Color.Black);

            foreach (GoldCoin gc in goldCoins)
            {
                gc.Draw(_spriteBatch);
            }
            printText.Print("Points: " + player.Points, _spriteBatch, new Vector2(0, 0), Color.Black);

            _spriteBatch.End();



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    


    }
}

     