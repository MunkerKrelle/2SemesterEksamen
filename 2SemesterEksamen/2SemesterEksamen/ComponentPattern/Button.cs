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
        private Vector2 buttonPosition;
        Texture2D buttonTexture;
        private float scale = 1f;
        Rectangle rectangleForButtons;
        Color colorCode = Color.White;
        Vector2 originSprite, originText;
        public bool active = true;
        string buttonText;
        Delegate actionFunction;
        protected Vector2 SpriteSize
        {
            get
            {
                return new Vector2(buttonTexture.Width * scale, buttonTexture.Height * scale);
            }
        }

        public Button(Vector2 buttonPosition, string buttonText, Delegate actionFunction, GameObject gameObject) : base(gameObject)
        {
            this.buttonPosition = buttonPosition;
            this.buttonText = buttonText;
            this.actionFunction = actionFunction;
        }

        public void Update()
        {
            MouseOnButton();
            MousePressed();
        }
        public void PositionUpdate()
        {
            minPosition.X = buttonPosition.X - (rectangleForButtons.Width / 2);
            minPosition.Y = buttonPosition.Y - (rectangleForButtons.Height / 2);
            maxPosition.X = buttonPosition.X + (rectangleForButtons.Width / 2);
            maxPosition.Y = buttonPosition.Y + (rectangleForButtons.Height / 2);
        }
        public void LoadContent(ContentManager content)
        {
            buttonTexture = content.Load<Texture2D>("button");

            rectangleForButtons = new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, buttonTexture.Width / 2, buttonTexture.Height / 2);

            PositionUpdate();
        }
        public void MouseOnButton()
        {

            if (GameWorld.mouseState.X > minPosition.X && GameWorld.mouseState.Y > minPosition.Y && GameWorld.mouseState.X < maxPosition.X && GameWorld.mouseState.Y < maxPosition.Y)
            {
                colorCode = Color.LightGray;
            }
            else
            {
                colorCode = Color.White;
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
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 fontLength = GameWorld.font.MeasureString(buttonText);

            originSprite = new Vector2(buttonTexture.Width / 2f, buttonTexture.Height / 2f);
            originText = new Vector2(fontLength.X / 2f, fontLength.Y / 2f);

            spriteBatch.Draw(buttonTexture, rectangleForButtons, null, colorCode, 0, originSprite, SpriteEffects.None, 0.98f);
            spriteBatch.DrawString(GameWorld.font, buttonText, buttonPosition, Color.Black, 0, originText, 1, SpriteEffects.None, 0.1f);

        }   
    }
}
