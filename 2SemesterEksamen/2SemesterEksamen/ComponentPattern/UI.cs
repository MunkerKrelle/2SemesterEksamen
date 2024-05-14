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
            weaponInfo = database.ReturnValues();
            spriteBatch.DrawString(font, weaponInfo[0], Vector2.Zero, Color.White);
        }
    }
}
