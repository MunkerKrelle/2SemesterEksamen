using _2SemesterEksamen;
using Microsoft.Xna.Framework;
using StatePattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComponentPattern
{
    class Player : Component
    {
        private float speed;
        protected int health;
        Animator animator;
        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                if (health <= 0)
                {
                    health = 0;
                    OnDeath();
                }
            }
        }

        public Player(GameObject gameObject) : base(gameObject)
        {
        }

        public void Move(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }

            velocity *= speed;

            GameObject.Transform.CellMovement2(velocity);

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
            animator.PlayAnimation("Forward");
        }

        public override void Start()
        {
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.SetSprite("1fwd");
        }

        public override void Update(GameTime gameTime)
        {
            if (health < 0)
            {
                GameWorld.Instance.Destroy(GameObject);
            }
        }

        public override void OnCollisionEnter(Collider col)
        {
            Enemy enemy = (Enemy)col.GameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                Health -= 5;
            }

            base.OnCollisionEnter(col);
        }

        private void OnDeath()
        {
            GameWorld.Instance.Destroy(GameObject);
            GameWorld.Instance.ShowRespawnButton();
        }

        public void Respawn(Vector2 startPosition)
        {
            Health = 100;
            GameObject.Transform.Position = startPosition;
            GameWorld.Instance.Instantiate(GameObject);
        }
    }
}

