using _2SemesterEksamen;
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
            //ChangeState();
        }

        float timeSinceLastSwitch;
        float changeTime = 1f;

        public override void Update(GameTime gameTime)
        {
            SearchForPlayer();
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

        private void SearchForPlayer()
        {
            //RUN ASTAR

            //IF DISTANCE < WHATEVER
            //{
            AttackPlayer();
            //}
        }

        private void AttackPlayer()
        {
            //HVERT TREDJE ISH SEKUND, PLAYER.HELATH - 2
        }

    }
}

