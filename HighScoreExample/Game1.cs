using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HighScoreExample
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        SpriteFont myFont;
        HighScore highScore;
        enum State { PrintHighScore, EnterHighScore};
        State currentState;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            highScore = new HighScore(10);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            myFont = Content.Load<SpriteFont>("myFont");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (currentState)
            {
                case State.EnterHighScore: // Skriv in oss i listan
                                           // Fortsätt så länge HighScore.EnterUpdate() returnerar true:
                    if (highScore.EnterUpdate(gameTime, 10))
                        currentState = State.PrintHighScore;
                    break;
                default: // Highscore-listan (tar emot en tangent)
                    KeyboardState keyboardState = Keyboard.GetState();
                    if (keyboardState.IsKeyDown(Keys.E))
                        currentState = State.EnterHighScore;
                    break;
            }

                    base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Använd spriteBatch för att rita ut saker på skärmen
            _spriteBatch.Begin();
            switch (currentState)
            {
                case State.EnterHighScore: // Skriv in oss i listan
                    highScore.EnterDraw(_spriteBatch, myFont);
                    break;
                default: // Rita ut highscore-listan
                    highScore.PrintDraw(_spriteBatch, myFont);
                    break;
            }
            _spriteBatch.End();


            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {

            base.UnloadContent();
            highScore.SaveToFile("highscore.txt");
        }
    }
}