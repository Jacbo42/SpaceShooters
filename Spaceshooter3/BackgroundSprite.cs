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
    /// Kontrollerar bakgrundens rörelse
    /// </summary>
    internal class BackgroundSprite : GameObjekt
    {
        //BackgroundSprite(), konstruktor för att skapa BackgroundSprite-objekt

        public BackgroundSprite(Texture2D texture, float X, float Y) : base(texture, X, Y)
        {

        }
        //Update(), ändrar positionen för ett BackgroundSprite-objekt.
        //Flyttar det längst upp ifall det har gåt ut i nedkant av skärmen

        public void Update(GameWindow window, int nrBackgroundsY)
        {
            vector.Y += 2f; //flytta bakgrunden
            //kontrollera om bakgrunder har åkt ut i nedkant
            if (vector.Y > window.ClientBounds.Height)
            {
                //Flytta bilden så att den hamnar ovanför alla andra bakgrundsbilder:
                vector.Y = vector.Y - nrBackgroundsY * texture.Height;
            }

        }
    }
}
