using _2SemesterEksamen;
using CommandPattern;
using FactoryPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using RepositoryPattern;

namespace ComponentPattern
{
    public class Inventory : Component
    {
        private UserRegistrationWithPattern database = new UserRegistrationWithPattern();
        public List<string> gameInventory = new List<string>();
        public Dictionary<string, GameObject> dicInventory = new Dictionary<string, GameObject>();
        public GameObject item;

        public Inventory(GameObject gameObject) : base(gameObject)
        {
        }

        public void RemoveItem(Weapon weapon)
        {

        }

        public void AddItem(string itemName)
        {
            item = ItemFactory.Instance.Create(itemName);
            GameWorld.Instance.Instantiate(item);
            item.Transform.Position = new Vector2(500 + 50 * gameInventory.Count, 200);
            gameInventory.Add(itemName);
        }

        public override void Awake()
        {
        }

        public void SellItem(Weapon weapon) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont font = GameWorld.Instance.Content.Load<SpriteFont>("text2");
            for (int i = 0; i < gameInventory.Count; i++) 
            {
                var wrenchValues = database.ReturnValues(dicInventory[]);
                spriteBatch.DrawString(font, $"{wrenchValues[0].Item1}", new Vector2(450 + 50 * i, 250), Color.White);
                spriteBatch.DrawString(font, $"{wrenchValues[0].Item2}", new Vector2(450 + 50 * i, 300), Color.White);
                spriteBatch.DrawString(font, $"{wrenchValues[0].Item3}", new Vector2(450 + 50 * i, 350), Color.White);
            }
        }
    }
}
