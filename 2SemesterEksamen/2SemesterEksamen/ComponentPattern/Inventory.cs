using _2SemesterEksamen;
using CommandPattern;
using FactoryPattern;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentPattern
{
    public class Inventory : Component
    {
        public List<GameObject> gameInventory = new List<GameObject>();
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
            item.Transform.Position = new Vector2(GameObject.Transform.Position.X, GameObject.Transform.Position.Y);
            item.Transform.Scale = new Vector2(10,10);
            gameInventory.Add(item);
        }

        public override void Awake()
        {
        }

        public void SellItem(Weapon weapon) { }
    }
}
