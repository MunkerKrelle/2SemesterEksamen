﻿using _2SemesterEksamen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ComponentPattern
{
    /// <summary>
    /// Giver mulighed for at GameObjecter kan kollidere 
    /// </summary>
    public class Collider : Component
    {
        public Lazy<List<RectangleData>> rectangles = new Lazy<List<RectangleData>>();
        private Texture2D texture;

        private SpriteRenderer spriteRenderer;

        public Collider(GameObject gameObject) : base(gameObject)
        {
        }

        private void UpdatePixelCollider()
        {
            if (rectangles.IsValueCreated)
            {
                for (int i = 0; i < rectangles.Value.Count; i++)
                {
                    rectangles.Value[i].UpdatePosition(GameObject, spriteRenderer.Sprite.Width, spriteRenderer.Sprite.Height);
                }
            }
        }

        /// <summary>
        /// Laver en pixel kollisionsboks, der læser hver enkelt pixel i GameObject spritet 
        /// </summary>
        /// <returns></returns>
        public List<RectangleData> CreateRectangles()
        {
            texture = GameWorld.Instance.Content.Load<Texture2D>("Pixel");
            List<Color[]> lines = new List<Color[]>();
            List<RectangleData> pixels = new List<RectangleData>();
            for (int y = 0; y < spriteRenderer.Sprite.Height; y++)
            {
                Color[] colors = new Color[spriteRenderer.Sprite.Width];
                spriteRenderer.Sprite.GetData(0, new Rectangle(0, y, spriteRenderer.Sprite.Width, 1), colors, 0, spriteRenderer.Sprite.Width);
                lines.Add(colors);
            }

            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x].A != 0)
                    {
                        if ((x == 0)
                            || (x == lines[y].Length)
                            || (x > 0 && lines[y][x - 1].A == 0)
                            || (x < lines[y].Length - 1 && lines[y][x + 1].A == 0)
                            || (y == 0)
                            || (y > 0 && lines[y - 1][x].A == 0)
                            || (y < lines.Count - 1 && lines[y + 1][x].A == 0))
                        {

                            RectangleData rd = new RectangleData(x, y);

                            pixels.Add(rd);
                        }
                    }
                }
            }
            return pixels;
        }


        /// <summary>
        /// Laver en kollisionsboks rundet GameObjecterne i forhold til deres sprite, så de kan blive kollideret med
        /// </summary>
        /// <returns></returns>
        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle
                    (
                        (int)(GameObject.Transform.Position.X - spriteRenderer.Sprite.Width * GameObject.Transform.Scale.X / 2),
                        (int)(GameObject.Transform.Position.Y - spriteRenderer.Sprite.Height * GameObject.Transform.Scale.Y / 2),
                        spriteRenderer.Sprite.Width * (int)GameObject.Transform.Scale.X,
                        spriteRenderer.Sprite.Height * (int)GameObject.Transform.Scale.Y
                    );
            }
        }

        public override void Update(GameTime gameTime)
        {
            UpdatePixelCollider();
        }
        public override void Awake()
        {
            IsEnabled = true;
        }

        public override void Start()
        {

            spriteRenderer = (SpriteRenderer)GameObject.GetComponent<SpriteRenderer>();
            texture = GameWorld.Instance.Content.Load<Texture2D>("Pixel");
            rectangles= new Lazy<List<RectangleData>>(()=> CreateRectangles());

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (rectangles.IsValueCreated)
            {
                foreach (RectangleData rd in rectangles.Value)
                {
                    DrawRectangle(rd.Rectangle, spriteBatch);
                }
            }
            DrawRectangle(CollisionBox, spriteBatch);
        }

        /// <summary>
        /// Tegner selve kollisonsboksen
        /// </summary>
        /// <param name="collisionBox">Kollisionsboksens info efter spriten</param>
        /// <param name="spriteBatch"></param>
        private void DrawRectangle(Rectangle collisionBox, SpriteBatch spriteBatch)
        {
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

            spriteBatch.Draw(texture, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        public class RectangleData
        {
            public Rectangle Rectangle { get; set; }

            public int X { get; set; }

            public int Y { get; set; }

            public RectangleData(int x, int y)
            {
                this.Rectangle = new Rectangle();
                this.X = x ;
                this.Y = y;
            }

            public void UpdatePosition(GameObject gameObject, int width, int height)
            {
                Rectangle = new Rectangle((int)gameObject.Transform.Position.X + X - width / 2, (int)gameObject.Transform.Position.Y + Y - height / 2, 1, 1);
            }
        }
    }
}



