using _2SemesterEksamen;
using CommandPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StatePattern;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ComponentPattern
{
    public class Enemy : Component
    {
        public float speed = 1;
        protected int enemyhealth;
        public int Health
        {
            get { return enemyhealth; }
            set { enemyhealth = value; }
        }
        public bool startAstarBool = true;
        private Point EnemyPointPosition;
        private Point targetPointPos;
        private float enemyTimer;
        private float timeElapsed2;

        public Vector2 velocity = new Vector2(0, 1);
        public Enemy(GameObject gameObject) : base(gameObject)
        {
            enemyhealth = 100;
            //ChangeState();
        }

        float timeSinceLastSwitch;
        float changeTime = 1f;

        public override void Start() 
        {
            GameObject.Transform.Position = new Vector2(500, 500);
        }

        public override void Update(GameTime gameTime)
        {
            GetPlayerPosition(GameWorld.Instance.GameObjects[0].Transform.VectorToPointConverter(GameWorld.Instance.GameObjects[0].Transform.Position));
            enemyTimer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeElapsed2 += enemyTimer;
            
            if (timeElapsed2 >= 1f)
            {
                SearchForPlayer();
                timeElapsed2 = 0;
            }
            
            if (Health < 0)
            {
                GameWorld.Instance.Destroy(GameObject);
            }
        }

        public override void OnCollisionEnter(Collider col)
        {
            Player player = (Player)col.GameObject.GetComponent<Player>();

            //if (player != null)
            //{
            //    player.Health -= 1;
            //}
            base.OnCollisionEnter(col);
        }

        Istate<Enemy> currentState;

        public void ChangeState(Istate<Enemy> state)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }
            currentState = state;
            currentState.Enter(this);
        }

        public void SearchForPlayer()
        {
            //RUN ASTAR
            
      
            KeyboardState keyState = Keyboard.GetState();

            if (/*keyState.IsKeyDown(Keys.G) && */(startAstarBool == true))
            {
                startAstarBool = false;
                EnemyPointPosition = GameObject.Transform.VectorToPointConverter(GameObject.Transform.Position);
                Point enemy1 = new Point(EnemyPointPosition.X, EnemyPointPosition.Y);
                Point player1 = new Point(targetPointPos.X, targetPointPos.Y);

                if (GameWorld.targetPointList.Count == 2)
                {
                    GameWorld.targetPointList.Clear();
                }

                if (GameWorld.targetPointList.Count < 2) 
                {
                    GameWorld.targetPointList.Add(enemy1);
                    GameWorld.targetPointList.Add(player1);
                }

              
                Thread enemyThread = new Thread(GameWorld.Instance.RunAStar);
                enemyThread.IsBackground = true;
                enemyThread.Start();

                //Thread.Sleep(1000);
            }


            if (keyState.IsKeyDown(Keys.H)) 
            {
                startAstarBool = true;
                GameWorld.targetPointList.Clear();
            }
            //IF DISTANCE < WHATEVER
            //{
            AttackPlayer();
            //}
        }

        private void AttackPlayer()
        {
            //HVERT TREDJE ISH SEKUND, PLAYER.HEALTH - 2
        }
      
        public void GetPlayerPosition(Point playerPoint) 
        {
            targetPointPos = playerPoint;
        }

    }
}

