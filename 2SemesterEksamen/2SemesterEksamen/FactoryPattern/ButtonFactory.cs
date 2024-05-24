using System;
using ComponentPattern;
using Microsoft.Xna.Framework;
using static ComponentPattern.Button;

namespace FactoryPattern
{

    class ButtonFactory : Factory
    {
        private static ButtonFactory instance;

        public static ButtonFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ButtonFactory();
                }
                return instance;
            }
        }

        public delegate void ButtonFunctionWeapon(Weapon weapon);

        public delegate void ButtonFunction();

        /// <summary>
        /// Buttonfactory
        /// </summary>
        /// <param name="buttonPosition">button starting position</param>
        /// <param name="buttonText">The text written on the button</param>
        /// <param name="actionFunction">Whic method should run when the button is pressed</param>
        /// <returns></returns>
        //public GameObject Create(Vector2 buttonPosition, string buttonText, BUTTONTYPE bUTTONTYPE,Action<Weapon> action)
        //{
        //    GameObject go = new GameObject();

        //    SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        //    sr.SetSprite("button");
        //    go.AddComponent<Button>(buttonPosition, buttonText, bUTTONTYPE, action);

        //    return go;
        //}

        public GameObject Create(Vector2 buttonPosition, string buttonText, Action actionFunction)
        {
            GameObject go = new GameObject();

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.SetSprite("button");
            go.AddComponent<Button>(buttonPosition, buttonText, actionFunction);

            return go;
        }

        public override GameObject Create()
        {
            GameObject go = new GameObject();

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.SetSprite("button");
            go.AddComponent<Weapon>();
            return go;
        }
    }
}
