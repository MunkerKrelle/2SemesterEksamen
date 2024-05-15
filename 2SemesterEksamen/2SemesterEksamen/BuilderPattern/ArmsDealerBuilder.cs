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

        private int RandomItem()
        {
            while (true)
            {
                int tryNumber = rnd.Next(8);
                if (!list.Contains(tryNumber))
                {
                    list.Add(tryNumber);
                    return tryNumber;
                }
                else
                {
                    continue;
                }
            }
        }

        private void BuildComponents()
        {
            gameObject.AddComponent<ArmsDealer>();
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.AddComponent<Collider>();
            RandomItem();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            //inventory.AddItem("Bat");
            //inventory.AddItem("Katana");
            //inventory.AddItem("Chainsword");
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
