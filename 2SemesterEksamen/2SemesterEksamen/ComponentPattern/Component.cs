using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ComponentPattern
{
    public abstract class Component : ICloneable
    {
        public bool IsEnabled { get; set; }

        public GameObject GameObject { get; private set; }

        public Component(GameObject gameObject)
        {
            GameObject = gameObject;
        }
        public virtual void Awake()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual object Clone()
        {
            Component component = (Component)MemberwiseClone();
            component.GameObject = GameObject;
            return component;
        }
        public virtual void SetNewGameObject(GameObject gameObject)
        {
            GameObject = gameObject;
        }
        public virtual void OnCollisionEnter (Collider col)
        {

        }
    }
}
