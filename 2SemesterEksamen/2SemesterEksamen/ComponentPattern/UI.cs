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
        private NpgsqlDataSource datasource;
        private UserRegistrationWithPattern database = new UserRegistrationWithPattern();

        public UI(GameObject gameObject) : base(gameObject)
        {
        }

        public override void Awake()
        {
            base.Awake();
            font = GameWorld.Instance.Content.Load<SpriteFont>("text");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var bestiaryInfo = database.ShowBestiaryInfo();
            spriteBatch.DrawString(font, ("Name: " + bestiaryInfo[0] + "    " + "Health: " + bestiaryInfo[1] + "    "+ 
                "Damage: " + bestiaryInfo[2] + "    " + "Strengths: " + bestiaryInfo[3] + "    " + "Weaknesses: " + bestiaryInfo[4] 
                + "    " + "Scrap Dropped: " + bestiaryInfo[5] +
                "    " + "Defeated: " + bestiaryInfo[6]), Vector2.Zero, Color.White);
        }
    }
}
