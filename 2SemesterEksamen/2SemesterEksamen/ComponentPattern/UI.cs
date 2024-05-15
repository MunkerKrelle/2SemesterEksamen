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
            List<BestiaryInfo> bestiaryInfo = new List<BestiaryInfo>();
            bestiaryInfo = database.ShowBestiaryInfo();

            spriteBatch.DrawString(font, bestiaryInfo[0].ToString(), Vector2.Zero, Color.White);
            

                      //DRONE
            //        spriteBatch.DrawString(font, ("Name: " + bestiaryInfo[0] + "    " + "Health: " + bestiaryInfo[1] + "    "+ 
            //            "Damage: " + bestiaryInfo[2] + "    " + "Strengths: " + bestiaryInfo[3] + "    " + "Weaknesses: " + bestiaryInfo[4] 
            //            + "    " + "Scrap Dropped: " + bestiaryInfo[5] +
            //            "    " + "Defeated: " + bestiaryInfo[6]), new Vector2(0, 200), Color.White);

            //        //ANDROID
            //        spriteBatch.DrawString(font, ("Name: " + bestiaryInfo[7] + "    " + "Health: " + bestiaryInfo[8] + "    " +
            //            "Damage: " + bestiaryInfo[9] + "    " + "Strengths: " + bestiaryInfo[10] + "    " + "Weaknesses: " + bestiaryInfo[11]
            //            + "    " + "Scrap Dropped: " + bestiaryInfo[12] +
            //            "    " + "Defeated: " + bestiaryInfo[13]), new Vector2(0,250), Color.White);

            //        //SENTINEL
            //        spriteBatch.DrawString(font, ("Name: " + bestiaryInfo[14] + "    " + "Health: " + bestiaryInfo[15] + "    " +
            //"Damage: " + bestiaryInfo[16] + "    " + "Strengths: " + bestiaryInfo[17] + "    " + "Weaknesses: " + bestiaryInfo[18]
            //+ "    " + "Scrap Dropped: " + bestiaryInfo[19] +
            //"    " + "Defeated: " + bestiaryInfo[20]), new Vector2(0, 300), Color.White);

            //        //ENFORCER
            //        spriteBatch.DrawString(font, ("Name: " + bestiaryInfo[21] + "    " + "Health: " + bestiaryInfo[22] + "    " +
            //"Damage: " + bestiaryInfo[23] + "    " + "Strengths: " + bestiaryInfo[24] + "    " + "Weaknesses: " + bestiaryInfo[25]
            //+ "    " + "Scrap Dropped: " + bestiaryInfo[26] +
            //"    " + "Defeated: " + bestiaryInfo[27]), new Vector2(0, 350), Color.White);

            //        //CYBORG
            //        spriteBatch.DrawString(font, ("Name: " + bestiaryInfo[28] + "    " + "Health: " + bestiaryInfo[29] + "    " +
            //"Damage: " + bestiaryInfo[30] + "    " + "Strengths: " + bestiaryInfo[31] + "    " + "Weaknesses: " + bestiaryInfo[32]
            //+ "    " + "Scrap Dropped: " + bestiaryInfo[33] +
            //"    " + "Defeated: " + bestiaryInfo[34]), new Vector2(0, 400), Color.White);
        }
    }
}
