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
using System.Reflection;
using System.Runtime.CompilerServices;
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
        Vector2 originText;
        string fontText = "";
        Vector2 fontLength;

        private int index = 0;
        public static List<Point> targetPointList = new List<Point>();

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
            IRepository repository = new Database();
            new Database(repository).RunLoop();

            Director director = new Director(new PlayerBuilder());
            Director director1 = new Director(new ArmsDealerBuilder());
            GameObject playerGo = director.Construct();
            GameObject armsDealerGo = director1.Construct();
            Player player = playerGo.GetComponent<Player>() as Player;
            ArmsDealer armsDealer = armsDealerGo.GetComponent<ArmsDealer>() as ArmsDealer;

            GameObject database = new GameObject();
            // database.AddComponent<UI>();

            InputHandler.Instance.AddUpdateCommand(Keys.D, new MoveCommand(player, new Vector2(1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.A, new MoveCommand(player, new Vector2(-1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.W, new MoveCommand(player, new Vector2(0, -1)));
            InputHandler.Instance.AddUpdateCommand(Keys.S, new MoveCommand(player, new Vector2(0, 1)));
            InputHandler.Instance.AddUpdateCommand(Keys.M, new AttackCommand(player));
            InputHandler.Instance.AddUpdateCommand(Keys.P, new InventoryCommand(player.inventory));

            //sprites.Add("cellGrid", Content.Load<Texture2D>("cellGrid"));
            //sprites.Add("1fwd", Content.Load<Texture2D>("1fwd"));
            //sprites.Add("Robot1", Content.Load<Texture2D>("Robot1"));
            //CellManager cellManager = new CellManager();
            //cellManager.SetUpCells(10,10);

            gameObjects.Add(playerGo);
            gameObjects.Add(armsDealerGo);
            gameObjects.Add(database);

            gameObjects.Add(EnemyFactory.Instance.Create());
            gameObjects.Add(ButtonFactory.Instance.Create(new Vector2(500, 200), "Respawn", Exit));

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
                    SceneShop(gameTime);
                    break;
                case GameState.Combat:
                    SceneCombat(gameTime);
                    break;
                    //case GameState.EndOfGame:
                    //    UpdateEndOfGame(gameTime);
                    //    break;
            }

            if (timeElapsed >= 0.3f)
            {
                InputHandler.Instance.Execute();
                timeElapsed = 0;
            }
            //InputHandler.Instance.Execute();
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

            //if (timeElapsed >= 0.3f)
            //{
            Enemy enemy = gameObjects[3].GetComponent<Enemy>() as Enemy;
            enemy.GetPlayerPosition(gameObjects[0].Transform.VectorToPointConverter(gameObjects[0].Transform.Position));
            //timeElapsed = 0;
            //}

            foreach (GameObject go in gameObjects)
            {
                go.Update(gameTime);
            }

            KeyboardState keyState = Keyboard.GetState();

            //if (keyState.IsKeyDown(Keys.C) && timeElapsed >= 0.3f)
            //{
            //    // Cells[gameObjects[100].Transform.CellMovement(gameObjects[100].Transform.Position)].Sprite = sprites["1fwd"];
            //    SpriteRenderer sr = (SpriteRenderer)gameObjects[101].GetComponent<SpriteRenderer>();
            //    sr.SetSprite("1fwd");
            //}

            if (keyState.IsKeyDown(Keys.B) && timeElapsed >= 0.3f)
            {
                timeElapsed = 0;
            }
            base.Update(gameTime);

                Cleanup();
            }


        void SceneShop(GameTime deltaTime) 
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.B))
            {
                _state = GameState.Combat;
            }
        }

        void SceneCombat(GameTime deltaTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.N))
            {
                _state = GameState.Shop;
            }
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
                    Enemy enemy = gameObjects[3].GetComponent<Enemy>() as Enemy;
                    enemy.GameObject.Transform.Position = new Vector2 (VARIABLE.Position.X * 100, VARIABLE.Position.Y * 100);
                    Thread.Sleep(1000);
                    for (int i = 0; i < Cells.Count; i++)
                    {
                        //if (Cells.ElementAt(i).Key == VARIABLE.Position)
                        //{
                        //    SpriteRenderer sr2 = (SpriteRenderer)gameObjects[i].GetComponent<SpriteRenderer>();
                        //    sr2.SetSprite("1fwd");
                        //    sr2.GameObject.Transform.Layer = 0.1f;
                        //    //Enemy enemy = gameObjects[103].GetComponent<Enemy>() as Enemy;
                        //    //enemy.GameObject.Transform.Position = gameObjects[i].Transform.Position;
                        //    //Cells[targetPointList[index]].Sprite = sprites["1fwd"];
                        //    //break;
                        //}
                    }
                }
                index++;
            }

            if (index < targetPointList.Count)
            {
                //RunAStar();
            }
            index = 0;
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
            go.Awake();
            go.Start();
        }

        /// <summary>
        /// Adding GameObject to destroyed GameObjects list
        /// </summary>
        /// <param name="go"></param>
        public void Destroy(GameObject go)
        {
            destroyedGameObjects.Add(go);
        }
        public void CreateRespawnButton()
        {
            specificButton = ButtonFactory.Instance.Create(new Vector2(1000, 1000), "Respawn", RespawnPlayer);
            Instantiate(specificButton);
            //gameObjects.Add(specificButton);
        }

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
            InputHandler.Instance.AddUpdateCommand(Keys.M, new AttackCommand(player));
            InputHandler.Instance.AddUpdateCommand(Keys.P, new InventoryCommand(player.inventory));
            Destroy(specificButton);
           
        }
        void CheckCollision()
        {
            try
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
            catch (Exception)
            {

                Debug.Write("stuff borke");
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
            _spriteBatch.DrawString(font, $"{mouseState}", new Vector2(300, 300), Color.Black, 0, originText, 1f, SpriteEffects.None, 1f);
            // _spriteBatch.Draw(); //Draw background

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
