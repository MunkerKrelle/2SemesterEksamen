using _2SemesterEksamen;
using FactoryPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ComponentPattern
{
    /// <summary>
    /// Inventory component der holder styr på hvilke våbden spilleren og armsdealeren har
    /// </summary>
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
            weaponsList.Remove(weapon);
        }

        /// <summary>
        /// Generere tilfældige våben til armsDealeren
        /// </summary>
        /// <param name="itemID">Hvilket ID våbnet har i databasen</param>
        public void GenerateRandomItem(int itemID)
        {
            Active = true;
            item = ItemFactory.Instance.Create(itemID);
            GameWorld.Instance.Instantiate(item);
            item.Transform.Scale = new Vector2(0.4f, 0.4f);
            weapon = item.GetComponent<Weapon>() as Weapon;
            weaponsList.Add(weapon);
        }

        /// <summary>
        /// Tilføj et våben til spillerens inventory
        /// </summary>
        /// <param name="itemName"></param>
        public void AddItem(string itemName)
        {
            item = ItemFactory.Instance.Create(itemName);
            GameWorld.Instance.Instantiate(item);
            item.Transform.Scale = new Vector2(0.4f, 0.4f);
            weaponsList.Add(item.GetComponent<Weapon>() as Weapon);
        }

        public override void Awake()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                for (int i = 0; i < weaponsList.Count; i++)
                {
                    spriteBatch.DrawString(GameWorld.font, $"{weaponsList[i].Name}\nDamage: {weaponsList[i].Damage}\nScraps: {weaponsList[i].Price}", new Vector2(weaponsList[i].GameObject.Transform.Position.X - 48, weaponsList[i].GameObject.Transform.Position.Y + 50), Color.Black, 0f, new Vector2 (0,0), 1f, SpriteEffects.None, 1f) ;
                }
            }
        }
    }
}
