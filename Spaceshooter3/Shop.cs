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
        //lista på menuItems
        //Första valet i listan är valt

        private List<ShopItem> shop;

        //currentHeigt används för att rita ut shopItems på olika höjd:


        // det state som representerar själva menyn
        int ShopState;

        //Shop(), konstruktor som skapar listan med ShopItem:s

        public Shop(int shopState)
        {
            shop = new List<ShopItem>();
            this.ShopState = shopState;
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



        }
    }
}
