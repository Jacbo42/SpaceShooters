using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Spaceshooter3
{
    /// <summary>
    /// En class menat för att erhålla statistiska värden som spelaren får under spelets gång
    /// </summary>
    internal class StatMaster
    {

        /// <summary>
        /// Spelarens poäng mängd.
        /// </summary>
        int points = 0;
        /// <summary>
        /// Mängden fiender spelaren har dödat. Detta värde reglerar när fiender laddar in igen.
        /// </summary>
        /// <summary>
        /// Spelarens pengar, vilket bestämmer vad spelaren kan köpa
        /// </summary>
        int cash = 0;
        /// <summary>
        /// Mängden liv spelaren har
        /// </summary>
        /// <summary>
        /// Den "nivån" spelaren har nått. Egentligen är detta bara ett nummer som visar mängden gånger spelaren har dödat 10 eller fler fiender
        /// </summary>
        int level = 0;
        public int Points { get { return points; } set { points = value; } }
        public int Cash { get { return cash; } set { cash = value; } }
        public int Level { get { return level; } set { level = value; } }
        public StatMaster() { }

        public void Reset()
        {
            //återställ spelarens position och hastighet
            
            //återställ ala skott:
            //återställer spelarens liv
            // återställ spelarens poäng:
            points = 0;
            // återställer spelarens pengar:
            cash = 0;

            level = 0;
        }
    }
}
