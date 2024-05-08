using Microsoft.Xna.Framework;
using StatePattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComponentPattern
{
    public class Enemy : Component
    {
        public float speed = 1;

        public Vector2 velocity = new Vector2(0, 1);
        public Enemy(GameObject gameObject) : base(gameObject)
        {
            ChangeState();
        }       

        float timeSinceLastSwitch;
        float changeTime = 1f;

        public override void Update(GameTime gameTime)
        {
            timeSinceLastSwitch += GameWorld.Instance.DeltaTime;
            if (timeSinceLastSwitch >= changeTime)
            {
                if (currentState is AttackState)
                {
                    ChangeState();
                }
                else
                {
                    ChangeState();
                }
                timeSinceLastSwitch = 0;
            }
            currentState.Execute();
            if (GameObject.Transform.Position.Y > GameWorld.Instance.GraphicsDevice.Viewport.Height)
            {
                //EnemyPool.Instance.ReleaseObject(GameObject);
            }
        }

        public override void OnCollisionEnter(Collider col)
        {
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
    }
}

