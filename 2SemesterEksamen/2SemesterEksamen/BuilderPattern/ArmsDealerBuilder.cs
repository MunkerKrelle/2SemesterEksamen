using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ComponentPattern;
using _2SemesterEksamen;
namespace BuilderPattern
{
    class ArmsDealerBuilder : IBuilder
    {
        private GameObject gameObject;
        private Random rnd = new Random();
        private List<int> list = new List<int>() { 0 };

        public void BuildGameObject()
        {
            gameObject = new GameObject();

            BuildComponents();
        }

        private void BuildComponents()
        {
            gameObject.AddComponent<ArmsDealer>(); 
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.AddComponent<Collider>();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            // Animator animator = gameObject.AddComponent<Animator>();

        }
        //private Animation BuildAnimation(string animationName, string[] spriteNames)
        //{
        //    Texture2D[] sprites = new Texture2D[spriteNames.Length];

        //    for (int i = 0; i < sprites.Length; i++)
        //    {
        //        sprites[i] = GameWorld.Instance.Content.Load<Texture2D>(spriteNames[i]);
        //    }

        //    Animation animation = new Animation(animationName, sprites, 5);

        //    return animation;
        //}



        public GameObject GetResult()
        {
            return gameObject;
        }
    }

}
