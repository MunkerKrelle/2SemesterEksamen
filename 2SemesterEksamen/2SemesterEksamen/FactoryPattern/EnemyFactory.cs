using System;
using ComponentPattern;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace FactoryPattern
{
    public enum ENEMYTYPE { SLOW, FAST, WATCH }
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
        /// <summary>
        /// This method creates the guards by adding them to the new gameobject that is being made, as a component.
        /// The different Enums enable us to select which guard we want created for different purposes.
        /// We also make sure to add a SpriteRenderer to the gameobject so the guard is drawn on screen.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameObject Create(ENEMYTYPE type)
        {
            GameObject go = new GameObject();

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            go.AddComponent<Collider>();

            switch (type)
            {
                case ENEMYTYPE.SLOW:
                    sr.SetSprite("GuardFoward");
                    go.AddComponent<Enemy>(new SideToSideMovement(50, new Vector2(0, 1), go));
                    break;
                case ENEMYTYPE.FAST:
                    sr.SetSprite("GuardFoward");
                    //go.AddComponent<Enemy>(new PathMovement(100, new Vector2(0, 1), go));
                    break;
                case ENEMYTYPE.WATCH:
                    sr.SetSprite("GuardLeft");
                    //go.AddComponent<Enemy>(new LookOutStrategy(go));
                    break;
            }

            return go;
        }
        /// <summary>
        /// This is a default Create method for the factory.
        /// </summary>
        /// <returns></returns>
        public override GameObject Create()
        {
            GameObject go = new GameObject();

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.SetSprite("Guard");
            go.AddComponent<Enemy>();
            return go;
        }
    }
}
