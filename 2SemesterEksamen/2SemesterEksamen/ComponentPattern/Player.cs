using FactoryPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Opgave_08;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ComponentPattern
{
    class Player : Component
    {
        private float speed;
        Animator animator;
        public Player(GameObject gameObject) : base(gameObject)
        {
            
        }
        bool isMoving;
        public void Move(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
            }

            velocity *= speed;

            GameObject.Transform.Translate(velocity * GameWorld.Instance.DeltaTime);

            //if (velocity.X > 0)
            //{
            //    animator.PlayAnimation("Right");
            //}
            //else if (velocity.X < 0)
            //{
            //    animator.PlayAnimation("Left");
            //}
        }

        public override void Awake()
        {
            speed = 100; 
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
        bool canShoot = true;
        public void Shoot()
        {
            if (canShoot)
            {
                canShoot = false;
                lastShot = 0;
                GameObject laser = LaserFactory.Instance.Create();
                laser.Transform.Position = GameObject.Transform.Position;
                GameWorld.Instance.Instantiate(laser);
            }
        }
        float shootTimer = 1;
        float lastShot = 0;
        public override void Update(GameTime gameTime)
        {
         
            lastShot += GameWorld.Instance.DeltaTime;
            if (lastShot > shootTimer)
            {
                canShoot = true;
            }
        }

        public override void OnCollisionEnter(Collider col)
        {
            base.OnCollisionEnter(col);
        }
    }
}
