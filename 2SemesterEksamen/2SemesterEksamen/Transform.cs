using Microsoft.Xna.Framework;
using SharpDX.Direct2D1;
using System.Windows.Forms;

namespace _2SemesterEksamen
{
    public class Transform
    {
        public Vector2 Position { get; set; }
        public Point PosOnCell { get; set; }
        public float Rotation { get; set; } = 0f;
        public Vector2 Scale { get; set; } = new Vector2(1, 1);
        public Color Color { get; set; } = Color.White;
        public float Layer {  get; set; }

        public void Transformer(Vector2 Position, float Rotaion, Vector2 Scale, Color Color, float Layer)
        {
            this.Position = Position;
            this.Rotation = Rotaion;
            this.Scale = Scale;
            this.Color = Color;
            this.Layer = Layer;
        }
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
