using _2SemesterEksamen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentPattern
{
    internal class Button : Component
    {
        private Vector2 minPosition;
        private Vector2 maxPosition;
        private SpriteRenderer sr;
        private Vector2 buttonPosition;
        private Rectangle rectangleForButtons;
        private Color colorCode = Color.White;
        private Vector2 originSprite, originText;
        public bool active = true;
        string buttonText;
        public Delegate actionFunction;

        public Button(GameObject gameObject, Vector2 buttonPosition, string buttonText, Delegate actionFunction) : base(gameObject)
        {
            this.buttonPosition = buttonPosition;
            this.buttonText = buttonText;
            this.actionFunction = actionFunction;
        }

        public override void Update(GameTime gameTime)
        {
            MouseOnButton();
            MousePressed();
        }
        public void PositionUpdate()
        {
            minPosition.X = GameObject.Transform.Position.X - sr.Sprite.Width / 2;
            minPosition.Y = GameObject.Transform.Position.Y - sr.Sprite.Height / 2;
            maxPosition.X = GameObject.Transform.Position.X + sr.Sprite.Width / 2;
            maxPosition.Y = GameObject.Transform.Position.Y + sr.Sprite.Height / 2;
        }
        public override void Awake()
        {
            sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            GameObject.Transform.Position = buttonPosition;
            PositionUpdate();
        }
        public void MouseOnButton()
        {
            if (GameWorld.mouseState.X > minPosition.X && GameWorld.mouseState.Y > minPosition.Y && GameWorld.mouseState.X < maxPosition.X && GameWorld.mouseState.Y < maxPosition.Y)
            {
                GameObject.Transform.Color = Color.LightGray;
            }
            else
            {
                GameObject.Transform.Color = Color.White;
            }
        }
        public void MousePressed()
        {
            if (GameWorld.isPressed == true)
            {
                if (GameWorld.mouseState.X > minPosition.X && GameWorld.mouseState.Y > minPosition.Y && GameWorld.mouseState.X < maxPosition.X && GameWorld.mouseState.Y < maxPosition.Y)
                {
                    colorCode = Color.Yellow;
                    actionFunction = () => { };
                }
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 fontLength = GameWorld.font.MeasureString(buttonText);

            originText = new Vector2(fontLength.X / 2f, fontLength.Y / 2f);

            spriteBatch.DrawString(GameWorld.font, buttonText, buttonPosition, Color.Black, 0, originText, 1, SpriteEffects.None, 1f);

            spriteBatch.DrawString(GameWorld.font, $"{minPosition}", new Vector2(500, 300), Color.Black, 0, originText, 1, SpriteEffects.None, 1f);
            spriteBatch.DrawString(GameWorld.font, $"{maxPosition}", new Vector2(500, 330), Color.Black, 0, originText, 1, SpriteEffects.None, 1f);

        }
    }
}
