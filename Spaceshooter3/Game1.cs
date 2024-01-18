using Microsoft.Xna.Framework;
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
        private SpriteBatch _spriteBatch;
        SpriteFont myFont;
        HighScore highScore;
        Random random;
        //enum State { PrintHighScore, EnterHighScore };
        //State currentState;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            GameElements.currentState = GameElements.State.Menu;
            GameElements.Initialize();
            highScore = new HighScore(10);




            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
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
                    GameElements.currentState = GameElements.HighScoreUpdate();
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
            //switch (currentState)
            //{
            //    case State.EnterHighScore: // Skriv in oss i listan
            //                               // Fortsätt så länge HighScore.EnterUpdate() returnerar true:
            //        if (highScore.EnterUpdate(gameTime, 10))
            //            currentState = State.PrintHighScore;
            //        break;
            //    default: // Highscore-listan (tar emot en tangent)
            //        KeyboardState keyboardState = Keyboard.GetState();
            //        if (keyboardState.IsKeyDown(Keys.E))
            //            currentState = State.EnterHighScore;
            //        break;
            //}

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            switch (GameElements.currentState)
            {
                case GameElements.State.Run: //Kör själva spelet
                    GameElements.RunDraw(_spriteBatch);
                    break;
                case GameElements.State.HighScore:
                    GameElements.HighScoreDraw(_spriteBatch);
                    break;
                case GameElements.State.Quit:
                    this.Exit();
                    break;
                case GameElements.State.Shop:
                    GameElements.ShopDraw(_spriteBatch);
                    break;
                default: //menyn
                    GameElements.MenuDraw(_spriteBatch);
                    break;
            }
            
            
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    


    }
}

     