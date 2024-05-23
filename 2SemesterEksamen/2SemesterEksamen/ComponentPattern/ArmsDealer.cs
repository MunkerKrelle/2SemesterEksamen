using _2SemesterEksamen;
using FactoryPattern;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentPattern
{
    public class ArmsDealer : Component
    {
        public Inventory inventory;

        public ArmsDealer(GameObject gameObject) : base(gameObject)
        {

        }

        public override void Awake()
        {
            inventory = GameObject.GetComponent<Inventory>() as Inventory;
            inventory.Active = true;
            for (int i = 0; i < inventory.weaponsList.Count; i++)
            {
                inventory.weaponsList[i].GameObject.Transform.Position = new Vector2(400 + i * 100, 500);
            }

        }

        public void SellToPlayer(Weapon weapon)
        {
            GameWorld.Instance.Destroy(weapon.button);
            GameWorld.Instance.Destroy(weapon.GameObject);
        }

        public override void Start()
        {
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.SetSprite("ShopKeeper");
            GameObject.Transform.Transformer(new Vector2(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2, sr.Sprite.Height * 0.4f), 0f, new Vector2(0.4f, 0.4f), Color.White, 0.8f);
        }
        public override void OnCollisionEnter(Collider col)
        {
            base.OnCollisionEnter(col);
        }
    }
}
