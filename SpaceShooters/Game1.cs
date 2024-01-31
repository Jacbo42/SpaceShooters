using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooters
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
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

            

            ship_vector.X = 300;
            ship_vector.Y = 20;

            ship_speed.X = 2.5f;
            ship_speed.Y = 4.5F;

            ship_vector2.X = 300;
            ship_vector2.Y = 20;

            ship_speed2.X = 2.5f;
            ship_speed2.Y = 4.5F;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ship_texture = Content.Load<Texture2D>("ship");


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            ship_vector2.X += ship_speed2.X;

            if (ship_vector2.X > Window.ClientBounds.Width - ship_texture.Width || ship_vector2.X < 0)
            {
                ship_speed2.X *= -1; //Byter riktning på skeppet
            }

            ship_vector2.Y += ship_speed2.Y;

            if (ship_vector2.Y > Window.ClientBounds.Height - ship_texture.Height || ship_vector2.Y < 0)
            {
                ship_speed2.Y *= -1; //Byter riktning på skeppet
            }

            KeyboardState keyboardState = Keyboard.GetState();

            //if (keyboardState.IsKeyDown(Keys.Right)) { ship_vector.X += ship_speed.X; }
            //if (keyboardState.IsKeyDown(Keys.Left)) { ship_vector.X -= ship_speed.X; }
            //if (keyboardState.IsKeyDown(Keys.Up)) { ship_vector.Y -= ship_speed.Y; }
            //if (keyboardState.IsKeyDown(Keys.Down)) { ship_vector.Y += ship_speed.Y; }

            if (ship_vector.X <= Window.ClientBounds.Width - ship_texture.Width && ship_vector.X >= 0)
            {
                if (keyboardState.IsKeyDown(Keys.Right)) { ship_vector.X += ship_speed.X; }
                if (keyboardState.IsKeyDown(Keys.Left)) { ship_vector.X -= ship_speed.X; }
            }

            if (ship_vector.Y <= Window.ClientBounds.Height - ship_texture.Height && ship_vector.Y >= 0)
            {
                if (keyboardState.IsKeyDown(Keys.Up)) { ship_vector.Y -= ship_speed.Y; }
                if (keyboardState.IsKeyDown(Keys.Down)) { ship_vector.Y += ship_speed.Y; }
            }

            if (ship_vector.X < 0) ship_vector.X = 0;
            if (ship_vector.X > Window.ClientBounds.Width - ship_texture.Width) ship_vector.X = Window.ClientBounds.Width - ship_texture.Width;
            if (ship_vector.Y < 0) ship_vector.Y = 0;
            if (ship_vector.Y > Window.ClientBounds.Height - ship_texture.Height) ship_vector.Y = Window.ClientBounds.Height - ship_texture.Height;





            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _spriteBatch.Draw(ship_texture, ship_vector, Color.White);
            _spriteBatch.Draw(ship_texture, ship_vector2, Color.Red);
            _spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}