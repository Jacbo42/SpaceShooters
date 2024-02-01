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
    /// <summary>
    /// En fiende som kan skjuta
    /// </summary>
    internal class UFO : Enemy
    {
        List<EnemyBullet> enemybullets;
        Texture2D bulletTexture;
        Random random = new Random();
        public UFO(Texture2D texture, float X, float Y, Texture2D bulletTexture) : base(texture, X, Y, 2f, 0.1f)
        {
            enemybullets = new List<EnemyBullet>();
            this.bulletTexture = bulletTexture;
        }
        public List<EnemyBullet> EnemyBullets { get { return enemybullets; } }


        //Update, uppdaterar fiendens position
        public override void Update(GameWindow window)
        {

            foreach (EnemyBullet b in enemybullets.ToList())
            {
                //Flytta på skottet:
                b.Update(window);
                //kontrollera så att skottet inte är "Dött"
                if (b.IsAlive == false) 
                {
                    enemybullets.Remove(b); // Ta bort skottet ur listan
                }
            }

            if (random.Next(240) == 0)
            {
                ShootBullet(window);
            }
            //Flytta på fienden
            vector.X += speed.X;


            //kontrollera så den inte pekar utanför fönstret på sidorna
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
            
            //Ritar in skottet när metoden anropas
            foreach (EnemyBullet bullet in enemybullets)
            {
                bullet.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
        //Metoden som "skjuter" skottet
        private void ShootBullet(GameWindow window)
        {

            EnemyBullet bullet = new EnemyBullet(bulletTexture, vector.X + texture.Width / 2, vector.Y);
            enemybullets.Add(bullet);


            foreach (EnemyBullet b in enemybullets.ToList())
            {
                //Flytta på skottet:
                b.Update(window);
                //kontrollera så att skottet inte är "Dött"
                if (b.IsAlive == false)
                {
                    EnemyBullets.Remove(b); // Ta bort skottet ur listan
                }
            }


        }
    }
}
