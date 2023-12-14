using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HighScoreExample
{
    internal class HSItem
    {
        // Variabler och egenskaper för dem:
        string name;
        int points;

        public string Name { get { return name; } set { name = value; } }

        public int Points { get { return points; } set { points = value; } }

        // =======================================================================
        // HSItem(), klassens konstruktor
        // =======================================================================
        public HSItem(string name, int points)
        {
            this.name = name;
            this.points = points;
        }
    }
}
