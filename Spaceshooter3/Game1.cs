using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Spaceshooter3
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        SpriteFont myFont;
        HighScore highScore;
        StatMaster statMaster;

        enum State { PrintHighScore, EnterHighScore };
        State currentStateScore;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            GameElements.currentState = GameElements.State.Menu;
            GameElements.Initialize();

            if (File.Exists("highscore.txt"))
            {
                highScore = new HighScore(10);
                highScore.LoadFromFile("highscore.txt");
            }
            else
            {
                highScore = new HighScore(10);
            }

            statMaster = new StatMaster();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            myFont = Content.Load<SpriteFont>("myFont");

            GameElements.LoadContent(Content, Window);
            

        }

        //UnloadContent(), anropas då spelet avslutat. Här kan man ladda ur de objekt som skulle kunna behöva det för att rensa minne

        protected override void UnloadContent()
        {
            base.UnloadContent();
            highScore.SaveToFile("highscore.txt");

        }

        protected override void Update(GameTime gameTime)
        {
            //Stänger av spelet om man trycker på back-knappen på gamepaden
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }
            switch (GameElements.currentState)
            {
                case GameElements.State.Run:
                    GameElements.currentState = GameElements.RunUpdate(Content, Window, gameTime);
                    break;
                case GameElements.State.HighScore:
                    GameElements.currentState = GameElements.HighScoreUpdate(gameTime);
                    break;
                case GameElements.State.Quit:
                    this.Exit();
                    break;
                case GameElements.State.Shop:
                    GameElements.currentState = GameElements.ShopUpdate(gameTime);
                    break;
                
                

                default: //menyn
                    GameElements.currentState = GameElements.MenuUpdate(gameTime);
                    break;
            }
            //Aktiverings logiken för highscore, vilket tillåter en att sätta in värden och se på highscore värden

            if (GameElements.currentState == GameElements.State.HighScore)
            {
                switch (currentStateScore)
                {
                    case State.EnterHighScore: // Skriv in oss i listan
                                               // Fortsätt så länge HighScore.EnterUpdate() returnerar true:
                                               // Tyvärr behövde jag använda en global variabel här, alltså Points från StatMaster classen
                        if (highScore.EnterUpdate(gameTime, GameElements.GetStatMasterInstance().Points))
                            currentStateScore = State.PrintHighScore;
                        break;
                    default: // Highscore-listan (tar emot en tangent)
                        KeyboardState keyboardState = Keyboard.GetState();
                        if (keyboardState.IsKeyDown(Keys.E))
                            currentStateScore = State.EnterHighScore;
                        break;
                }
            }
            


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            switch (GameElements.currentState)
            {
                case GameElements.State.Run: //Kör själva spelet
                    GameElements.RunDraw(spriteBatch);
                    break;
                case GameElements.State.HighScore:
                    GameElements.HighScoreDraw(spriteBatch, myFont);
                    break;
                case GameElements.State.Quit:
                    this.Exit();
                    break;
                case GameElements.State.Shop:
                    GameElements.ShopDraw(spriteBatch);
                    break;
                default: //menyn
                    GameElements.MenuDraw(spriteBatch);
                    break;
            }
            //Logik som ritar ut saker i Highscore beroende på vilket state highscore är i.
            if (GameElements.currentState == GameElements.State.HighScore)
            {
                switch (currentStateScore)
                {
                    case State.EnterHighScore: // Skriv in oss i listan
                        highScore.EnterDraw(spriteBatch, myFont);
                        break;
                    default: // Rita ut highscore-listan
                        highScore.PrintDraw(spriteBatch, myFont);
                        break;
                }
            }
            


            spriteBatch.End();

            base.Draw(gameTime);
        }
    


    }
}

     