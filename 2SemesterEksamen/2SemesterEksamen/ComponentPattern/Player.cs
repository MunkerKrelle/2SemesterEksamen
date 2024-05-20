using _2SemesterEksamen;
using CommandPattern;
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
        Animator animator;
        Inventory inventory;
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
            speed = 400;
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
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.SetSprite("Player/Idle/Idle1");
            GameObject.Transform.Position = new Vector2(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2, GameWorld.Instance.Graphics.PreferredBackBufferHeight - sr.Sprite.Height / 3);

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
        }

        public override void OnCollisionEnter(Collider col)
        {
            base.OnCollisionEnter(col);
        }
    }
}
