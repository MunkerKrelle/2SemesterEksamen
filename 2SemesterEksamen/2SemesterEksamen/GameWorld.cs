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
using SharpDX.Direct3D9;
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
        private float timeElapsed;
        //private bool startAstarBool = true;

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
            database.AddComponent<UI>();
         
            //gameObjects.Add(EnemyFactory.Instance.Create());
          
            //InputHandler.Instance.AddUpdateCommand(Keys.D, new MoveCommand(player, new Vector2(1, 0)));
            //InputHandler.Instance.AddUpdateCommand(Keys.A, new MoveCommand(player, new Vector2(-1, 0)));
            //InputHandler.Instance.AddUpdateCommand(Keys.W, new MoveCommand(player, new Vector2(0, -1)));
            //InputHandler.Instance.AddUpdateCommand(Keys.S, new MoveCommand(player, new Vector2(0, 1)));

            InputHandler.Instance.AddUpdateCommand(Keys.D, new MoveCommand(player, new Vector2(1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.A, new MoveCommand(player, new Vector2(-1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.W, new MoveCommand(player, new Vector2(0, -1)));
            InputHandler.Instance.AddUpdateCommand(Keys.S, new MoveCommand(player, new Vector2(0, 1)));
            
            //var treeSprite = GameWorld.Instance.sprites["Pixel"];
            //walls.Add(WallFactory.Instance.Create());
            //walls[i].Transform.Position = new Vector2(115 * i, 0);
            
            sprites.Add("cellGrid", Content.Load<Texture2D>("cellGrid"));
            sprites.Add("1fwd", Content.Load<Texture2D>("1fwd"));
            sprites.Add("Robot1", Content.Load<Texture2D>("Robot1"));
            SetUpCells();

            gameObjects.Add(playerGo);
            //playerGo.Transform.CellMovement(new Vector2(1000), new Vector2(1000));
            gameObjects.Add(armsDealerGo);
            gameObjects.Add(database);

            gameObjects.Add(EnemyFactory.Instance.Create());
            gameObjects.Last().Transform.Position = new Vector2(40, 40);
            //Enemy enemy = gameObjects.Last().GetComponent<Enemy>() as Enemy;
            ////enemy.GetPlayerPosition(gameObjects[100].Transform.Position);
            //enemy.GetPlayerPosition(gameObjects[100].Transform.PosOnCell);

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
                //index++;
                Enemy enemy = gameObjects[103].GetComponent<Enemy>() as Enemy;
                //enemy.GetPlayerPosition(gameObjects[100].Transform.Position);
                enemy.GetPlayerPosition(gameObjects[100].Transform.CellMovement(gameObjects[100].Transform.Position));
                timeElapsed = 0;
            }
          

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.C) && timeElapsed >= 0.3f)
            {
                //Point tree1Position = new Point(-10, -10);
                //gameObjects[100].Transform.Position += new Vector2(tree1Position.X , tree1Position.Y);
                //Cells.Remove(new Point(5, 5));
                //gameObjects.Remove(gameObjects[55]);

                //SpriteRenderer sr = (SpriteRenderer)gameObjects[0].GetComponent<SpriteRenderer>();
                //sr.SetSprite("1fwd");

                //Cells[new Point(1, 1)].Sprite = sprites["1fwd"];
                //Cells[new Point(5, 5)].Sprite = sprites["1fwd"];
                //Cells[new Point(4, 5)].Sprite = sprites["Robot1"];
                //Cells[new Point(5, 6)].Sprite = sprites["Robot1"];
                //Cells[new Point(5, 7)].Sprite = sprites["Robot1"];
                //Cells[new Point(5, 8)].Sprite = sprites["Robot1"];s

                //Point player2 = new Point(targetPointPos.X, targetPointPos.Y);

                
                //gameObjects[100].Transform.CellMovement(new Vector2(1000), new Vector2(1000));
                
                Cells[gameObjects[100].Transform.CellMovement(gameObjects[100].Transform.Position)].Sprite = sprites["1fwd"];
                SpriteRenderer sr = (SpriteRenderer)gameObjects[100].GetComponent<SpriteRenderer>();
                sr.SetSprite("1fwd", 1);


               
                
                //gameObjects[100].Transform.CellMovement(gameObjects[100].Transform.Position);

                //Point player1 = new Point(1, 1);
                //Point player2 = new Point(5, 5);
                //Point enemy1 = new Point(3, 7);
                //Point enemy2 = new Point(2, 6);
                //Point enemy3 = new Point(8, 9);
                //Point enemy4 = new Point(10, 10);

                //targetPointList.Add(player1);
                //targetPointList.Add(player2);

                //targetPointList.Add(enemy1);
                //targetPointList.Add(enemy2);
                //targetPointList.Add(enemy3);
                //targetPointList.Add(enemy4);



                //gameObjects.Add(EnemyFactory.Instance.Create());
                //gameObjects.Last().Transform.Position = new Vector2(160, 160);

                //if (startAstarBool == true) 
                //{
                //    startAstarBool = false;
                //    RunAStar();
                //}

                //RunAStar();
                timeElapsed = 0;
                

                //Cell cell = Cells.Values.ElementAt(0);
                //gameObjects[22].Transform.Position = new Vector2(20, 20);

                //Player player = gameObjects[100].GetComponent<Player>() as Player;
                //player.GameObject.Transform.CellMovement(new Vector2(1100), new Vector2(300));

                //Enemy enemy = gameObjects[103].GetComponent<Enemy>() as Enemy;
                //gameObjects[103].Transform.CellMovement(new Vector2(1200), new Vector2(300));

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
            if (keyState.IsKeyDown(Keys.V))
            {
                //Enemy enemy = gameObjects.Last().GetComponent<Enemy>() as Enemy;
                //Enemy enemy = gameObjects[103].GetComponent<Enemy>() as Enemy;
                ////enemy.GetPlayerPosition(gameObjects[100].Transform.Position);
                //enemy.GetPlayerPosition(gameObjects[100].Transform.CellMovement(gameObjects[100].Transform.Position));

                //timeElapsed = 0;
                //SpriteRenderer sr = (SpriteRenderer)gameObjects[37].GetComponent<SpriteRenderer>();
                //sr.SetSprite("cellGrid");
                //SpriteRenderer sr2 = (SpriteRenderer)gameObjects[38].GetComponent<SpriteRenderer>();
                //sr2.SetSprite("1fwd");
                //Player player = gameObjects[0].GetComponent<Player>() as Player;
                //player.GameObject.Transform.PosOnCell = new Point(8, 8);
                //player.GameObject.Transform.Position = new Vector2(1000, 80);
                //player.GameObject.Transform.CellMovement(new Vector2(1200), new Vector2(500));
                //Enemy enemy = gameObjects[103].GetComponent<Enemy>() as Enemy;
                //gameObjects[103].Transform.CellMovement(new Vector2(1200), new Vector2(300));
                // enemy.GameObject.Transform.CellMovement2(new Vector2(100));
            }

            if (keyState.IsKeyDown(Keys.B) && timeElapsed >= 0.3f)
            {
                //InputHandler.Instance.Execute();
                timeElapsed = 0;
                //SpriteRenderer sr = (SpriteRenderer)gameObjects[38].GetComponent<SpriteRenderer>();
                //sr.SetSprite("cellGrid");
                //SpriteRenderer sr2 = (SpriteRenderer)gameObjects[39].GetComponent<SpriteRenderer>();
                //sr2.SetSprite("1fwd");
                //Cells[new Point(1, 1)].Sprite = sprites["Robot1"];
                //Cell cell = Cells.Values.ElementAt(0);
                //gameObjects[22].Transform.Position = new Vector2(20, 20);
                //Player player = gameObjects[100].GetComponent<Player>() as Player;
                // player.GameObject.Transform.CellMovement(new Vector2(1100), new Vector2(300));

                //Enemy enemy = gameObjects[103].GetComponent<Enemy>() as Enemy;
                // gameObjects[103].Transform.CellMovement(new Vector2(1200), new Vector2(300));

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
                //var path = astar.FindPath(targetPointList[0], targetPointList[index]);
                //foreach (var VARIABLE in path)
                //{
                //    for (int i = 0; i < Cells.Count; i++)
                //    {
                //        //if (Cells.ElementAt(i).Key == new Point (3,5))
                //        //if (Cells.ElementAt(i).Key == targetPointList[index])
                //        if (Cells.ElementAt(i).Key == VARIABLE.Position)
                //        {
                //            //gameObjects[i].Transform.Position = new Vector2(1500, 1500);
                //            //SpriteRenderer sr2 = (SpriteRenderer)gameObjects[i].GetComponent<SpriteRenderer>();
                //            ////Cells[targetPointList[index]].Sprite = sprites["1fwd"];
                //            //sr2.SetSprite("1fwd", 0.1f);
                //            //break;
                //        }
                //    }
                //}
                index++;
            }

                if (index > 0 && index <= targetPointList.Count)
                {
                    var path = astar.FindPath(targetPointList[index - 1], targetPointList[index]);
                    foreach (var VARIABLE in path)
                    {
                    //Vector2 tmpPos = VARIABLE.Position.ToVector2();
                    //wizard.Position = new Vector2(tmpPos.X * cellSize + 40, tmpPos.Y * cellSize);
                    //VARIABLE.spriteColor = Color.Aqua;
                    //Cells[targetPointList[index]].Sprite = sprites["1fwd"];
                    //SpriteRenderer sr1 = (SpriteRenderer)gameObjects[0].GetComponent<SpriteRenderer>();
                    //sr1.SetSprite("1fwd");

                    //sr1.SetSprite("cellGrid");
                    //if (VARIABLE.Position.X == targetPointList[index - 1].X && VARIABLE.Position.Y == targetPointList[index - 1].Y) 
                    //{
                    //    SpriteRenderer sr2 = (SpriteRenderer)gameObjects[VARIABLE.Position.X].GetComponent<SpriteRenderer>();
                    //}
                    for (int i = 0; i < Cells.Count; i++)
                        {
                            //if (Cells.ElementAt(i).Key == new Point (3,5))
                            //if (Cells.ElementAt(i).Key == targetPointList[index])
                            if (Cells.ElementAt(i).Key == VARIABLE.Position)
                            {

                            //gameObjects[i].Transform.Position = new Vector2(1500, 1500);
                            
                            SpriteRenderer sr2 = (SpriteRenderer)gameObjects[i].GetComponent<SpriteRenderer>();
                            sr2.SetSprite("1fwd", 0.1f);

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
                    sr.SetSprite("cellGrid",0);
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
