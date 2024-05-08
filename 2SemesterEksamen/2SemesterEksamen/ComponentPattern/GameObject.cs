using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ComponentPattern
{

    public class GameObject : ICloneable
    {
        private List<Component> components = new List<Component>();

        public Transform Transform { get; private set; } = new Transform();

        public string Tag { get; set; }


        public T AddComponent<T>(params object[] additionalParameters) where T : Component
        {
            Type componentType = typeof(T);
            try
            {
                // Opret en instans ved hjælp af den fundne konstruktør og leverede parametre
                object[] allParameters = new object[1 + additionalParameters.Length];
                allParameters[0] = this;
                Array.Copy(additionalParameters, 0, allParameters, 1, additionalParameters.Length);

                T component = (T)Activator.CreateInstance(componentType, allParameters);
                components.Add(component);
                return component;
            }
            catch (Exception e)
            {
                // Håndter tilfælde, hvor der ikke er en passende konstruktør
                throw new InvalidOperationException($"Klassen {componentType.Name} har ikke en " +
                    $"konstruktør, der matcher de leverede parametre.");

            }
        }

        public Component AddComponentWithExistingValues(Component component)
        {
            components.Add(component);
            return component;
        }

        public Component GetComponent<T>() where T : Component
        {
            return components.Find(x => x.GetType() == typeof(T));
        }

        public void Awake()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Awake();
            }
        }

        public void Start()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Start();
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Update(gameTime);
            }
        }
        public void OnCollisionEnter(Collider collider)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].OnCollisionEnter(collider);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Draw(spriteBatch);
            }
        }

        public object Clone()
        {
            GameObject go = new GameObject();
            foreach (Component component in components)
            {
                Component newComponent = go.AddComponentWithExistingValues(component.Clone() as Component);
                newComponent.SetNewGameObject(go);
            }
            return go;

        }
    }
}
