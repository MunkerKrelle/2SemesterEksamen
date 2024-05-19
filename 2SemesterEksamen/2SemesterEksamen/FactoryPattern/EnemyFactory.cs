using System;
using ComponentPattern;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Threading;


namespace FactoryPattern
{
    class EnemyFactory : Factory
    {
        private static EnemyFactory instance;

        private Random rnd = new Random();

        public static EnemyFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EnemyFactory();

                }

                return instance;
            }
        }

        public override GameObject Create() 
        {
            GameObject go = new GameObject();

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            go.Transform.Position = new Vector2(0, 0);
            sr.SetSprite("Robot1", 0.9f);
            go.AddComponent<Enemy>();
            Enemy meGo = (Enemy)go.GetComponent<Enemy>();
            


            Thread enemyThread = new Thread(meGo.SearchForPlayer);
            enemyThread.IsBackground = true;
            enemyThread.Start();


            return go;
        }
    }
}
