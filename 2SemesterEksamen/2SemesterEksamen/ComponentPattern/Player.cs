using _2SemesterEksamen;
using Microsoft.Xna.Framework;
using RepositoryPattern;
using System.Security.Policy;

namespace ComponentPattern
{
    class Player : Component
    {
        private float speed;
        protected int health;
        public int damage;
        Animator animator;
        Inventory inventory;
        Database database = new Database();
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Player(GameObject gameObject) : base(gameObject)
        {

        }

        bool isMoving;

        public void Move(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
                isMoving = true;
            }

            velocity *= speed;

            //GameObject.Transform.Translate(velocity * GameWorld.Instance.DeltaTime);
            GameObject.Transform.PlayerPointMove(velocity);

            if (velocity.X > 0)
            {
                animator.PlayAnimation("Right");
            }
            else if (velocity.X < 0)
            {
                animator.PlayAnimation("Left");
            }
        }

        public override void Awake()
        {
            speed = 10;
            health = 100;
            animator = GameObject.GetComponent<Animator>() as Animator;
            animator.PlayAnimation("Idle");
            GameObject.Transform.Scale = new Vector2(3f, 3f);
            inventory = GameObject.GetComponent<Inventory>() as Inventory;
            inventory.Active = true;
            //inventory.weaponsList[0].GameObject.Transform.Position = GameObject.Transform.Position;
        }

        public void MoveByAddition(Vector2 velocity)
        {
            GameObject.Transform.Position += velocity;
        }

        public override void Start()
        {
            GameObject.Transform.Position = new Vector2(300, 300);
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.SetSprite("Player/Idle/Idle1");
            GameObject.Transform.Layer = 0.7f;
        }

        public override void Update(GameTime gameTime)
        {
            if (inventory.weaponsList.Count != 0)
            {
                inventory.weaponsList[0].GameObject.Transform.Position = GameObject.Transform.Position;
            }

            if (Database.playerItemsUpdated == true)
            {
                inventory.AddItem(database.AddToInventory());
                Database.playerItemsUpdated = false;

            }
        }

        public void Attack()
        {
            animator.PlayAnimation("Attack");
            if (inventory.weaponsList.Count > 0)
            {
                damage = 1 + inventory.weaponsList[0].Damage;
            }
            else
            {
                damage = 1;
            }
        }

        public void Buy(Player player)
        {
            database.TradeWeapon(this.inventory.weaponsList[1]);
            GameWorld.Instance.Destroy(this.inventory.weaponsList[0].button);
            GameWorld.Instance.Destroy(this.inventory.weaponsList[0].GameObject);
            this.inventory.AddItem(inventory.weaponsList[0].Name);
        }

        public override void OnCollisionEnter(Collider col)
        {

            Enemy enemy = (Enemy)col.GameObject.GetComponent<Enemy>();

            if (enemy != null && animator.currentAnimation.Name == "Attack" && animator.CurrentIndex < 3)
            {
                enemy.Health -= damage;
            } else if (enemy != null)
                {
                health -= 5;
            }

            base.OnCollisionEnter(col);
        }

        private void AttackEnemy()
        {

        }
    }
}
