using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2SemesterEksamen;
using Microsoft.Xna.Framework;

namespace ComponentPattern
{
    public class Weapon : Component
    {
        private string name;
        private int damage, price;
        public string Name { get { return name; } }
        public int Damage { get { return damage; } }
        public int Price {  get { return price; } }

        public Weapon(GameObject gameObject, string name, int damage, int price) : base(gameObject)
        {
            this.name = name;
            this.damage = damage;
            this.price = price;
        }

        public override void Update(GameTime gameTime)
        {
            //GameObject.Transform.Position = new Vector2(500, 500);
        }
    }
}
