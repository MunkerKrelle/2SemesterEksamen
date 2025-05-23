﻿using Algoritmer;
using BuilderPattern;
using CommandPattern;
using ComponentPattern;
using FactoryPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RepositoryPattern;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace _2SemesterEksamen
{
    enum GameState
    {
        Shop,
        Combat
    }

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
        private GameObject specificButton;

        public static MouseState mouseState;
        public static MouseState newState;
        public static bool isPressed;


        private float timeElapsed;
        private GameState _state;

        public static SpriteFont font;
        public ArmsDealer armsDealer;
        private int index = 0;
        public List<Point> targetPointList = new List<Point>();
  

        public List<GameObject> GameObjects
        {
            get
            {
                return gameObjects;
            }
        }
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
            try
            {
                IRepository.currentRepository = new Database();
                new Database(IRepository.currentRepository).CreateDatabase();
            }
            catch
            {
                IRepository.currentRepository = new CSV();
                new CSV(IRepository.currentRepository).CreateDatabase();
            }


            Director director = new Director(new PlayerBuilder());
            Director director1 = new Director(new ArmsDealerBuilder());
            GameObject playerGo = director.Construct();
            GameObject armsDealerGo = director1.Construct();
            Player player = playerGo.GetComponent<Player>() as Player;
            armsDealer = armsDealerGo.GetComponent<ArmsDealer>() as ArmsDealer;
            GameObject database = new GameObject();

            InputHandler.Instance.AddUpdateCommand(Keys.D, new MoveCommand(player, new Vector2(1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.A, new MoveCommand(player, new Vector2(-1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.W, new MoveCommand(player, new Vector2(0, -1)));
            InputHandler.Instance.AddUpdateCommand(Keys.S, new MoveCommand(player, new Vector2(0, 1)));
            InputHandler.Instance.AddUpdateCommand(Keys.P, new AttackCommand(player));
            InputHandler.Instance.AddUpdateCommand(Keys.C, new InventoryCommand(player));

            sprites.Add("cellGrid", Content.Load<Texture2D>("cellGrid"));
            sprites.Add("1fwd", Content.Load<Texture2D>("Robot1"));
            sprites.Add("Robot1", Content.Load<Texture2D>("Robot1"));
            sprites.Add("EnterShop", Content.Load<Texture2D>("EnterShop"));
            sprites.Add("ExitShop", Content.Load<Texture2D>("ExitShop"));
            CellManager cellManager = new CellManager();
            cellManager.SetUpCells(11, 11);

            gameObjects.Add(playerGo);
            gameObjects.Add(armsDealerGo);
            gameObjects.Add(database);

            gameObjects.Add(EnemyFactory.Instance.Create());
            gameObjects.Add(EnemyFactory.Instance.Create());
            gameObjects.Add(EnemyFactory.Instance.Create());
            gameObjects.Add(EnemyFactory.Instance.Create());
            gameObjects.Add(ButtonFactory.Instance.Create(new Vector2(800, 200), "GenerateShop", armsDealer.UpdateItems));

            foreach (GameObject go in gameObjects)
            {
                go.Awake();
            }

            _graphics.PreferredBackBufferWidth = 11 * 100 + 200;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 11 * 100 + 1;   // set this value to the desired height of your window
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

            switch (_state)
            {
                case GameState.Shop:
                    SceneShop();
                    break;
                case GameState.Combat:
                    SceneCombat();
                    break;
            }

            if (timeElapsed >= 0.3f)
            {
                InputHandler.Instance.Execute();
                timeElapsed = 0;
            }

            CheckCollision();
            EnterShop();
            ExitShop();

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
            {
                go.Update(gameTime);
            }

            base.Update(gameTime);

            Cleanup();
        }

        /// <summary>
        /// Spilleren forlader "Combat" området og går ind i shoppen
        /// </summary>
        private void EnterShop()
        {
            if (gameObjects[0].Transform.Position == new Vector2(900, 100))
            {
                _state = GameState.Shop;

                for (int i = 5; i < Cells.Count + 5; i++)
                {
                    gameObjects[i].Transform.Transformer(gameObjects[i].Transform.Position, 0, new Vector2(1, 1), Color.SaddleBrown, 0f);
                }

                Enemy enemy = gameObjects[3].GetComponent<Enemy>() as Enemy;
                enemy.startAstarBool = false;
                enemy.GameObject.Transform.Position = new Vector2(2000, 2000);
                armsDealer.GameObject.IsActive = false;
                //Inventory inventory = armsDealer.GameObject.GetComponent<Inventory>() as Inventory;
                //foreach (var i in inventory.weaponsList)
                //{
                //    i.GameObject.IsActive = true;
                //    i.button.IsActive = true;
                //}

            }
        }

        /// <summary>
        /// Spilleren forlader shoppen og går til "Combat" området
        /// </summary>
        private void ExitShop()
        {
            if (_state != GameState.Combat)
            {
                if (gameObjects[0].Transform.Position == new Vector2(1000, 100))
                {
                    _state = GameState.Combat;

                    for (int i = 5; i < Cells.Count + 5; i++)
                    {
                        gameObjects[i].Transform.Transformer(gameObjects[i].Transform.Position, 0, new Vector2(1, 1), Color.White, 0f);
                    }

                    // UI elements go away

                    // Astar starts again
                    Enemy enemy = gameObjects[3].GetComponent<Enemy>() as Enemy;
                    enemy.startAstarBool = true;
                    enemy.GameObject.Transform.Position = new Vector2(500, 500);


                    armsDealer.GameObject.IsActive = true;
                }
            }
        }

        /// <summary>
        /// Skifter scene til "Shop" så våben og armsDealer loader frem
        /// </summary>
        private void SceneShop()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.B))
            {
                _state = GameState.Combat;
            }
        }

        /// <summary>
        /// Skifter scene til "Combat" så enemies loader frem
        /// </summary>
        private void SceneCombat()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.N))
            {
                _state = GameState.Shop;
            }
        }

        /// <summary>
        /// Starter AStar for enemy, så den begynder at lede efter spilleren, hvis de findes
        /// </summary>
        //public void RunAStar()
        //{
        //    Astar astar = new Astar(Cells);
        //    Enemy enemy = gameObjects[3].GetComponent<Enemy>() as Enemy;

        //    if (index > targetPointList.Count - 1)
        //    {
        //        return;
        //    }

        //    if (index == 0)
        //    {
        //        index++;
        //    }

        //    if (index > 0 && index <= targetPointList.Count)
        //    {
        //        var path = astar.FindPath(targetPointList[index - 1], targetPointList[index]);
        //        foreach (var VARIABLE in path)
        //        {
        //            enemy.animator.PlayAnimation("CyborgMove");
        //            enemy.GameObject.Transform.Position = new Vector2(VARIABLE.Position.X * 100, VARIABLE.Position.Y * 100);
        //            Thread.Sleep(1000);
        //        }
        //        index++;
        //    }

        //    index = 0;
        //    enemy.startAstarBool = true;
        //}

        /// <summary>
        /// Laver en knap der respawner spilleren
        /// </summary>
        public void CreateRespawnButton()
        {
            specificButton = ButtonFactory.Instance.Create(new Vector2(1000, 1000), "Respawn", RespawnPlayer);
            Instantiate(specificButton);
        }

        /// <summary>
        /// Respawn spilleren med deres componenter og inputs, da de også blev fjernet med spilleren
        /// </summary>
        private void RespawnPlayer()
        {
            Director director = new Director(new PlayerBuilder());
            GameObject playerGo = director.Construct();
            Player player = playerGo.GetComponent<Player>() as Player;
            player.Respawn();
            InputHandler.Instance.ClearCommands();

            InputHandler.Instance.AddUpdateCommand(Keys.D, new MoveCommand(player, new Vector2(1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.A, new MoveCommand(player, new Vector2(-1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.W, new MoveCommand(player, new Vector2(0, -1)));
            InputHandler.Instance.AddUpdateCommand(Keys.S, new MoveCommand(player, new Vector2(0, 1)));
            InputHandler.Instance.AddUpdateCommand(Keys.P, new AttackCommand(player));
            InputHandler.Instance.AddUpdateCommand(Keys.C, new InventoryCommand(player));
            Destroy(specificButton);

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
        /// <param name="go">GameObject der skal instaniate</param>
        public void Instantiate(GameObject go)
        {
            newGameObjects.Add(go);
        }

        /// <summary>
        /// Adding GameObject to destroyed GameObjects list
        /// </summary>
        /// <param name="go">GameObject der skal slettes</param>
        public void Destroy(GameObject go)
        {
            destroyedGameObjects.Add(go);
        }

        /// <summary>
        /// Checker om 2 GameObjects rammer hinanden  
        /// </summary>
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
                        foreach (Collider.RectangleData rects1 in col1.rectangles.Value)
                        {
                            foreach (Collider.RectangleData rects2 in col2.rectangles.Value)
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (GameObject go in gameObjects)
            {
                go.Draw(_spriteBatch);
            }
            _spriteBatch.DrawString(font, $"{mouseState}", new Vector2(300, 300), Color.Black, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
