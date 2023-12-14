using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaceshooter3
{

    //MenuItem, container-klass för ett menyval

    internal class MenuItem
    {
        Texture2D texture; //Bilden för menyvalet
        Vector2 position;  //Positionen för menyvalet
        int currentState;  //Menyvalets state

        //MenuItem(), konstruktor som sätter värden för de olika menyvalen

        public MenuItem(Texture2D texture, Vector2 position, int currentState)
        {
            this.position = position;
            this.texture = texture;
            this.currentState = currentState;
        }

        //(Get-)egenskaper för MenuItem

        public Texture2D Texture { get { return texture; } }
        public Vector2 Position { get { return position; } }
        public int State { get { return currentState; } }



    }
}
