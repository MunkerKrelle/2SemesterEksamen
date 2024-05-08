using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentPattern
{
    internal class Player : Component
    {
        private float speed;

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

        }

        public void MoveByAddition(Vector2 velocity)
        {
            GameObject.Transform.Position += velocity;
        }
        
        public override void Start()
        {
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.SetSprite("SPRITE");
            GameObject.Transform.Position = new Vector2(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2, GameWorld.Instance.Graphics.PreferredBackBufferHeight - sr.Sprite.Height / 3);
        }

        public void Attack()
        {
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void OnCollisionEnter(Collider col)
        {
            base.OnCollisionEnter(col);
        }
    }
}
