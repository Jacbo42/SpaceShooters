using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Content;

namespace Spaceshooter3
{
    internal class Shop
    {
        List<ShopItem> shop; //lista på menuItems
        int selected = 0; //Första valet i listan är valt
        PrintText printText;
        static Texture2D shopSprite;
        Player player;



        //currentHeigt används för att rita ut shopItems på olika höjd:

        float currentHeight = 0;

        // lastChange används för att "pausa" tangentuttryckningar, så att
        // det inte ska gå för fort att bläddra bland menyvalen:
        double lastChange = 0;
        // det state som representerar själva menyn
        int ShopState;

        //Menu(), konstruktor som skapar listan med MenuItem:s

        public Shop(int ShopState)
        {
            shop = new List<ShopItem>();
            this.ShopState = ShopState;
        }
        //AddItem(), lägger till ett menyval i listan

        public void AddItem(Texture2D itemTexture, int state)
        {
            //Sätt höjd på item
            float X = 0;
            float Y = 0;
            
            //skapa ett temporärt objekt och lägg det i listan
            ShopItem temp = new ShopItem(itemTexture, new Vector2(X, Y), state);

            shop.Add(temp);


        }

        //Update(), kollar om användaren tryckt någon tangent.
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
                    if (selected > shop.Count - 1)
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
                        selected = shop.Count - 1; //Det sista menyvalet

                    }
                }
                //ställ lastchange till exakt detta ögonblick:
                lastChange = gameTime.TotalGameTime.TotalMilliseconds;
            }

            //Välj ett menyval med ENTER
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                return shop[selected].State;
            }

            //om inget menyval har valts, så stannar vi kvar i menyn

            return ShopState;
        }

        //Draw(), ritar ut menyn

        public void Draw(SpriteBatch spriteBatch)
        {
           

            for (int i = 0; i < shop.Count; i++)
            {
                spriteBatch.Draw(shop[i].Texture, shop[i].Position, Color.White);
            }
           
            
            //printText.Print("Cash: " + player.Cash, spriteBatch, new Vector2(0, 15), Color.Blue);

        }
    }
}
