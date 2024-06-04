using _2SemesterEksamen;
using Algoritmer;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using StatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing.Text;

namespace ComponentPattern
{
    internal class NewEnemy : Component
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
        public Animator animator;

        public Vector2 velocity = new Vector2(0, 1);
        static readonly object DamagePlayerLock = new object();

        public List<Point> targetPointList = new List<Point>();

        public NewEnemy(GameObject gameObject) : base(gameObject)
        {

        }

        float timeSinceLastSwitch;
        float changeTime = 1f;

        public override void Awake()
        {

            Health = 100;
            GameObject.Transform.Scale = new Vector2(1f, 1f);
            animator = GameObject.GetComponent<Animator>() as Animator;
            animator.PlayAnimation("CyborgIdle");

        }
        public override void Start()
        {
            Random rnd = new Random();
            GameObject.Transform.Position = new Vector2(rnd.Next(100, 1000), rnd.Next(100, 1000));
        }

        public override void Update(GameTime gameTime)
        {
            SearchForPlayer();



            if (Health <= 0)
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

        public void SearchForPlayer()
        {
            //RUN ASTAR
            KeyboardState keyState = Keyboard.GetState();

            if (startAstarBool == true)
            {
                startAstarBool = false;
                EnemyPointPosition = GameObject.Transform.VectorToPointConverter(GameObject.Transform.Position);
                //Thread.Sleep(100);
                GetPlayerPosition(GameWorld.Instance.GameObjects[0].Transform.VectorToPointConverter(GameWorld.Instance.GameObjects[0].Transform.Position));
                Point enemy1 = new Point(EnemyPointPosition.X, EnemyPointPosition.Y);
                Point player1 = new Point(targetPointPos.X, targetPointPos.Y);

                if (targetPointList.Count == 2)
                {
                    targetPointList.Clear();
                }

                if (targetPointList.Count < 2)
                {
                    targetPointList.Add(enemy1);
                    targetPointList.Add(player1);
                }

                Thread enemyThread = new Thread(RunAStar);
                enemyThread.IsBackground = true;
                enemyThread.Start();

                //Thread.Sleep(1000);
            }
        }

        public void RunAStar()
        {
            Astar astar = new Astar(GameWorld.Instance.Cells);
            //Enemy enemy = gameObjects[3].GetComponent<Enemy>() as Enemy;
            int index = 0;

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


                    animator.PlayAnimation("CyborgMove");
                    GameObject.Transform.Position = new Vector2(VARIABLE.Position.X * 100, VARIABLE.Position.Y * 100);
                    //var test = GameWorld.Instance.Cells[GameObject.Transform.VectorToPointConverter(GameObject.Transform.Position)];
                    Thread.Sleep(1000);

                }
                index++;
            }

            index = 0;
            startAstarBool = true;
        }

        public void GetPlayerPosition(Point playerPoint)
        {
            targetPointPos = playerPoint;
        }

    }
}

