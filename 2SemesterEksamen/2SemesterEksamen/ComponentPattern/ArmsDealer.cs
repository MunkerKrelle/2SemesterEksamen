using _2SemesterEksamen;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ComponentPattern
{
    public class ArmsDealer : Component
    {
        public Inventory inventory;
        private Random rnd = new Random();
        private List<int> list = new List<int>() { 0 };

        public ArmsDealer(GameObject gameObject) : base(gameObject)
        {

        }

        /// <summary>
        /// Generere et tilfældigt til, som ikke er blevet dannet endnu
        /// </summary>
        /// <returns></returns>
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

        public override void Awake()
        {
            GameObject.IsActive = true;
            inventory = GameObject.GetComponent<Inventory>() as Inventory;
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            inventory.Active = true;
            for (int i = 0; i < inventory.weaponsList.Count; i++)
            {
                inventory.weaponsList[i].GameObject.Transform.Position = new Vector2(400 + i * 100, 500);
                inventory.weaponsList[i].CreateButtons();
            }
        }

        public override void Update(GameTime gametime)
        {
            if (GameObject.IsActive == true)
            {
                foreach (var i in inventory.weaponsList)
                {
                    i.GameObject.IsActive = false;
                    i.button.IsActive = false;
                }
            }
            if (GameObject.IsActive == false)
            {
                foreach (var i in inventory.weaponsList)
                {
                    i.GameObject.IsActive = true;
                    i.button.IsActive = true;
                }
            }
        }
        /// <summary>
        /// Generere 4 nye våben, som shopkeeper kan sælge
        /// </summary>
        public void UpdateItems()
        {
            foreach (var item in inventory.weaponsList)
            {
                GameWorld.Instance.Destroy(item.button);
                GameWorld.Instance.Destroy(item.GameObject);
            }

            inventory.weaponsList.Clear();
            list = new List<int>() { 0 };

            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());

            for (int i = 0; i < inventory.weaponsList.Count; i++)
            {
                inventory.weaponsList[i].GameObject.Transform.Position = new Vector2(400 + i * 100, 500);
                inventory.weaponsList[i].CreateButtons();
            }
        }

        public override void Start()
        {
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.SetSprite("ShopKeeper");
            GameObject.Transform.Transformer(new Vector2(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2, sr.Sprite.Height * 0.4f), 0f, new Vector2(0.4f, 0.4f), Color.White, 0.8f);
        }

        public override void OnCollisionEnter(Collider col)
        {
            base.OnCollisionEnter(col);
        }
    }
}
