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
            
            InputHandler.Instance.AddUpdateCommand(Keys.D, new MoveCommand(player, new Vector2(1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.A, new MoveCommand(player, new Vector2(-1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.W, new MoveCommand(player, new Vector2(0, -1)));
            InputHandler.Instance.AddUpdateCommand(Keys.S, new MoveCommand(player, new Vector2(0, 1)));

            gameObjects.Add(EnemyFactory.Instance.Create());

            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
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

            base.Update(gameTime);

            Cleanup();
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
