using _2SemesterEksamen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Npgsql;
using RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ComponentPattern
{
    internal class UI : Component
    {
        public SpriteFont font;
        private Database database = new Database();
        List<BestiaryInfo> bestiaryInfo = new List<BestiaryInfo>();

        public UI(GameObject gameObject) : base(gameObject)
        {
        }

        public override void Awake()
        {
            base.Awake();
            font = GameWorld.Instance.Content.Load<SpriteFont>("text2");
            bestiaryInfo = database.ShowBestiaryInfo();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            int offset = 0;

            foreach (BestiaryInfo value in bestiaryInfo)
            {
                offset += 50;

                spriteBatch.DrawString(font, ("Name: " + value.name + "    " + "Health: " + value.health + "    " +
                "Damage: " + value.damage + "    " + "Strengths: " + value.strengths + "    " + "Weaknesses: " + value.weaknesses
                + "    " + "Scrap Dropped: " + value.scrap_dropped +
                "    " + "Defeated: " + value.defeated), new Vector2(0, 500 + offset), Color.White);

            }
        }
    }
}
