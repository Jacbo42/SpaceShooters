using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaceshooter3
{
    /// <summary>
    /// Klass som reglerar själva objekten i spelet, genom exempelvis texture och vector
    /// </summary>
    internal class GameObjekt
    {
        protected Texture2D texture; // Rymdskeppets textur
        protected Vector2 vector; // Rymdskeppets kordninater

        //konstruktor

        public GameObjekt(Texture2D texture, float X, float Y)
        {
            this.texture = texture;
            this.vector.X = X;
            this.vector.Y = Y;
        }

        // Draw() ritar ut saker på skärmen
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, Color.White);

        }

        //Egeskaper fär GameObjekt

        public float X { get { return vector.X; } }
        public float Y { get { return vector.Y; } }

        public float Width { get { return texture.Width; } }
        public float Height { get { return texture.Height; } }





    }
}
