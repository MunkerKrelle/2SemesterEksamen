using _2SemesterEksamen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RepositoryPattern;
using System.Security.Cryptography.Xml;
using System.Threading;

namespace ComponentPattern
{
    /// <summary>
    /// Repræsenterer spillerkomponenten, der styrer spillerens bevægelser, animationer og interaktioner.
    /// </summary>
    class Player : Component
    {
        private float speed;
        protected int health;
        public int damage;
        private int currentInvSlot;
        Animator animator;
        Inventory inventory;
        Database database = new Database();

        bool isAlive = true;

        /// <summary>
        /// Får eller sætter spillerens sundhed. Hvis sundheden når 0 eller derunder, vil spilleren dø.
        /// </summary>
        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                if (health <= 0 && isAlive)
                {
                    isAlive = false;
                    Die();
                }
            }
        }

        /// <summary>
        /// Initialiserer en ny instans af <see cref="Player"/> klassen med det specificerede game object.
        /// </summary>
        /// <param name="gameObject">Det game object, som spilleren er knyttet til.</param>
        public Player(GameObject gameObject) : base(gameObject)
        {

        }

        bool isMoving;

        /// <summary>
        /// Flytter spilleren i henhold til den angivne hastighedsvektor.
        /// </summary>
        /// <param name="velocity">Hastighedsvektoren, som spilleren skal flytte sig med.</param>
        public void Move(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
                isMoving = true;
            }

            velocity *= speed;

            GameObject.Transform.PlayerPointMove(velocity);

            if (velocity.X > 0)
            {
                animator.PlayAnimation("PlayerMove");
                GameObject.Transform.SpriteEffect = SpriteEffects.None;
                if (inventory.weaponsList.Count > 0)
                {
                    inventory.weaponsList[currentInvSlot].GameObject.Transform.SpriteEffect = SpriteEffects.None;
                }
            }
            else if (velocity.X < 0)
            {
                animator.PlayAnimation("PlayerMove");
                GameObject.Transform.SpriteEffect = SpriteEffects.FlipHorizontally;
                if (inventory.weaponsList.Count > 0)
                {
                    inventory.weaponsList[currentInvSlot].GameObject.Transform.SpriteEffect = SpriteEffects.FlipHorizontally;
                }
            }
        }

        /// <summary>
        /// Initialiserer spillerens egenskaber, når komponenten vågner.
        /// </summary>
        public override void Awake()
        {
            speed = 100;
            health = 100;
            animator = GameObject.GetComponent<Animator>() as Animator;
            animator.PlayAnimation("Idle");
            GameObject.Transform.Scale = new Vector2(3f, 3f);
            inventory = GameObject.GetComponent<Inventory>() as Inventory;
            inventory.Active = true;        }

        /// <summary>
        /// Flytter spilleren ved at tilføje en vektor til spillerens nuværende position.
        /// </summary>
        /// <param name="velocity">Vektoren, som skal tilføjes til spillerens position.</param>
        public void MoveByAddition(Vector2 velocity)
        {
            GameObject.Transform.Position += velocity;
        }

        /// <summary>
        /// Initialiserer spillerens startposition og grafiske elementer.
        /// </summary>
        public override void Start()
        {
            GameObject.Transform.Position = new Vector2(300, 300);
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.SetSprite("Player/Idle/Idle1");
            GameObject.Transform.Layer = 0.9f;
        }

        /// <summary>
        /// Opdaterer spillerens tilstand i hver frame.
        /// </summary>
        /// <param name="gameTime">Spillets tid, der er gået siden sidste opdatering.</param>
        public override void Update(GameTime gameTime)
        {
            if (currentInvSlot >= inventory.weaponsList.Count)
            {
                currentInvSlot = 0;
            }

            if (inventory.weaponsList.Count != 0 && animator.currentAnimation.Name != "Attack")
            {
                inventory.weaponsList[currentInvSlot].GameObject.Transform.Layer = 0.5f;
                inventory.weaponsList[currentInvSlot].GameObject.Transform.Position = GameObject.Transform.Position;
            }
            else if (inventory.weaponsList.Count != 0 && animator.currentAnimation.Name == "Attack")
            {
                inventory.weaponsList[currentInvSlot].GameObject.Transform.Layer = 1f;
                if (inventory.weaponsList[currentInvSlot].GameObject.Transform.SpriteEffect == SpriteEffects.None)
                {
                    inventory.weaponsList[currentInvSlot].GameObject.Transform.Position = new Vector2(GameObject.Transform.Position.X + (animator.CurrentIndex * 8), inventory.weaponsList[currentInvSlot].GameObject.Transform.Position.Y);
                }
                else
                {
                    inventory.weaponsList[currentInvSlot].GameObject.Transform.Position = new Vector2(GameObject.Transform.Position.X - (animator.CurrentIndex * 8), inventory.weaponsList[currentInvSlot].GameObject.Transform.Position.Y);
                }
            }

            if (Database.playerItemsUpdated == true)
            {
                //if (inventory.weaponsList.Count > 0)
                //{
                //   // GameWorld.Instance.Destroy(inventory.weaponsList[currentInvSlot].GameObject);
                //    //inventory.weaponsList.RemoveAt(currentInvSlot);
                //    if (currentInvSlot > inventory.weaponsList.Count)
                //    {
                //        currentInvSlot = 0;
                //    }
                //}
                inventory.AddItem(database.AddToInventory());
                Database.playerItemsUpdated = false;

            }
        }

        public void ChangeItem()
        {
            foreach (var item in inventory.weaponsList)
            {
                item.GameObject.Transform.Position = new Vector2(-100, -100);
            }
            currentInvSlot++;
        }

        /// <summary>
        /// Udfører spillerens angrebsanimation.
        /// </summary>
        public void Attack()
        {
            animator.PlayAnimation("Attack");
            if (inventory.weaponsList.Count > 0)
            {
                damage = 1 + inventory.weaponsList[currentInvSlot].Damage;
            }
            else
            {
                damage = 1;
            }
        }

        /// <summary>
        /// Håndterer kollisioner med andre objekter.
        /// </summary>
        /// <param name="col">Kollisionen, som spilleren er involveret i.</param>
        public override void OnCollisionEnter(Collider col)
        {

            Enemy enemy = (Enemy)col.GameObject.GetComponent<Enemy>();

            if (enemy != null && animator.currentAnimation.Name == "Attack" && animator.CurrentIndex < 3)
            {
                enemy.Health -= damage;
            }
            else if (enemy != null)
            {
                health -= 5;
            }

            base.OnCollisionEnter(col);
        }

        /// <summary>
        /// Udfører angrebet på fjender (placeholder-metode).
        /// </summary>
        private void AttackEnemy()
        {

        }

        /// <summary>
        /// Håndterer spillerens død ved at vise en respawn-knap og destruere spillerens game object.
        /// </summary>
        private void Die()
        {
            //GameWorld.Instance.CreateRespawnButton(); 
            //GameWorld.Instance.Destroy(GameObject);  
        }

        /// <summary>
        /// Genopliver spilleren ved at genoprette helbred og position.
        /// </summary>
        public void Respawn()
        {
            Health = 100;
            GameObject.Transform.Position = new Vector2(300, 300);
            GameWorld.Instance.Instantiate(GameObject);
        }
    }
}
