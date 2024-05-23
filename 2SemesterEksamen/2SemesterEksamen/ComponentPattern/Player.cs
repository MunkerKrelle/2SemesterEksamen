using _2SemesterEksamen;
using CommandPattern;
using FactoryPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ComponentPattern
{
    class Player : Component
    {
        private float speed;
        protected int health;
        Animator animator;
        Inventory inventory;
        public int Health
        {
            get { return health; }
            set
            {
                health = value;
                if (health <= 0)
                {
                    Die();
                }
            }
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
            inventory.weaponsList[0].GameObject.Transform.Position = GameObject.Transform.Position;
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
            inventory.weaponsList[0].GameObject.Transform.Position = GameObject.Transform.Position;
        }

        public void Attack()
        {
            Inventory inventory = GameObject.GetComponent<Inventory>() as Inventory;
            if (inventory.weaponsList.Count >= 0)
            {
                animator.PlayAnimation("Attack");
            }
            if (health <= 0)
            {
                GameWorld.Instance.Destroy(GameObject);
            }
        }

        public override void OnCollisionEnter(Collider col)
        {
            Enemy enemy = (Enemy)col.GameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.Health -= 5;
            }

            base.OnCollisionEnter(col);
        }

        private void AttackEnemy()
        {

        }

        private void Die()
        {
            //GameWorld.Instance.Destroy(GameObject);

        }

        public void Respawn()
        {
            Health = 100;
            GameObject.Transform.Position = new Vector2(300, 300);
            Player player = new Player(GameObject);
        }
    }
}
