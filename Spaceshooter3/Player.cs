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
        //Skott
        List<Bullet> bullets;
        //Skott textur
        Texture2D bulletTexture;
        //StatMaster
        StatMaster statMaster;
        double timeSinceLastBullet = 0;
        double timeBetweenBullets = 400; // Initial value
        int upgradelimit = 0;



        //Variabler som reglerar ödodlighet
        private bool isInvulnerable;
        private float InvulnerabilityDuration = 3.0f;
        private float InvulnerableTimer;

       

        //konstruktor

        public Player(Texture2D texture, float X, float Y, float speedX, float speedY, Texture2D bulletTexture) : base(texture, X, Y, speedX, speedY)
        {
            bullets = new List<Bullet>();
            statMaster = new StatMaster();
            this.bulletTexture = bulletTexture;
        }


        //Egenskaper
        public double TimeBetweenBullets
        {
            get { return timeBetweenBullets; }
            set { timeBetweenBullets = value; }
        }

        public int UpgradeLimit
        {
            get { return upgradelimit; } set {  upgradelimit = value; }
        }

        public bool IsInvulnerable
        {
            get { return isInvulnerable; }
            set { isInvulnerable = value; }
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
        public List<Bullet> Bullets { get { return bullets; } }
        //Update(), flyttar på spelaren

        public void Update(GameWindow window, GameTime gameTime)
        {
            //Läs in tangenttryckningar
            KeyboardState keyboardState = Keyboard.GetState();


            if (isInvulnerable)
            {
                InvulnerableTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (InvulnerableTimer <= 0)
                {
                    isInvulnerable = false;
                    InvulnerableTimer = 0;
                }
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
            // Har det åkt till höger
            if (vector.X > window.ClientBounds.Width - texture.Width) vector.X = window.ClientBounds.Width - texture.Width;
            // Har det åkt ut uptill
            if (vector.Y < 0) vector.Y = 0;
            // Har det åkt ut nedtill
            if (vector.Y > window.ClientBounds.Height - texture.Height) vector.Y = window.ClientBounds.Height - texture.Height;

            // Spelaren vill skjuta
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                //Kontrollera om spelaren får skjuta
                if (gameTime.TotalGameTime.TotalMilliseconds > timeSinceLastBullet + timeBetweenBullets)
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
            timeBetweenBullets = 400;
            timeSinceLastBullet = 0;
            //återställer spelarens liv
            statMaster.Starterlives = 3;
            // återställ spelarens poäng:
            statMaster.Points = 0;
            // återställer spelarens pengar:
            statMaster.Cash = 0;
            //Återställ kills, så att det inte blir konstigt när spelaren kör igång igen och en ny level kommer nästan omedelbart
            statMaster.Kills = 0;
            // går så att spelaren lever igen:
            isAlive = true;

        }

        //Påbörjar odödlighet

        public void StartInvulnerability()
        {
            isInvulnerable = true;
            InvulnerableTimer = InvulnerabilityDuration;
            
        }

        
        //Gör så att spelaren förlorar liv
        public void LoseLife(GameWindow window, GameTime gameTime, Player player, Enemy e)
        {

            if (e.CheckCollision(player))
            {
                if (!IsInvulnerable)
                {
                    statMaster.Starterlives--;
                    if (statMaster.Starterlives <= 0)
                    {
                        IsAlive = false;
                    }
                    else if (IsAlive)
                    {
                        StartInvulnerability();
                    }

                }

                if (IsInvulnerable)
                {
                    if (invulnerableTimer <= 0)
                    {
                        IsInvulnerable = false;
                    }
                }
                //timer ser dålig ut


            }

        }
        //Gör så att spelaren förlorar liv, fast med enemybullet istället för fiender
        public void LoseLifeBullet(GameWindow window, GameTime gameTime, Player player, EnemyBullet e)
        {
            if (e.CheckCollision(player))
            {
                if (!IsInvulnerable)
                {
                    statMaster.Starterlives--;
                    if (statMaster.Starterlives <= 0)
                    {
                        IsAlive = false;
                    }
                    else if (IsAlive)
                    {
                        StartInvulnerability();
                    }

                }

                if (IsInvulnerable)
                {
                    if (invulnerableTimer <= 0)
                    {
                        IsInvulnerable = false;
                    }
                }
                //timer ser dålig ut


            }

        }

    }
}

