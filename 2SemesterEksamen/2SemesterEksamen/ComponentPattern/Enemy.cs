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
        private bool startAstarBool = true;
        private Vector2 targetPos;

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
            

            if (startAstarBool == true)
            {
                Point player1 = new Point(1, 1);
                Point player2 = new Point(5, 5);
                Point enemy1 = new Point(3, 7);
                Point enemy2 = new Point(2, 6);
                Point enemy3 = new Point(8, 9);
                Point enemy4 = new Point(10, 10);

                GameWorld.targetPointList.Add(player1);
                GameWorld.targetPointList.Add(player2);

                GameWorld.targetPointList.Add(enemy1);
                GameWorld.targetPointList.Add(enemy2);
                GameWorld.targetPointList.Add(enemy3);
                GameWorld.targetPointList.Add(enemy4);

                startAstarBool = false;
                GameWorld.Instance.RunAStar();
            }
            //IF DISTANCE < WHATEVER
            //{
            AttackPlayer();
            //}
        }

        private void AttackPlayer()
        {
            //HVERT TREDJE ISH SEKUND, PLAYER.HELATH - 2
        }

        public void GetPlayerPosition(Vector2 playerPos) 
        {
            targetPos = playerPos;
        }

    }
}

