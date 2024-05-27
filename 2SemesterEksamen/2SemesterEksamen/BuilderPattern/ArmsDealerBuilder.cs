using ComponentPattern;
using System;
using System.Collections.Generic;

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

        private void BuildComponents()
        {
            gameObject.AddComponent<ArmsDealer>();
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.AddComponent<Collider>();
            Inventory inventory = gameObject.AddComponent<Inventory>();
        }

        //}
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

