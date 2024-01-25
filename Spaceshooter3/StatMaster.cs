using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaceshooter3
{
    internal class StatMaster
    {
        int points = 0;
        int kills = 0;
        int cash = 0;
        int starterlives = 3;
        int level = 0;
        public int Points { get { return points; } set { points = value; } }
        public int Kills { get { return kills; } set { kills = value; } }
        public int Cash { get { return cash; } set { cash = value; } }
        public int Level { get { return level; } set { level = value; } }
        public int Starterlives { get { return starterlives; } set { starterlives = value; } }
        public StatMaster() { }
    }
}
