using _2SemesterEksamen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StatePattern;
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
        public Animator animator;

        public Vector2 velocity = new Vector2(0, 1);
        static readonly object DamagePlayerLock = new object();
        public Enemy(GameObject gameObject) : base(gameObject)
        {
            //ChangeState();
        }

        float timeSinceLastSwitch;
        float changeTime = 1f;

        public override void Awake()
        {
            Health = 100;
            GameObject.Transform.Scale = new Vector2(1f, 1f);
            animator = GameObject.GetComponent<Animator>() as Animator;
            //animator.PlayAnimation("CyborgIdle");
        }
        public override void Start()
        {
            GameObject.Transform.Position = new Vector2(500, 500);
        }

        public override void Update(GameTime gameTime)
        {
            //if (Player is dead)
            //{
            //    ChangeState(new IdleState());
            //}
            //else if (Player is not close)
            //{
            //    ChangeState(new MoveState());
            //}
            //else if (Player is close)
            //{
            //    ChangeState(new AttackState());
            //}

            enemyTimer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timeElapsed2 += enemyTimer;

            if (timeElapsed2 >= 1f)
            {
                SearchForPlayer();
                timeElapsed2 = 0;
            }

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

            if (startAstarBool == true)
            {
                startAstarBool = false;
                EnemyPointPosition = GameObject.Transform.VectorToPointConverter(GameObject.Transform.Position);
                //Thread.Sleep(100);
                GetPlayerPosition(GameWorld.Instance.GameObjects[0].Transform.VectorToPointConverter(GameWorld.Instance.GameObjects[0].Transform.Position));
                Point enemy1 = new Point(EnemyPointPosition.X, EnemyPointPosition.Y);
                Point player1 = new Point(targetPointPos.X, targetPointPos.Y);

                if (GameWorld.Instance.targetPointList.Count == 2)
                {
                    GameWorld.Instance.targetPointList.Clear();
                }

                if (GameWorld.Instance.targetPointList.Count < 2) 
                {
                    GameWorld.Instance.targetPointList.Add(enemy1);
                    GameWorld.Instance.targetPointList.Add(player1);
                }

                Thread enemyThread = new Thread(GameWorld.Instance.RunAStar);
                enemyThread.IsBackground = true;
                enemyThread.Start();

                //Thread.Sleep(1000);
            }
            //IF DISTANCE < WHATEVER
            //{
            AttackPlayer();
            //}
        }

        private void AttackPlayer()
        {
            //HVERT TREDJE ISH SEKUND, PLAYER.HEALTH - 2
            lock (DamagePlayerLock)
            {
               //insert attack code here to remove health.
            }
        }

        public void GetPlayerPosition(Point playerPoint)
        {
            targetPointPos = playerPoint;
        }

    }
}

