using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Spaceshooter3
{
    internal class Tripod : Enemy
    {
        //konstruktor

        public Tripod(Texture2D texture, float X, float Y) : base(texture, X, Y, 0f, 3f) { }

        //Update, uppdaterar fiendens position
        public override void Update(GameWindow window)
        {
            //flytta på fienden
            vector.Y += speed.Y;
            if (vector.Y > window.ClientBounds.Height)
            {
                isAlive = false;
            }
        }
    }
}

