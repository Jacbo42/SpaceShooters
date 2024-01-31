using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaceshooter3
{
    /// <summary>
    /// Klassen som reglerar spelarens skott
    /// </summary>
    internal class Bullet : PhysicalObject
    {
        //Konstruktor

        public Bullet(Texture2D texture, float X, float Y) : base(texture, X, Y, 0, 3f)
        {
        }

        //Update(), uppdaterar skottets position och tar ort det om det åker utanför fönstret

        public void Update(Microsoft.Xna.Framework.GameWindow window)
        {
            vector.Y -= speed.Y;
            if (vector.Y < 0)
            {
                isAlive = false;
            }
        }




    }

    /// <summary>
    /// Samma klass som normala Bullet, fast den enda skillnaden är att denna klass har ett annat namn och fart på skotten
    /// </summary>
    internal class EnemyBullet : PhysicalObject
    {
        public EnemyBullet(Texture2D texture, float X, float Y) : base(texture, X, Y, 0, -3f)
        {
        }

        //Update(), uppdaterar skottets position och tar ort det om det åker utanför fönstret

        public void Update(Microsoft.Xna.Framework.GameWindow window)
        {
            vector.Y -= speed.Y;
            if (vector.Y < 0)
            {
                isAlive = false;
            }
        }
    }
}
