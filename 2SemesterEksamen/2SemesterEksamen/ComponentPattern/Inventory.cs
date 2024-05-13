using CommandPattern;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentPattern
{
    public class Inventory : Component
    {
        public List<Weapon> gameInventory = new List<Weapon>();

        public Inventory(GameObject gameObject) : base(gameObject)
        {
        }

        public void RemoveItem(Weapon weapon)
        {

        }

        public void AddItem(Weapon weapon) { }

        public void SellItem(Weapon weapon) { }
    }
}
