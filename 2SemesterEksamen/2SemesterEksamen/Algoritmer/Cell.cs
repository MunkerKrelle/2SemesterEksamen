using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _2SemesterEksamen;

namespace Algoritmer
{
    public class Cell
    {
        /// <summary>
        /// Farven på cellens sprite
        /// </summary>
        public Color spriteColor = Color.Red;

        /// <summary>
        /// Farven på cellens kanter
        /// </summary>
        private Color edgeColor = Color.Black;

        /// <summary>
        /// Texturen for cellens sprite
        /// </summary>
        public Texture2D Sprite { get; set; }

        /// <summary>
        /// Positionen for cellen
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Højden og bredden af cellen
        /// </summary>
        public int Height { get; set; }
        public int Width { get; set; }

        /// <summary>
        /// Rektangler til at repræsentere cellens kanter og baggrund
        /// </summary>
        private Rectangle topLine;
        private Rectangle bottomLine;
        private Rectangle rightLine;
        private Rectangle leftLine;
        private Rectangle background;

        /// <summary>
        /// G, H og F værdier for A* algoritmen
        /// </summary>
        public int G { get; set; }
        public int H { get; set; }
        public int F => G + H;

        /// <summary>
        /// Reference til forældrecellen i A* algoritmen
        /// </summary>
        public Cell Parent;

        /// <summary>
        /// Konstruktør der initialiserer cellens position, bredde og højde
        /// </summary>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Cell(Point position, int width, int height)
        {
            this.Position = position;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Metode til at indlæse grafisk indhold
        /// </summary>
        public void LoadContent()
        {
            //Sprite = GameWorld.Instance.sprites["Pixel"];

            //// Initialiserer rektanglerne til at repræsentere cellens kanter og baggrund
            //topLine = new Rectangle(Position.X * Width, Position.Y * Height, Width, 1);
            //bottomLine = new Rectangle(Position.X * Width, (Position.Y * Height) + Height, Width, 1);
            //rightLine = new Rectangle((Position.X * Width) + Width, Position.Y * Height, 1, Height);
            //leftLine = new Rectangle(Position.X * Width, Position.Y * Height, 1, Height);
            //background = new Rectangle(Position.X * Width, Position.Y * Height, Width, Height);
        }

        // Metode til at opdatere
        public void Update()
        {
            // Intet at opdatere i denne klasse
        }

        /// <summary>
        /// Metode til at tegne cellen og dens kanter samt information
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Sprite, background, spriteColor);

            //// Tegner cellens kanter
            //spriteBatch.Draw(GameWorld.Instance.sprites["Pixel"], topLine, edgeColor);
            //spriteBatch.Draw(GameWorld.Instance.sprites["Pixel"], bottomLine, edgeColor);
            //spriteBatch.Draw(GameWorld.Instance.sprites["Pixel"], rightLine, edgeColor);
            //spriteBatch.Draw(GameWorld.Instance.sprites["Pixel"], leftLine, edgeColor);

            // Tekst der viser cellens position, F, H og G værdier
            string cellString = $"{Position.X.ToString()},{Position.Y.ToString()}\n F:{F} \n h:{H}\n g: {G}";
            //if (GameWorld.debug)
            //    spriteBatch.DrawString(GameWorld.Instance.SpriteFont, cellString, new Vector2(topLine.X, topLine.Y), Color.Black);
        }

        /// <summary>
        /// Metode til at nulstille cellens tilstand
        /// </summary>
        public void Reset()
        {
            spriteColor = Color.White;
            G = 0;
            H = 0;
        }
    }
}
