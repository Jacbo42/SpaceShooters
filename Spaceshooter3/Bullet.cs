using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaceshooter3
{
    internal class Bullet : PhysicalObject
    {
        //Konstruktor

        public Bullet(Texture2D texture, float X, float Y) : base(texture, X, Y, 0, 3f)
        {
        }

        //Update(), uppdaterar skottets position och tar ort det om det åker utanför fönstret

        public void Update()
        {
            vector.Y -= speed.Y;
            if (vector.Y < 0)
            {
                isAlive = false;
            }
        }




    }

    internal class EnemyBullet : PhysicalObject
    {
        public EnemyBullet(Texture2D texture, float X, float Y) : base(texture, X, Y, 0, -3f)
        {
        }

        //Update(), uppdaterar skottets position och tar ort det om det åker utanför fönstret

        public void Update()
        {
            vector.Y -= speed.Y;
            if (vector.Y < 0)
            {
                isAlive = false;
            }
        }
    }
}
