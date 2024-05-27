using _2SemesterEksamen;
using FactoryPattern;
using Microsoft.Xna.Framework;
using RepositoryPattern;
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
        private GameObject gameObject;
        private Random rnd = new Random();
        private List<int> list = new List<int>() { 0 };

        public ArmsDealer(GameObject gameObject) : base(gameObject)
        {

        }

        private int RandomItem()
        {
            while (true)
            {
                int tryNumber = rnd.Next(8);
                if (!list.Contains(tryNumber))
                {
                    list.Add(tryNumber);
                    return tryNumber;
                }
                else
                {
                    continue;
                }
            }
        }

        public override void Awake()
        {
            inventory = GameObject.GetComponent<Inventory>() as Inventory;
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            inventory.Active = true;
            for (int i = 0; i < inventory.weaponsList.Count; i++)
            {
                inventory.weaponsList[i].GameObject.Transform.Position = new Vector2(400 + i * 100, 500);
                inventory.weaponsList[i].CreateButtons();
            }
        }

        public void UpdateItems()
        {
            foreach (var item in inventory.weaponsList)
            {
                GameWorld.Instance.Destroy(item.button);
                GameWorld.Instance.Destroy(item.GameObject);
            }
            inventory.weaponsList.Clear();
            list = new List<int>() { 0 };
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            inventory.GenerateRandomItem(RandomItem());
            for (int i = 0; i < inventory.weaponsList.Count; i++)
            {
                inventory.weaponsList[i].GameObject.Transform.Position = new Vector2(400 + i * 100, 500);
                inventory.weaponsList[i].CreateButtons();
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
