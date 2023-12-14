using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaceshooter5
{
    //Menu, används för att skapa en meny, lägga till menyval i menyn samt
    //att ta emot tangenttryckninar för olika menyval och att rita ut menyn
    internal class Menu //Ska man ärva MenuItem????
    {
        List<MenuItem> menu; //lista på menuItems
        int selected = 0; //Första valet i listan är valt

        //currentHeigt används för att rita ut menyItems på olika höjd:

        float currentHeight = 0;

        // lastCahnge används för att "pausa" tangentuttryckningar, så att
        // det inte ska gå för fort att bläddra bland menyvalen:
        double lastChange = 0;
        // det state som representerar själva menyn
        int defaultMenuState;

        //Menu(), konstruktor som skapar listan med MenuItem:s

        public Menu(int defaultMenuState)
        {
            menu = new List<MenuItem>();
            this.defaultMenuState = defaultMenuState;
        }
        //AddItem(), lägger till ett menyval i listan

        public void AddItem(Texture2D itemTexture, int state)
        {
            //Sätt höjd på item
            float X = 0;
            float Y = 0 + currentHeight;

            // Ändra currentHeight efter detta höjd + 20 pixlar för lite extra mellanrum

            currentHeight += itemTexture.Height + 20;
            //skapa ett temporärt objekt och lägg det i listan
            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), state);

            menu.Add(temp);


        }

        //Update(), kolar om användaren tryckt någon tangent.
        //antingen kan pil-tangenterna användas för att välja en viss MenuItem
        //(utan att gå in i just det alet) eller så kan ENTER användas för att gå
        //in i den valda MenuItem:en

        public int Update(GameTime gameTime)
        {
            //Läs in tangenttryckningar
            KeyboardState keyboardState = Keyboard.GetState();

            //Byte mellan olika menyval. Först måste vi dock kontrollera så att användaren
            //inte precis nyligen bytte menyval. Vi vill ju inte att det ska ändras 30
            //eller 60 gånger per sekund. Därför pausar vi i 130 millisekunder.

            if (lastChange + 130 < gameTime.TotalGameTime.TotalMilliseconds)
            {
                // Gå ett steg ned i menyn
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    selected++;
                    //Om vi har gått utanför de möjliga valen, så vill vi att det första menyvalet ska väljas
                    if (selected > menu.Count - 1)
                    {
                        selected = 0;
                    }

                }
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    selected--;
                    //Om vi har gått utanför de möjliga valen (alltså negativa siffrorna),
                    //så vill vi att det sista & menyvalet ska väljas:
                    if (selected < 0)
                    {
                        selected = menu.Count - 1; //Det sista menyvalet
                        
                    }
                }
                //ställ lastchange till exakt detta ögonblick:
                lastChange = gameTime.TotalGameTime.TotalMilliseconds;
            }

            //Välj ett menyval med ENTER
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                return menu[selected].State; 
            }

            //om inget menyval har valts, så stannar vi kvar i menyn

            return defaultMenuState;
        }

        //Draw(), ritar ut menyn

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < menu.Count; i++)
            {
                // Om vi har ett aktivt menyval ritar vi ut det med en speciell färgtoning

                if (i == selected)
                {
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.RosyBrown);
                }

                else //anars ingen färgtoning alls:
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.White);
            }
        }



    }
}
