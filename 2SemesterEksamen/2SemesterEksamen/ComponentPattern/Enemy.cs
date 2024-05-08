using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentPattern
{
    internal class Enemy : Component
    {
        public float speed = 1;

        public Vector2 velocity = new Vector2(0, 1);
        public Enemy(GameObject gameObject) : base(gameObject)
        {
        }

        float timeSinceLastSwitch;
        float changeTime = 1f;

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void OnCollisionEnter(Collider col)
        {

        }

        //Istate<Enemy> currentState;

        //public void ChangeState(Istate<Enemy> state)
        //{
        //    if (currentState != null)
        //    {
        //        currentState.Exit();
        //    }
        //    currentState = state;
        //    currentState.Enter(this);
        //}
    }
}
