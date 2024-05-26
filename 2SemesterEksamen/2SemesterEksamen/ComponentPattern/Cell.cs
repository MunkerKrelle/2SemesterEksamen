using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _2SemesterEksamen;
using System.Runtime.CompilerServices;

namespace ComponentPattern
{
    public class Cell : Component
    {
        /// <summary>
        /// Positionen for cellen
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// G, H og F værdier for A* algoritmen
        /// </summary>
        public int G { get; set; }
        public int H { get; set; }
        public int F => G + H;

        /// <summary>
        /// Reference til forældrecellen i A* algoritmen
        /// </summary>
        public Cell Parent;

        /// <summary>
        /// Konstruktør der initialiserer cellens position, bredde og højde
        /// </summary>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Cell(GameObject go, Point position) : base(go)
        {
            Position = position;
        }
    }
}
