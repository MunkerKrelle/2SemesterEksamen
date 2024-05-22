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

            gameObjects.Add(ButtonFactory.Instance.Create(new Vector2(500, 200), "Respawn", Exit));
            respawnButton = new Button(new Vector2(500, 300), "Respawn", RespawnPlayer);
            respawnButton.active = false; 

            GameObject database = new GameObject();
            database.AddComponent<UI>();
            gameObjects.Add(database);

            gameObjects.Add(EnemyFactory.Instance.Create());



            InputHandler.Instance.AddUpdateCommand(Keys.D, new MoveCommand(player, new Vector2(1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.A, new MoveCommand(player, new Vector2(-1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.W, new MoveCommand(player, new Vector2(0, -1)));
            InputHandler.Instance.AddUpdateCommand(Keys.S, new MoveCommand(player, new Vector2(0, 1)));

            InputHandler.Instance.AddUpdateCommand(Keys.P, new AttackCommand(player));

            gameObjects.Add(EnemyFactory.Instance.Create());

            //player.GameObject.Transform.CellMovement(new Vector2(1050), new Vector2(1050));

            foreach (GameObject go in gameObjects)
            {
                go.Awake();
            }
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

            //if (keyState.IsKeyDown(Keys.C))
            //{

            //    Cells[new Point(1, 1)].Sprite = sprites["1fwd"];

            //    Player player = gameObjects[0].GetComponent<Player>() as Player;
            //    player.GameObject.Transform.CellMovement(new Vector2(1100), new Vector2(300));


            //}
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

            foreach (GameObject go in gameObjects)

            if (respawnButton.active)
            {
                respawnButton.Update();
            }

            if (keyState.IsKeyDown(Keys.V))
            {
            Enemy enemy = gameObjects[104].GetComponent<Enemy>() as Enemy;
            enemy.GetPlayerPosition(gameObjects[101].Transform.VectorToPointConverter(gameObjects[101].Transform.Position));
            timeElapsed = 0;
            }

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.C) && timeElapsed >= 0.3f)
            {
                // Cells[gameObjects[100].Transform.CellMovement(gameObjects[100].Transform.Position)].Sprite = sprites["1fwd"];
                SpriteRenderer sr = (SpriteRenderer)gameObjects[101].GetComponent<SpriteRenderer>();
                sr.SetSprite("1fwd");
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
       

        public void RunAStar()
        {
            Astar astar = new Astar(Cells);

            if (index > targetPointList.Count - 1)
            {
                return;
            }

            if (index == 0)
            {
                index++;
            }

            if (index > 0 && index <= targetPointList.Count)
            {
                var path = astar.FindPath(targetPointList[index - 1], targetPointList[index]);
                foreach (var VARIABLE in path)
                {
                    Enemy enemy = gameObjects[104].GetComponent<Enemy>() as Enemy;
                    enemy.GameObject.Transform.Position = new Vector2 (VARIABLE.Position.X * 100, VARIABLE.Position.Y * 100);
                    for (int i = 0; i < Cells.Count; i++)
                        {
                            if (Cells.ElementAt(i).Key == VARIABLE.Position)
                            {
                            SpriteRenderer sr2 = (SpriteRenderer)gameObjects[i].GetComponent<SpriteRenderer>();
                            sr2.SetSprite("1fwd");
                            sr2.GameObject.Transform.Layer = 0.1f;
                            //Enemy enemy = gameObjects[103].GetComponent<Enemy>() as Enemy;
                            //enemy.GameObject.Transform.Position = gameObjects[i].Transform.Position;
                            //Cells[targetPointList[index]].Sprite = sprites["1fwd"];
                            //break;
                        }
                    }
                }
                index++;
            }

            if (index < targetPointList.Count)
            {
                RunAStar();
            }
            index = 0;
        }
    
        private void SetUpCells() //flyttes over til Cells
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
                    cellGrid.Transform.Layer = 0f;
                    Cells[new Point(x, y)].Sprite = sprites["cellGrid"];                     
                    cellGrid.Transform.Scale = new Vector2(1, 1);
                    Point pos = new Point(x, y);

                    cellGrid.Transform.Position = new Vector2(pos.X * 100, pos.Y * 100);

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

            _spriteBatch.DrawString(font, $"{mouseState}", new Vector2(300, 300), Color.Black, 0, originText, 1f, SpriteEffects.None, 1f);
            // _spriteBatch.Draw(); //Draw background

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
