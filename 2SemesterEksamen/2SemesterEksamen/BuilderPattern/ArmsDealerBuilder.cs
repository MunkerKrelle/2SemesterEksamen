using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ComponentPattern;
using _2SemesterEksamen;

namespace BuilderPattern
{
    /// <summary>
    /// bruger IBuilder til oprettelse af en Arms Dealer GameObject.
    /// </summary>
    class ArmsDealerBuilder : IBuilder
    {
        private GameObject gameObject;
        private Random rnd = new Random();
        private List<int> list = new List<int>() { 0 };

        /// <summary>
        /// Metode til at opbygge et Arms Dealer GameObject.
        /// </summary>
        public void BuildGameObject()
        {
            gameObject = new GameObject();

            BuildComponents();
        }

        /// <summary>
        /// Genererer et tilfældigt tal, der repræsenterer en genstand.
        /// </summary>
        /// <returns>Et tilfældigt tal, der repræsenterer en genstand.</returns>
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

        /// <summary>
        /// Metode til at opbygge de nødvendige komponenter til Arms Dealer GameObject.
        /// </summary>
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
            // Animator animator = gameObject.AddComponent<Animator>();

        }

        /// <summary>
        /// Metode til at få det færdigbyggede Arms Dealer GameObject.
        /// </summary>
        /// <returns>Det færdigbyggede Arms Dealer GameObject.</returns>
        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}

