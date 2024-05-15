using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ComponentPattern;
using _2SemesterEksamen;
using Microsoft.Xna.Framework;
namespace BuilderPattern
{
    class PlayerBuilder : IBuilder
    {
        private GameObject gameObject;

        public void BuildGameObject()
        {
            gameObject = new GameObject();

            BuildComponents();
        }

        private void BuildComponents()
        {
            gameObject.AddComponent<Player>();
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.AddComponent<Collider>();
            Inventory inventory = gameObject.AddComponent<Inventory>();
            inventory.AddItem("Wrench");
            inventory.AddItem("SteelBat");
            Animator animator = gameObject.AddComponent<Animator>();
            animator.AddAnimation(BuildAnimation("Forward", new string[] { "1fwd", "2fwd", "3fwd" }));
            animator.AddAnimation(BuildAnimation("Right", new string[] { "1rght", "2rght", "3rght" }));
            animator.AddAnimation(BuildAnimation("Left", new string[] { "1lft", "2lft", "3lft" }));

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
        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
