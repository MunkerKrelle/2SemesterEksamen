using _2SemesterEksamen;
using CommandPattern;
using FactoryPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using RepositoryPattern;
using SharpDX.Win32;
using System;

namespace ComponentPattern
{
    public class Inventory : Component
    {
        public GameObject item;
        public List<Weapon> weaponsList = new List<Weapon>();
        public Weapon weapon;

        public bool Active { get; set; }

        public Inventory(GameObject gameObject) : base(gameObject)
        {
            Active = false;
        }

        public void RemoveItem(Weapon weapon)
        {

        }

        public void GenerateItem(int itemID)
        {
            Active = true;
            item = ItemFactory.Instance.Create(itemID);
            GameWorld.Instance.Instantiate(item);
            item.Transform.Scale = new Vector2(0.4f, 0.4f);
            item.Transform.Position = new Vector2(430 + 100 * weaponsList.Count, 280);
            weapon = item.GetComponent<Weapon>() as Weapon;
            weaponsList.Add(weapon);


        }

        public void AddItem(string itemName)
        {
            item = ItemFactory.Instance.Create(itemName);
            GameWorld.Instance.Instantiate(item);
            item.Transform.Scale = new Vector2(0.4f, 0.4f);
            item.Transform.Position = new Vector2(630 + 100 * weaponsList.Count, 280);
            weaponsList.Add(item.GetComponent<Weapon>() as Weapon);
        }

        public override void Awake()
        {
        }

        public void SellItem(Weapon weapon) { }


        public void LoadItems()
        {
            if (Active)
            {
                for (int i = 0; i < weaponsList.Count; i++)
                {
                    weaponsList[i].GameObject.Transform.Position = new Vector2(-1000, -1000);
                }
            }
            else
            {
                for (int i = 0; i < weaponsList.Count; i++)
                {

                    weaponsList[i].GameObject.Transform.Position = new Vector2(430 + 100 * i, 280);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                SpriteFont font = GameWorld.Instance.Content.Load<SpriteFont>("text2");
                for (int i = 0; i < weaponsList.Count; i++)
                {
                    spriteBatch.DrawString(font, $"{weaponsList[i].Name}\nDamage: {weaponsList[i].Damage}\nScraps: {weaponsList[i].Price}", new Vector2(weaponsList[i].GameObject.Transform.Position.X - 50, weaponsList[i].GameObject.Transform.Position.Y + 50), Color.White);

                }
            }
        }
    }
}
