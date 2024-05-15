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

        public void CellMovement(Vector2 a, Vector2 b) 
        {
            //PosOnCell.ToVector2();
            
            //a.ToPoint();
            //b.ToPoint();

            Point PTPosition = new Point((int)a.X, (int)b.Y);
            Position = new Vector2(PTPosition.X,PTPosition.Y);
            //maybe add cell moving here?

            //Position = new Vector2(a.ToPoint(), b.ToPoint());
            //Position = new Vector2(1100,300);
            //Position.X = a;
            //PosOnCell = new Point(a, b).ToVector2();
        }

        public void CellMovement2(Vector2 a)
        {
            //PosOnCell.ToVector2();

            //a.ToPoint();
            //b.ToPoint();

            Point PTPosition = new Point((int)a.X, (int)a.Y);
            Position += new Vector2(PTPosition.X, PTPosition.Y);
            //Position += new Vector2(PTPosition.X * a, PTPosition.Y * a);

            //GameWorld.Instance.Cells[PTPosition].Position = PTPosition;
            //GameWorld.Instance.Cells[PTPosition].Sprite = ;
            //maybe add cell moving here?

            //Position = new Vector2(a.ToPoint(), b.ToPoint());
            //Position = new Vector2(1100,300);
            //Position.X = a;
            //PosOnCell = new Point(a, b).ToVector2();
        }
    }
}
