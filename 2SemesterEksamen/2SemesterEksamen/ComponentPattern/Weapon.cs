using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2SemesterEksamen;
using FactoryPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RepositoryPattern;

namespace ComponentPattern
{
    public class Weapon : Component
    {
        public GameObject button;
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

        public override void Awake()
        {
            button = ButtonFactory.Instance.Create(GameObject.Transform.Position, Name, () => Sell());
            GameWorld.Instance.Instantiate(button);
            button.Transform.Scale = new Vector2(0.2f, 0.4f);
            button.Transform.Color = Color.Black;
        }

        public void Sell()
        {
            GameWorld.Instance.Destroy(button);
            GameWorld.Instance.Destroy(GameObject);
        }

        public override void Update(GameTime gameTime)
        {
            //GameObject.Transform.Position = new Vector2(500, 500);
        }
    }
}
