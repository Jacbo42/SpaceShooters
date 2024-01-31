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
    /// En klass vars uppgift är att skriva ut värden
    /// </summary>
    internal class PrintText
    {
        SpriteFont font;
        //konstruktor
        public PrintText(SpriteFont font)
        {
            this.font = font;
        }

        // Print(), skriv ut texten

        public void Print(string text, SpriteBatch spriteBatch, Vector2 pos, Color color)
        {
            spriteBatch.DrawString(font, text, pos, Color.White);
        }
    }
}
