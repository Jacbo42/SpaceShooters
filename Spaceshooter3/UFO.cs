using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Spaceshooter3
{
    internal class UFO : Enemy
    {
        List<Bullet> bullets;
        Texture2D bulletTexture;
        double timeSinceLastBullet = 0;
        Random random = new Random();
        public UFO(Texture2D texture, float X, float Y, Texture2D bulletTexture) : base(texture, X, Y, 2f, 0.1f)
        {
            bullets = new List<Bullet>();
            this.bulletTexture = bulletTexture;
        }
        public List<Bullet> Bullets { get { return bullets; } }


        //Update, uppdaterar fiendens position
        public override void Update(GameWindow window)
        {

            foreach (Bullet bullet in bullets)
            {
                bullet.Update();
            }
            if (random.Next(120) == 0)
            {
                ShootBullet();
            }
            //Flytta på fienden
            vector.X += speed.X;


            //kontrollera så dne inte pker utanför fönstret på sidorna
            if (vector.X > window.ClientBounds.Width - texture.Width || vector.X < 0)
            {
                speed.X *= -1;
            }
            vector.Y += speed.Y;
            //Gör fienden inaktiv om den åker där nere
            if (vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            

            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }

        private void ShootBullet()
        {
            Bullet bullet = new Bullet(bulletTexture, vector.X + texture.Width / 2, vector.Y);
            bullets.Add(bullet);
        }
    }
}
