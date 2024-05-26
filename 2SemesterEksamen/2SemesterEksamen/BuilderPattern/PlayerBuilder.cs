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
    /// <summary>
    /// Bygger en spillerobjekt med de nødvendige komponenter og animationer.
    /// </summary>
    class PlayerBuilder : IBuilder
    {
        private GameObject gameObject;

        /// <summary>
        /// Opretter et nyt GameObject og bygger de nødvendige komponenter til spilleren.
        /// </summary>
        public void BuildGameObject()
        {
            gameObject = new GameObject();

            BuildComponents();
        }

        /// <summary>
        /// Tilføjer de nødvendige komponenter til GameObject.
        /// </summary>
        private void BuildComponents()
        {
            gameObject.AddComponent<Player>();
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.AddComponent<Collider>();
            Inventory inventory = gameObject.AddComponent<Inventory>();

            Animator animator = gameObject.AddComponent<Animator>();
            animator.AddAnimation(BuildAnimation("Idle", new string[] { "Player/Idle/Idle1", "Player/Idle/Idle2", "Player/Idle/Idle3", "Player/Idle/Idle4" }));
            animator.AddAnimation(BuildAnimation("Right", new string[] { "Player/Run/Run1", "Player/Run/Run2", "Player/Run/Run3", "Player/Run/Run4", "Player/Run/Run5", "Player/Run/Run6" }));
            animator.AddAnimation(BuildAnimation("Left", new string[] { "Player/Run/RunLeft1", "Player/Run/RunLeft2", "Player/Run/RunLeft3", "Player/Run/RunLeft4", "Player/Run/RunLeft5", "Player/Run/RunLeft6" }));
            animator.AddAnimation(BuildAnimation("Attack", new string[] { "Player/Attack/Attack1", "Player/Attack/Attack2", "Player/Attack/Attack3", "Player/Attack/Attack4", "Player/Attack/Attack5", "Player/Attack/Attack6" }));
        }

        /// <summary>
        /// Bygger en animation ved at indlæse de nødvendige sprites.
        /// </summary>
        /// <param name="animationName">Navnet på animationen.</param>
        /// <param name="spriteNames">En række af sprite-navne, der udgør animationen.</param>
        /// <returns>Den opbyggede animation.</returns>
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

        /// <summary>
        /// Returnerer det opbyggede GameObject.
        /// </summary>
        /// <returns>Det opbyggede GameObject.</returns>
        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
