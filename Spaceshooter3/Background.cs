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
    /// Det är denna klass som överser bakgrunden till menyn och själva spelet.
    /// </summary>
    internal class Background
    {
        BackgroundSprite[,] background;
        int nrBackgroundsX, nrBackgroundsY;

        //Background(), konstruktor som skpar alla BackgroundSprite objekt
        //i en två dimensionell vektor
        public Background(Texture2D texture, GameWindow window)
        {
            //Hur många bilder ska vi ha på bredden?
            double tmpX = (double)window.ClientBounds.Width / texture.Width;
            //avrunda uppåt med Math.Ceiling()
            nrBackgroundsX = (int)Math.Ceiling(tmpX);
            //Hur många bilder ska vi ha på höjden?
            double tmpY = (double)window.ClientBounds.Height / texture.Height;
            //Avrunda uppåt med Math.Ceiling(), lägg till en extra
            nrBackgroundsY = (int)Math.Ceiling(tmpY) + 1;
            //Sätt storleken på vektorn
            background = new BackgroundSprite[nrBackgroundsX, nrBackgroundsY];

            //Fyll på vektorn med BackgroundSprite-objekt
            for(int i = 0; i < nrBackgroundsX; i++)
            {
                for(int j = 0; j < nrBackgroundsY; j++)
                {
                    int posX = i* texture.Width;
                    //Gör att den först hamnar ovanför skärmen
                    int posY = j* texture.Height - texture.Height;
                    background[i, j] = new BackgroundSprite(texture, posX, posY);
                }
            }


        }

        //Update(), uppdaterr positionen för samtliga BackgroundSprite-objekt
        public void Update(GameWindow window)
        {
            for (int i = 0; i < nrBackgroundsX; i++)
            {
                for (int j = 0; j < nrBackgroundsY; j++)
                {
                    background[i, j].Update(window, nrBackgroundsY);
                }
            }
        }

        //Draw(), ritar ut samtliga BackgroundSprite-objekt

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < nrBackgroundsX; i++)
            {
                for (int j = 0; j < nrBackgroundsY; j++)
                {
                    background[i, j].Draw(spriteBatch);
                }
            }
        }
        public void MenuBackground(SpriteBatch spriteBatch)
        {

        }
    }
}
