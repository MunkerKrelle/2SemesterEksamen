using _2SemesterEksamen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RepositoryPattern;
using System.Collections.Generic;

namespace ComponentPattern
{
    /// <summary>
    /// Data fra Beastiary databasen, som kan skrives ud
    /// </summary>
    public class UI
    {
        public SpriteFont font;
        private Texture2D sprite;
        private Database database = new Database();
        List<BestiaryInfo> bestiaryInfo = new List<BestiaryInfo>();
        private Vector2 scale;
        private List<Texture2D> sprites;

        public UI()
        {
        }

        public void Awake()
        {
            font = GameWorld.Instance.Content.Load<SpriteFont>("text2");
            bestiaryInfo = database.ShowBestiaryInfo();
            sprite = GameWorld.Instance.Content.Load<Texture2D>("backdrop");
            scale = new Vector2(3f, 3f);
            sprites = new List<Texture2D>();
        }

        public void Start()
        {
            sprites.Add(GameWorld.Instance.Content.Load<Texture2D>("drone"));
            sprites.Add(GameWorld.Instance.Content.Load<Texture2D>("robot"));
            sprites.Add(GameWorld.Instance.Content.Load<Texture2D>("robo-spider"));
            sprites.Add(GameWorld.Instance.Content.Load<Texture2D>("skull-robot"));
            sprites.Add(GameWorld.Instance.Content.Load<Texture2D>("cyborg"));
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            int offset = 0;

            spriteBatch.Draw(sprite, new Vector2(0, 700), null, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, .9f);

            foreach (BestiaryInfo value in bestiaryInfo)
            {
                offset += 70;

                spriteBatch.DrawString(font, ("Name: " + value.name + "    " + "Health: " + value.health + "    " +
                "Damage: " + value.damage + "    " + "Strengths: " + value.strengths + "    " + "Weaknesses: " + value.weaknesses
                + "    " + "Scrap Dropped: " + value.scrap_dropped +
                "    " + "Defeated: " + value.defeated), new Vector2(120, 680 + offset), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
            }

            spriteBatch.Draw(sprites[0], new Vector2(10, 730), null, Color.White, 0, new Vector2(0, 0), 0.8f, SpriteEffects.None, 1f);
            spriteBatch.Draw(sprites[1], new Vector2(30, 810), null, Color.White, 0, new Vector2(0, 0), 0.6f, SpriteEffects.None, 1f);
            spriteBatch.Draw(sprites[2], new Vector2(15, 850), null, Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 1f);
            spriteBatch.Draw(sprites[3], new Vector2(20, 930), null, Color.White, 0, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);
            spriteBatch.Draw(sprites[4], new Vector2(20, 1000), null, Color.White, 0, new Vector2(0, 0), 1.2f, SpriteEffects.None, 1f);



        }
    }
}
