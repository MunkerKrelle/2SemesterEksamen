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
        private Database database = new Database();
        List<BestiaryInfo> bestiaryInfo = new List<BestiaryInfo>();

        public UI()
        {
        }

        public  void Awake()
        {
            font = GameWorld.Instance.Content.Load<SpriteFont>("text2");
            bestiaryInfo = database.ShowBestiaryInfo();
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            int offset = 0;

            foreach (BestiaryInfo value in bestiaryInfo)
            {
                offset += 50;

                spriteBatch.DrawString(font, ("Name: " + value.name + "    " + "Health: " + value.health + "    " +
                "Damage: " + value.damage + "    " + "Strengths: " + value.strengths + "    " + "Weaknesses: " + value.weaknesses
                + "    " + "Scrap Dropped: " + value.scrap_dropped +
                "    " + "Defeated: " + value.defeated), new Vector2(0, 500 + offset), Color.Black);

            }
        }
    }
}
