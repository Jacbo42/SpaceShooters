using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spaceshooter3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Spaceshooter3
{

    internal class Player : PhysicalObject
    {
        List<Bullet> bullets;
        Texture2D bulletTexture;
        double timeSinceLastBullet = 0;
        int points = 0;
        int cash = 0;
        int starterlives = 3;

        private bool isInvulnerable;
        private float InvulnerabilityDuration = 3.0f;
        private float InvulnerableTimer;

        //konstruktor

        public Player(Texture2D texture, float X, float Y, float speedX, float speedY, Texture2D bulletTexture) : base(texture, X, Y, speedX, speedY)
        {
            bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;
        }

        //Update(), flyttar på spelaren

        //points

        public int Points
        {
            get { return points; } set {  points = value; }
        }
        public bool IsInvulnerable
        {
            get { return isInvulnerable; }
            set { isInvulnerable = value; }
        }
        public int Cash
        {
            get { return cash; }
            set { cash = value; }
        }
        public int Lives
        {
            get { return starterlives; }
            set { starterlives = value; }
        }
        public float invulnerabilityDuration
        {
            get { return invulnerabilityDuration; }
            set { invulnerabilityDuration = value; }
        }
        public float invulnerableTimer
        {
            get { return InvulnerableTimer; }
            set { InvulnerableTimer = value; }
        }



        public List<Bullet> Bullets {  get { return bullets; } }    

        public void Update(GameWindow window, GameTime gameTime)
        {
            //Läs in tangenttryckningar
            KeyboardState keyboardState = Keyboard.GetState();


            if (isInvulnerable)
            {
                InvulnerableTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if(InvulnerableTimer<=0)
                {
                    isInvulnerable = false;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                isAlive = false;
            }



            //Flytta rymdskeppet efter tangenttryckningar (om det inte är på väg ut från kanten)

            if (vector.X <= window.ClientBounds.Width - texture.Width && vector.X >= 0)
            {
                if (keyboardState.IsKeyDown(Keys.Right)) { vector.X += speed.X; }
                if (keyboardState.IsKeyDown(Keys.Left)) { vector.X -= speed.X; }
            }
            if (vector.Y <= window.ClientBounds.Height - texture.Height && vector.Y >= 0)
            {
                if (keyboardState.IsKeyDown(Keys.Down)) { vector.Y += speed.Y; }
                if (keyboardState.IsKeyDown(Keys.Up)) { vector.Y -= speed.Y; }
            }

            //Kontrollera ifall rymdskeppet har åkt ut från kanten, om det har det, så återståll dess position

            //Har det åkt till vänster
            if (vector.X < 0) vector.X = 0;
            // Har det åkt till håger
            if (vector.X > window.ClientBounds.Width - texture.Width) vector.X = window.ClientBounds.Width - texture.Width;
            // Har det åkt ut uptill
            if (vector.Y < 0) vector.Y = 0;
            // Har det åkt ut nedtill
            if (vector.Y > window.ClientBounds.Height - texture.Height) vector.Y = window.ClientBounds.Height - texture.Height;

            // Spelaren vill sluta
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                //Kontrollera om spelaren får skjuta
                if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + 200)
                {
                    //skapa skottet:
                    Bullet temp = new Bullet(bulletTexture, vector.X + texture.Width / 2, vector.Y);
                    bullets.Add(temp); //Lägg till skottet i listan

                    //Sätt timeSinceLastBullet till detta ögonblick
                    timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;

                }
            }

            foreach (Bullet b in bullets.ToList())
            {
                //Flytta på skottet:
                b.Update(window);
                //kontrollera så att skottet inte är "Dött"
                if (!b.IsAlive)
                {
                    bullets.Remove(b); // Ta bort skottet ur listan
                }
            }
        }

        //Draw(), ritar ut saker på skärmen

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, Color.White);
            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);
            }
        }

        //Reset(), återställer spelaren för ett nytt spel

        public void Reset(float X, float Y, float speedX, float speedY)
        {
            //återställ spelarens position och hastighet
            vector.X = X;
            vector.Y = Y;
            speed.X = speedX;
            speed.Y = speedY;
            //återställ ala skott:
            bullets.Clear();
            timeSinceLastBullet = 0;
            //återställer spelarens liv
            starterlives = 3;
            // återställ spelarens poäng:
            points = 0;
            // går så att spelaren lever igen:
            isAlive = true;
        }

       

        public void StartInvulnerability()
        {
            isInvulnerable = true;
            InvulnerableTimer = InvulnerabilityDuration;
        }
    }
}
