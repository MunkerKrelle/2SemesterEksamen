using _2SemesterEksamen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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
        public delegate void ButtonFunction();
        public ButtonFunction buttonFunction;
        public Button(GameObject gameObject, Vector2 buttonPosition, string buttonText, ButtonFunction buttonFunction) : base(gameObject)
        {
            this.buttonPosition = buttonPosition;
            this.buttonText = buttonText;
            this.buttonFunction = buttonFunction;
        }

        public override void Update(GameTime gameTime)
        {
            MouseOnButton();
            MousePressed();
            PositionUpdate();
        }
        public void PositionUpdate()
        {
            minPosition.X = (GameObject.Transform.Position.X - sr.Sprite.Width / 2) * GameObject.Transform.Scale.X;
            minPosition.Y = (GameObject.Transform.Position.Y - sr.Sprite.Height / 2) * GameObject.Transform.Scale.Y;
            maxPosition.X = (GameObject.Transform.Position.X + sr.Sprite.Width / 2) *GameObject.Transform.Scale.X;
            maxPosition.Y = (GameObject.Transform.Position.Y + sr.Sprite.Height / 2) * GameObject.Transform.Scale.Y;
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
                    GameObject.Transform.Color = Color.Yellow;
                    buttonFunction = () => { };
                }
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 fontLength = GameWorld.font.MeasureString(buttonText);

            originText = new Vector2(fontLength.X / 2f, fontLength.Y / 2f);

            spriteBatch.DrawString(GameWorld.font, buttonText, buttonPosition, Color.Black, 0, originText, 1, SpriteEffects.None, 1f);

            spriteBatch.DrawString(GameWorld.font, $"{minPosition}", new Vector2(buttonPosition.X*2, buttonPosition.Y + 100), Color.Black, 0, originText, 1, SpriteEffects.None, 1f);
            spriteBatch.DrawString(GameWorld.font, $"{maxPosition}", new Vector2(buttonPosition.X*2, buttonPosition.Y + 120), Color.Black, 0, originText, 1, SpriteEffects.None, 1f);

        }
        
    }
}
