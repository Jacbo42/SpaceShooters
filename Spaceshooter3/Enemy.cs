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
    /// klass för att implementera fiender
    /// </summary>
    internal abstract class Enemy : PhysicalObject
    {

        public Enemy(Texture2D texture, float X, float Y, float speedX, float speedY) : base(texture, X, Y, speedX, speedY)
        {

        }

        //Update(), uppdaterar fiendens position

        public abstract void Update(GameWindow window);
        
  
    }
}
