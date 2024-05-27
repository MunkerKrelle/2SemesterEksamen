using System;
using ComponentPattern;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Threading;
using _2SemesterEksamen;
using Microsoft.Xna.Framework.Graphics;


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
            sr.SetSprite("Cyborg/CyborgIdle/Cyborg_Idle1"); 
            go.Transform.Layer = 0.9f;
            go.AddComponent<Enemy>();
            go.AddComponent<Collider>();
            Animator animator = go.AddComponent<Animator>();
            animator.AddAnimation(BuildAnimation("CyborgIdle", new string[] { "Cyborg/CyborgIdle/Cyborg_Idle1", "Cyborg/CyborgIdle/Cyborg_Idle2", "Cyborg/CyborgIdle/Cyborg_Idle3", "Cyborg/CyborgIdle/Cyborg_Idle4" }));
            animator.AddAnimation(BuildAnimation("CyborgMove", new string[] { "Cyborg/CyborgMove/Cyborg_Move1", "Cyborg/CyborgMove/Cyborg_Move2", "Cyborg/CyborgMove/Cyborg_Move3", "Cyborg/CyborgMove/Cyborg_Move4", "Cyborg/CyborgMove/Cyborg_Move5", "Cyborg/CyborgMove/Cyborg_Move6" }));
            animator.AddAnimation(BuildAnimation("CyborgAttack", new string[] { "Cyborg/CyborgAttack/Cyborg_Attack1", "Cyborg/CyborgAttack/Cyborg_Attack2", "Cyborg/CyborgAttack/Cyborg_Attack3", "Cyborg/CyborgAttack/Cyborg_Attack4", "Cyborg/CyborgAttack/Cyborg_Attack5", "Cyborg/CyborgAttack/Cyborg_Attack6" }));

            //Enemy meGo = (Enemy)go.GetComponent<Enemy>();
            //Thread enemyThread = new Thread(meGo.SearchForPlayer);
            //enemyThread.IsBackground = true;
            //enemyThread.Start();

            return go;
        }
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

    }
}
