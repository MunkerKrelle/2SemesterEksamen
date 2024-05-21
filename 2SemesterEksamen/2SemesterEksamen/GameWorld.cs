using Algoritmer;
using BuilderPattern;
using CommandPattern;
using ComponentPattern;
using FactoryPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RepositoryPattern;
using StatePattern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace _2SemesterEksamen
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;

        private SpriteBatch _spriteBatch;

        private List<GameObject> gameObjects = new List<GameObject>();
        private List<GameObject> newGameObjects = new List<GameObject>();

        private List<GameObject> destroyedGameObjects = new List<GameObject>();
        public float DeltaTime { get; private set; }
        public GraphicsDeviceManager Graphics { get => _graphics; set => _graphics = value; }

        private static List<Button> buttons = new List<Button>();
        Button respawnButton;

        public static MouseState mouseState;
        public static MouseState newState;
        public static bool isPressed;
        
        private int cellCount = 11;
        private int cellSize = 100;
        private float timeElapsed;

        public static SpriteFont font;
        Vector2 originText;
        string fontText = "";
        Vector2 fontLength;

        private static GameWorld instance;
        private GameObject playerGo;
        private Vector2 playerStartPosition = new Vector2(800, 700); // Example start position

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public Dictionary<Point, Cell> Cells { get; private set; } = new Dictionary<Point, Cell>();

        public Dictionary<string, Texture2D> sprites { get; private set; } = new Dictionary<string, Texture2D>();

        protected override void Initialize()
        {
            IRepository repository = new Database();
            new Database(repository).RunLoop();


            Director director = new Director(new PlayerBuilder());
            Director director1 = new Director(new ArmsDealerBuilder());
            playerGo = director.Construct();
            GameObject armsDealerGo = director1.Construct();
            gameObjects.Add(playerGo);
            gameObjects.Add(armsDealerGo);

            Player player = playerGo.GetComponent<Player>() as Player;
            ArmsDealer armsDealer = armsDealerGo.GetComponent<ArmsDealer>() as ArmsDealer;

            buttons.Add(new Button(new Vector2(100, 200), "test", Exit));
            respawnButton = new Button(new Vector2(500, 300), "Respawn", (Action)RespawnPlayer);
            respawnButton.active = false; 

            GameObject database = new GameObject();
            database.AddComponent<UI>();
            gameObjects.Add(database);

            gameObjects.Add(EnemyFactory.Instance.Create());

            foreach (GameObject go in gameObjects)
            {
                go.Awake();
            }


            InputHandler.Instance.AddUpdateCommand(Keys.D, new MoveCommand(player, new Vector2(1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.A, new MoveCommand(player, new Vector2(-1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.W, new MoveCommand(player, new Vector2(0, -1)));
            InputHandler.Instance.AddUpdateCommand(Keys.S, new MoveCommand(player, new Vector2(0, 1)));

            InputHandler.Instance.AddUpdateCommand(Keys.P, new AttackCommand(player));

            gameObjects.Add(EnemyFactory.Instance.Create());
            //var treeSprite = GameWorld.Instance.sprites["Pixel"];
            //walls.Add(WallFactory.Instance.Create());
            //walls[i].Transform.Position = new Vector2(115 * i, 0);
            player.GameObject.Transform.CellMovement(new Vector2(1050), new Vector2(1050));
           // sprites.Add("cellGrid", Content.Load<Texture2D>("cellGrid"));
            //sprites.Add("1fwd", Content.Load<Texture2D>("1fwd"));
           // SetUpCells();

            _graphics.PreferredBackBufferWidth = cellCount * cellSize + 200;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = cellCount * cellSize + 1;   // set this value to the desired height of your window
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (GameObject go in gameObjects)
            {
                go.Start();
            }
            foreach (var button in buttons)
            {
                button.LoadContent(Content);
            }
            respawnButton.LoadContent(Content);
            font = Content.Load<SpriteFont>("text2");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeElapsed += DeltaTime;

            foreach (GameObject go in gameObjects)
            {
                go.Update(gameTime);
            }

            if (timeElapsed >= 0.3f)
            {
                InputHandler.Instance.Execute();
                timeElapsed = 0;
            }

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.C))
            {
                //Point tree1Position = new Point(-10, -10);
                //gameObjects[100].Transform.Position += new Vector2(tree1Position.X , tree1Position.Y);
                //Cells.Remove(new Point(5, 5));
                //gameObjects.Remove(gameObjects[55]);

                //SpriteRenderer sr = (SpriteRenderer)gameObjects[37].GetComponent<SpriteRenderer>();
                //sr.SetSprite("1fwd");
                Cells[new Point(1, 1)].Sprite = sprites["1fwd"];
                //Cell cell = Cells.Values.ElementAt(0);
                //gameObjects[22].Transform.Position = new Vector2(20, 20);
                Player player = gameObjects[0].GetComponent<Player>() as Player;
                player.GameObject.Transform.CellMovement(new Vector2(1100), new Vector2(300));

                //Cells.GetValueOrDefault(new Point(4, 4));

                //Cells.Values.ElementAt(5).Sprite = sprites["cellGrid"];

                //player.GameObject.Transform.PosOnCell = new Point(8, 8);
                //player.GameObject.Transform.Position = new Vector2(1000, 80);
                //Cells[new Point(5, 5)].Sprite = Instance.Content.Load<Texture2D>("1fwd");
                //Cells[new Point(5, 5)].Sprite = sprites["Pixel"];
                //sr.SetSprite("cellGrid");
                //Point test = new Point(5, 5);
                //gameObjects[100].
                //var test = gameObjects[55].GetComponent<SpriteRenderer>();
                //test.Sprite = sprites["Pixel"];
            }
                InputHandler.Instance.Execute();
                CheckCollision();

                mouseState = Mouse.GetState();
            
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                isPressed = true;
            }
            else
            {
                isPressed = false;
            }

            foreach (var button in buttons)
            {
                button.Update();
            }
            if (respawnButton.active)
            {
                respawnButton.Update();
            }

            if (keyState.IsKeyDown(Keys.V))
            {
                SpriteRenderer sr = (SpriteRenderer)gameObjects[37].GetComponent<SpriteRenderer>();
                sr.SetSprite("cellGrid");
                SpriteRenderer sr2 = (SpriteRenderer)gameObjects[38].GetComponent<SpriteRenderer>();
                sr2.SetSprite("1fwd");
                Player player = gameObjects[0].GetComponent<Player>() as Player;
                //player.GameObject.Transform.PosOnCell = new Point(8, 8);
                //player.GameObject.Transform.Position = new Vector2(1000, 80);
                player.GameObject.Transform.CellMovement(new Vector2(1200), new Vector2(500));
            }
            if (keyState.IsKeyDown(Keys.B))
            {
                SpriteRenderer sr = (SpriteRenderer)gameObjects[38].GetComponent<SpriteRenderer>();
                sr.SetSprite("cellGrid");
                SpriteRenderer sr2 = (SpriteRenderer)gameObjects[39].GetComponent<SpriteRenderer>();
                sr2.SetSprite("1fwd");
            }
            base.Update(gameTime);

            Cleanup();
        }

        private void SetUpCells() 
        {
            for (int y = 1; y < cellCount; y++)
            {
                for (int x = 1; x < cellCount; x++)
                {
                    //if (x != 8)
                    //{
                    Cells.Add(new Point(x, y), new Cell(new Point(x, y), cellSize, cellSize));
                    GameObject cellGrid = new GameObject();
                    SpriteRenderer sr = cellGrid.AddComponent<SpriteRenderer>();
                    gameObjects.Add(cellGrid);
                    sr.SetSprite("cellGrid");
                    Cells[new Point(x, y)].Sprite = sprites["cellGrid"];
                        //if (x == 1 && y == 1)
                        //{
                            
                        //}
                   
                        //Cells.Values.ElementAt(5).Sprite = sprites["cellGrid"];
                        //sr.Sprite = sprites["cellGrid"];
                    //Point meTest = new Point(x, y);
                    //Cells[meTest].Sprite = sprites["cellGrid"];


                    cellGrid.Transform.Scale = new Vector2(1, 1);
                    Point pos = new Point(x, y);
                    //Cells[new Point(x, y)].Sprite = Instance.Content.Load<Texture2D>("Pixel");
                    //Cells[pos].Sprite = sprites["Pixel"];
                    cellGrid.Transform.Position = new Vector2(pos.X * 100, pos.Y * 100);
                        //cellGrid.Transform.PosOnCell = new Point(x * 100, y * 100);
                        //SpriteRenderer sr1 = (SpriteRenderer)gameObjects[0].GetComponent<SpriteRenderer>();
                        //sr.GameObject.Transform.PosOnCell = new Point(x, y);
                    //}
                }
            }
        }

        private void Cleanup()
        {
            // Adding newly instantiated GameObjects
            for (int i = 0; i < newGameObjects.Count; i++)
            {
                gameObjects.Add(newGameObjects[i]);
                newGameObjects[i].Awake(); // Initializing new GameObjects
                newGameObjects[i].Start(); // Starting new GameObjects
            }

            // Removing destroyed GameObjects
            for (int i = 0; i < destroyedGameObjects.Count; i++)
            {
                gameObjects.Remove(destroyedGameObjects[i]);
            }
            destroyedGameObjects.Clear(); // Clearing destroyed GameObjects list
            newGameObjects.Clear(); // Clearing new GameObjects list
        }

        /// <summary>
        /// Adding GameObject to new GameObjects list
        /// </summary>
        /// <param name="go"></param>
        public void Instantiate(GameObject go)
        {
            newGameObjects.Add(go);
        }

        /// <summary>
        /// Adding GameObject to destroyed GameObjects list
        /// </summary>
        /// <param name="go"></param>
        public void Destroy(GameObject go)
        {
            destroyedGameObjects.Add(go);
        }
        void CheckCollision()
        {
            foreach (GameObject go1 in gameObjects)
            {
                foreach (GameObject go2 in gameObjects)
                {
                    if (go1 == go2)
                    {
                        continue;
                    }
                    Collider col1 = go1.GetComponent<Collider>() as Collider;
                    Collider col2 = go2.GetComponent<Collider>() as Collider;

                    if (col1 != null && col2 != null && col1.CollisionBox.Intersects(col2.CollisionBox))
                    {
                        foreach (Collider.RectangleData rects1 in col1.rectangles)
                        {
                            foreach (Collider.RectangleData rects2 in col2.rectangles)
                            {
                                if (rects1.Rectangle.Intersects(rects2.Rectangle))
                                {
                                    go1.OnCollisionEnter(col2);
                                    go2.OnCollisionEnter(col1);
                                }
                            }
                        }
                    }
                }
            }
        }
        public void ShowRespawnButton()
        {
            respawnButton.active = true;
        }

        private void RespawnPlayer()
        {
            playerGo = new GameObject();
            Player player = playerGo.AddComponent<Player>() as Player;
            player.Respawn(playerStartPosition);
            gameObjects.Add(playerGo);
            respawnButton.active = false; // Hide the respawn button
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (GameObject go in gameObjects)
            {
                go.Draw(_spriteBatch);
            }
            if (respawnButton.active)
            {
                respawnButton.Draw(_spriteBatch, gameTime);
            }
            buttons[0].Draw(_spriteBatch, gameTime);
            _spriteBatch.DrawString(font, $"{mouseState.X}", new Vector2(300, 300), Color.Black, 0, originText, 1f, SpriteEffects.None, 1f);
            // _spriteBatch.Draw(); //Draw background

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
