using Algoritmer;
using ComponentPattern;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamen
{
    public class Transform
    {
        public Vector2 Position { get; set; }
        public Point PosOnCell { get; set; }
        public float Rotation { get; set; } = 0f;
        public Vector2 Scale { get; set; } = new Vector2(1, 1);
        public void Translate(Vector2 translation)
        {
            if (!float.IsNaN(translation.X) && !float.IsNaN(translation.Y))
            {
                Position += translation;
            }
        }

        public Point VectorToPointConverter(Vector2 a) 
        {
            Point PTPosition = new Point((int)a.X / 100, (int)a.Y / 100);
            return PTPosition;
        }

        public void PlayerPointMove(Vector2 a)
        {
            Point PTPosition = new Point((int)a.X, (int)a.Y);
            Position += new Vector2(PTPosition.X, PTPosition.Y);
        }
    }
}
