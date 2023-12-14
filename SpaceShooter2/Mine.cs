using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter2
{
    internal class Mine : Enemy
    {
        //mine konstrukto

        public Mine(Texture2D texture, float X, float Y) : base(texture, X, Y, 6f, 0.3f)
        {

        }

        public override void Update(GameWindow window)
        {
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
    }
}
