using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ComponentPattern;
using _2SemesterEksamen;
using Microsoft.Xna.Framework;
using CommandPattern;
using Microsoft.Xna.Framework.Input;
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
            inventory.AddItem("Butterflyknife");
            //inventory.AddItem("Bat");
            //inventory.AddItem("Katana");
            //inventory.AddItem("Chainsword");
            Animator animator = gameObject.AddComponent<Animator>();
            InputHandler.Instance.AddUpdateCommand(Keys.I, new InventoryCommand(inventory));
            animator.AddAnimation(BuildAnimation("Idle", new string[] { "Player/Idle/Idle1", "Player/Idle/Idle2", "Player/Idle/Idle3", "Player/Idle/Idle4" }));
            animator.AddAnimation(BuildAnimation("Right", new string[] { "Player/Run/Run1", "Player/Run/Run2", "Player/Run/Run3", "Player/Run/Run4", "Player/Run/Run5", "Player/Run/Run6" }));
            animator.AddAnimation(BuildAnimation("Left", new string[] { "Player/Run/RunLeft1", "Player/Run/RunLeft2", "Player/Run/RunLeft3", "Player/Run/RunLeft4", "Player/Run/RunLeft5", "Player/Run/RunLeft6" }));
            animator.AddAnimation(BuildAnimation("Attack", new string[] { "Player/Attack/Attack1", "Player/Attack/Attack2", "Player/Attack/Attack3", "Player/Attack/Attack4", "Player/Attack/Attack5", "Player/Attack/Attack6" }));

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
