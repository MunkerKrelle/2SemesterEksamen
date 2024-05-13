using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComponentPattern
{
    public class Weapon : Component
    {
        public string name;
        public int damage, price;

        public Weapon(GameObject gameObject, string name, int damage, int price) : base(gameObject)
        {
            this.name = name;
            this.damage = damage;
            this.price = price;
        }
    }
}
