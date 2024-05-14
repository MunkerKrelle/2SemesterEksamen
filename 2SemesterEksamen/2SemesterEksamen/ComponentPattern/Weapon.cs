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
        public string damage, price;
        public int damgeValue;

        public Weapon(GameObject gameObject) : base(gameObject)
        {
            this.damgeValue = damgeValue;
        }
        //public Weapon(GameObject gameObject) : base(gameObject)
        //{ }

        public void Update()
        {
            GameObject.Transform.Position = new Vector2(500, 500);
        }
    }
}
