using _2SemesterEksamen;
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
        public ArmsDealer(GameObject gameObject) : base(gameObject)
        {

        }

        public override void Awake()
        {
            // animator = GameObject.GetComponent<Animator>() as Animator;
            //animator.PlayAnimation("Forward");
        }
        public override void Start()
        {
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.SetSprite("ShopKeeper");
            GameObject.Transform.Scale = new Vector2(0.4f, 0.4f) ;
            GameObject.Transform.Position = new Vector2(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2, sr.Sprite.Height * 0.4f);

        }
    }
}
