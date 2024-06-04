using _2SemesterEksamen;
using ComponentPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FactoryPattern
{
    /// <summary>
    /// Fabrik til af lave specifikke fjender, hvis vi også vil have forskellige slags fjender
    /// </summary>
    class EnemyFactory : Factory
    {
        public enum TYPE { ORIGNIAL, NEW }

        //public bool enemyOriginal;

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
        /// Bygger enemies med sprites, animationer og collider
        /// </summary>
        /// <returns></returns>
        public GameObject Create(TYPE type)
        {
            GameObject go = new GameObject();

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();

            switch (type)
            {

                case TYPE.ORIGNIAL:

                    //enemyOriginal = true;
                    go.Transform.Position = new Vector2(0, 0);
                    sr.SetSprite("Cyborg/CyborgIdle/Cyborg_Idle1");
                    go.Transform.Layer = 0.9f;
                    go.AddComponent<Enemy>();
                    go.AddComponent<Collider>();
                    Animator animator = go.AddComponent<Animator>();
                    animator.AddAnimation(BuildAnimation("CyborgIdle", new string[] { "Cyborg/CyborgIdle/Cyborg_Idle1", "Cyborg/CyborgIdle/Cyborg_Idle2", "Cyborg/CyborgIdle/Cyborg_Idle3", "Cyborg/CyborgIdle/Cyborg_Idle4" }));
                    animator.AddAnimation(BuildAnimation("CyborgMove", new string[] { "Cyborg/CyborgMove/Cyborg_Move1", "Cyborg/CyborgMove/Cyborg_Move2", "Cyborg/CyborgMove/Cyborg_Move3", "Cyborg/CyborgMove/Cyborg_Move4", "Cyborg/CyborgMove/Cyborg_Move5", "Cyborg/CyborgMove/Cyborg_Move6" }));
                    animator.AddAnimation(BuildAnimation("CyborgAttack", new string[] { "Cyborg/CyborgAttack/Cyborg_Attack1", "Cyborg/CyborgAttack/Cyborg_Attack2", "Cyborg/CyborgAttack/Cyborg_Attack3", "Cyborg/CyborgAttack/Cyborg_Attack4", "Cyborg/CyborgAttack/Cyborg_Attack5", "Cyborg/CyborgAttack/Cyborg_Attack6" }));
                    break;

                case TYPE.NEW:

                    //enemyOriginal = false;
                    go.Transform.Position = new Vector2(100, 100);
                    sr.SetSprite("robot");
                    go.Transform.Layer = 0.9f;
                    go.AddComponent<NewEnemy>();
                    go.AddComponent<Collider>();
                    Animator animator2 = go.AddComponent<Animator>();
                    animator2.AddAnimation(BuildAnimation("CyborgIdle", new string[] { "robot" }));
                    animator2.AddAnimation(BuildAnimation("CyborgMove", new string[] { "robot" }));
                    animator2.AddAnimation(BuildAnimation("CyborgAttack", new string[] { "robot" }));
                    break;

            }
            return go;
        }


        /// <summary>
        /// Bygger animationer, som enemies kan kører
        /// </summary>
        /// <param name="animationName">Navnet på animationen, så man nemt kan genbruge den</param>
        /// <param name="spriteNames">Hvilke sprites animation skal indeholde</param>
        /// <returns></returns>
        private Animation BuildAnimation(string animationName, string[] spriteNames)
        {
            Texture2D[] sprites = new Texture2D[spriteNames.Length];

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = GameWorld.Instance.Content.Load<Texture2D>(spriteNames[i]);
            }

            Animation animation = new Animation(animationName, sprites, 5);

            return animation;
        }

        public override GameObject Create()
        {
            return null;
        }
    }
}
