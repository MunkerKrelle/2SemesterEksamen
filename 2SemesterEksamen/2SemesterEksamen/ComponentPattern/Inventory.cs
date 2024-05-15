using _2SemesterEksamen;
using CommandPattern;
using FactoryPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using RepositoryPattern;
using SharpDX.Win32;

namespace ComponentPattern
{
    public class Inventory : Component
    {
        private UserRegistrationWithPattern database = new UserRegistrationWithPattern();
        public List<string> gameInventory = new List<string>();
        public Dictionary<string, GameObject> dicInventory = new Dictionary<string, GameObject>();
        public GameObject item;
        public string name;
        public int damage;
        public int price;

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
            var itemValues = database.ReturnValues(itemName);
            name = itemValues[0].Item1;
            damage = itemValues[0].Item2;
            price = itemValues[0].Item3;
        }

        public override void Awake()
        {
        }

        public void SellItem(Weapon weapon) { }


        public void LoadItems()
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont font = GameWorld.Instance.Content.Load<SpriteFont>("text2");
            for (int i = 0; i < gameInventory.Count; i++) 
            {
                spriteBatch.DrawString(font, $"{name}\nDamage: {damage}\nScraps: {price}", new Vector2(500 + 100 * i, 250), Color.White);

            }
        }
    }
}
