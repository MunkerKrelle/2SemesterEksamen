using _2SemesterEksamen;
using ComponentPattern;
using Microsoft.Xna.Framework.Graphics;
using RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    internal class ShowBestiaryCommand : ICommand
    {
        private UI ui;
        private SpriteFont font;
        private Database database = new Database();
        private SpriteBatch spriteBatch;
        List<BestiaryInfo> bestiaryInfo = new List<BestiaryInfo>();

        public ShowBestiaryCommand(UI ui)
        {
            this.ui = ui;

        }
        public void Execute()
        {
            ui.Draw(spriteBatch);
        }

        public void Undo()
        {
        }
    }
}
