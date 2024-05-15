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
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        private int cellCount = 11;
        private int cellSize = 100;

        private static GameWorld instance;

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
            IRepository repository = new PostgresRepository();
            new UserRegistrationWithPattern(repository).RunLoop();

            
            Director director = new Director(new PlayerBuilder());
            Director director1 = new Director(new ArmsDealerBuilder());
            GameObject playerGo = director.Construct();
            GameObject armsDealerGo = director1.Construct();
            gameObjects.Add(playerGo);
            gameObjects.Add(armsDealerGo);

            Player player = playerGo.GetComponent<Player>() as Player;
           
            ArmsDealer armsDealer = armsDealerGo.GetComponent<ArmsDealer>() as ArmsDealer;

            gameObjects.Add(EnemyFactory.Instance.Create());

            foreach (GameObject go in gameObjects)
            {
                go.Awake();
            }

            //InputHandler.Instance.AddUpdateCommand(Keys.D, new MoveCommand(player, new Vector2(1, 0)));
            //InputHandler.Instance.AddUpdateCommand(Keys.A, new MoveCommand(player, new Vector2(-1, 0)));
            //InputHandler.Instance.AddUpdateCommand(Keys.W, new MoveCommand(player, new Vector2(0, -1)));
            //InputHandler.Instance.AddUpdateCommand(Keys.S, new MoveCommand(player, new Vector2(0, 1)));

            InputHandler.Instance.AddUpdateCommand(Keys.D, new MoveCommand(player, new Vector2(1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.A, new MoveCommand(player, new Vector2(-1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.W, new MoveCommand(player, new Vector2(0, -1)));
            InputHandler.Instance.AddUpdateCommand(Keys.S, new MoveCommand(player, new Vector2(0, 1)));

            gameObjects.Add(EnemyFactory.Instance.Create());
            //var treeSprite = GameWorld.Instance.sprites["Pixel"];
            //walls.Add(WallFactory.Instance.Create());
            //walls[i].Transform.Position = new Vector2(115 * i, 0);
            player.GameObject.Transform.CellMovement(new Vector2(1050), new Vector2(1050));
            SetUpCells();

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            foreach (GameObject go in gameObjects)
            {
                go.Update(gameTime);
            }
            InputHandler.Instance.Execute();
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.C))
            {
                //Point tree1Position = new Point(-10, -10);
                //gameObjects[100].Transform.Position += new Vector2(tree1Position.X , tree1Position.Y);
                //Cells.Remove(new Point(5, 5));
                //gameObjects.Remove(gameObjects[55]);

                //SpriteRenderer sr = (SpriteRenderer)gameObjects[37].GetComponent<SpriteRenderer>();
                //sr.SetSprite("1fwd");
                
                //gameObjects[22].Transform.Position = new Vector2(20, 20);
                Player player = gameObjects[0].GetComponent<Player>() as Player;
                //player.GameObject.Transform.PosOnCell = new Point(8, 8);
                //player.GameObject.Transform.Position = new Vector2(1000, 80);
                player.GameObject.Transform.CellMovement(new Vector2(1100), new Vector2(300));

                //Cells[new Point(5, 5)].Sprite = Instance.Content.Load<Texture2D>("1fwd");
                //Cells[new Point(5, 5)].Sprite = sprites["Pixel"];
                //sr.SetSprite("cellGrid");
                //Point test = new Point(5, 5);
                //gameObjects[100].
                //var test = gameObjects[55].GetComponent<SpriteRenderer>();
                //test.Sprite = sprites["Pixel"];

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
                    if (x != 8)
                    {
                        Cells.Add(new Point(x, y), new Cell(new Point(x, y), cellSize, cellSize));
                    GameObject cellGrid = new GameObject();
                    SpriteRenderer sr = cellGrid.AddComponent<SpriteRenderer>();
                    gameObjects.Add(cellGrid);
                    sr.SetSprite("cellGrid");
                    cellGrid.Transform.Scale = new Vector2(1, 1);
                    Point pos = new Point(x, y);
                    //Cells[new Point(x, y)].Sprite = Instance.Content.Load<Texture2D>("Pixel");
                    //Cells[pos].Sprite = sprites["Pixel"];
                    cellGrid.Transform.Position = new Vector2(pos.X * 100, pos.Y * 100);
                        //cellGrid.Transform.PosOnCell = new Point(x * 100, y * 100);
                        //SpriteRenderer sr1 = (SpriteRenderer)gameObjects[0].GetComponent<SpriteRenderer>();
                        //sr.GameObject.Transform.PosOnCell = new Point(x, y);
                    }
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (GameObject go in gameObjects)
            {
                go.Draw(_spriteBatch);
            }

           // _spriteBatch.Draw(); //Draw background

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
