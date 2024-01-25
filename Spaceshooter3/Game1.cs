using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Spaceshooter3
{
    public class Game1 : Game
    {

        //TA BORT HIGHSCORE KOD? HIGHSCORE EJ KLAR!!!!!!!!!!!!!!!!

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
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            GameElements.currentState = GameElements.State.Menu;
            GameElements.Initialize();
            highScore = new HighScore(10);
            statMaster = new StatMaster();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            myFont = Content.Load<SpriteFont>("myFont");

            GameElements.LoadContent(Content, Window);
            

            // TODO: use this.Content to load your game content here
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
                case GameElements.State.Continue:
                    GameElements.currentState = GameElements.RunUpdate(Content, Window, gameTime);
                    break;
                

                default: //menyn
                    GameElements.currentState = GameElements.MenuUpdate(gameTime);
                    break;
            }
            if (GameElements.currentState == GameElements.State.HighScore)
            {
                switch (currentStateScore)
                {
                    case State.EnterHighScore: // Skriv in oss i listan
                                               // Fortsätt så länge HighScore.EnterUpdate() returnerar true:
                        if (highScore.EnterUpdate(gameTime, statMaster.Points))
                            currentStateScore = State.PrintHighScore;
                        break;
                    default: // Highscore-listan (tar emot en tangent)
                        KeyboardState keyboardState = Keyboard.GetState();
                        if (keyboardState.IsKeyDown(Keys.E))
                            currentStateScore = State.EnterHighScore;
                        break;
                }
            }
            

            // TODO: Add your update logic here

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
                //case GameElements.State.GameOver:
                //    GameElements.GameOver(spriteBatch);
                    break;
                default: //menyn
                    GameElements.MenuDraw(spriteBatch);
                    break;
            }
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
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    


    }
}

     