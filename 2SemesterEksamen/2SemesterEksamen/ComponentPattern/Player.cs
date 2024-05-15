using _2SemesterEksamen;
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
        public int health;
        Animator animator;
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

            GameObject.Transform.Translate(velocity * GameWorld.Instance.DeltaTime);

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
            speed = 200;
            health = 100;
            animator = GameObject.GetComponent<Animator>() as Animator;
            animator.PlayAnimation("Forward");
        }
        public void MoveByAddition(Vector2 velocity)
        {
            GameObject.Transform.Position += velocity;
        }
        public override void Start()
        {
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.SetSprite("1fwd");
            GameObject.Transform.Position = new Vector2(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2, GameWorld.Instance.Graphics.PreferredBackBufferHeight - sr.Sprite.Height / 3);

        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void OnCollisionEnter(Collider col)
        {
            Enemy enemy = (Enemy)col.GameObject.GetComponent<Enemy>(); 

            if (enemy != null)
            {
                enemy.Health -= 1; 
            }
            if (enemy.Health < 0)
            {
                
            }

            base.OnCollisionEnter(col);
        }

        private void AttackEnemy()
        {

        }
    }
}
