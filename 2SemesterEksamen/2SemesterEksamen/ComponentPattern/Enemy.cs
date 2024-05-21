using _2SemesterEksamen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
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
        private bool startAstarBool = false;
        private Point targetPointPos;

        public Vector2 velocity = new Vector2(0, 1);
        public Enemy(GameObject gameObject) : base(gameObject)
        {
            enemyhealth = 100;
            //ChangeState();
        }

        float timeSinceLastSwitch;
        float changeTime = 1f;

        public override void Update(GameTime gameTime)
        {
            SearchForPlayer();
            if (Health < 0)
            {
                GameWorld.Instance.Destroy(GameObject);
            }
        }

        public override void OnCollisionEnter(Collider col)
        {
            Player player = (Player)col.GameObject.GetComponent<Player>();

            if (player != null)
            {
                player.Health -= 1;
            }
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

            if (keyState.IsKeyDown(Keys.G) && (startAstarBool == false)) 
            {
                //if (startAstarBool == true)
                //{
                    startAstarBool = true;
                    //Point player1 = new Point(1, 1);
                    //Point player2 = new Point(5, 5);
                    //Point enemy1 = new Point(3, 7);
                    //Point enemy2 = new Point(2, 6);
                    //Point enemy3 = new Point(8, 9);
                    //Point enemy4 = new Point(10, 10);

                    //GameWorld.targetPointList.Add(player1);
                    //GameWorld.targetPointList.Add(player2);

                    //GameWorld.targetPointList.Add(enemy1);
                    //GameWorld.targetPointList.Add(enemy2);
                    //GameWorld.targetPointList.Add(enemy3);
                    //GameWorld.targetPointList.Add(enemy4);
                    //Enemy enemy = gameObjects[103].GetComponent<Enemy>() as Enemy;
                    ////enemy.GetPlayerPosition(gameObjects[100].Transform.Position);
                    //enemy.GetPlayerPosition(gameObjects[100].Transform.CellMovement(gameObjects[100].Transform.Position));
                    //timeElapsed = 0;


                    Point player1 = new Point(5, 5);

                    Point player2 = new Point(targetPointPos.X, targetPointPos.Y);
                    //Point enemy1 = new Point(1, 9);
                    //Point enemy2 = new Point(9, 9);
                    //Point enemy3 = new Point(9, 1);

                    GameWorld.targetPointList.Add(player1);
                    GameWorld.targetPointList.Add(player2);
                    //GameWorld.targetPointList.Add(enemy1);
                    //GameWorld.targetPointList.Add(enemy2);
                    //GameWorld.targetPointList.Add(enemy3);

                    //Point enemy1 = new Point(1, 2);
                    //Point enemy2 = new Point(1, 3);

                    //GameWorld.targetPointList.Add(enemy1);
                    //GameWorld.targetPointList.Add(enemy2);

                    Thread enemyThread = new Thread(GameWorld.Instance.RunAStar);
                    enemyThread.IsBackground = true;
                    enemyThread.Start();
                    //Thread.Sleep(1000);
                //}
            }

            if (keyState.IsKeyDown(Keys.H)) 
            {
                startAstarBool = false;
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
            //startAstarBool = true;
        }

    }
}

