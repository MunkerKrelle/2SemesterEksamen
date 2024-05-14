using System;
using ComponentPattern;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryPattern;
using System.Windows.Markup;
using System.Drawing.Text;

namespace FactoryPattern
{
    public enum WEAPONTYPE { WRENCH, STEELBAT, KATANA, LIGHTSABER }

    class ItemFactory : Factory
    {
        private static ItemFactory instance;

        public static ItemFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemFactory();
                }
                return instance;
            }
        }

        private UserRegistrationWithPattern database = new UserRegistrationWithPattern();


        private GameObject prototype;

        //public GameObject Create(WEAPONTYPE type)

        //{
        //    GameObject go = new GameObject();
        //    var wrenchValues = database.ReturnValues("wrench");

        //    SpriteRenderer sr = go.AddComponent<SpriteRenderer>();

        //    switch (type)
        //    {
        //        case WEAPONTYPE.WRENCH:
        //            sr.SetSprite(wrenchValues[0].Item1);
        //            go.AddComponent<Weapon>(wrenchValues[0].Item2, wrenchValues[0].Item3);
        //            break;
        //        case WEAPONTYPE.STEELBAT:
        //            sr.SetSprite("");
        //            break;
        //        case WEAPONTYPE.KATANA:
        //            sr.SetSprite("");
        //            break;
        //        case WEAPONTYPE.LIGHTSABER:
        //            sr.SetSprite("");
        //            break;
        //    }

        //    return go;
        //}

        public GameObject Create(string weaponType)
        {
            GameObject go = new GameObject();
            var wrenchValues = database.ReturnValues(weaponType);

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.SetSprite(wrenchValues[0].Item1.ToLower());
            go.AddComponent<Weapon>(wrenchValues[0].Item2, wrenchValues[0].Item3);

            return go;
        }

        public override GameObject Create()
        {
            GameObject go = new GameObject();

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.SetSprite("wrench");
            go.AddComponent<Weapon>();
            return go;
        }
    }
}
