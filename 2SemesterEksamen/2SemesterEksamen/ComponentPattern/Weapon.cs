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
        public string name;
        public int damage, price;

        public Weapon(GameObject gameObject, int damage, int price) : base(gameObject)
        {
            this.damage = damage;
            this.price = price;
        }
        //public Weapon(GameObject gameObject) : base(gameObject)
        //{ }

        public override void Update(GameTime gameTime)
        {
            GameObject.Transform.Position = new Vector2(500, 500);
        }
    }
}
