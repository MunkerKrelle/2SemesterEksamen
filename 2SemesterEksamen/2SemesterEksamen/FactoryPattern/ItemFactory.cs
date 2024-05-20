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
        //    var itemValues = database.ReturnValues("wrench");

        //    SpriteRenderer sr = go.AddComponent<SpriteRenderer>();

        //    switch (type)
        //    {
        //        case WEAPONTYPE.WRENCH:
        //            sr.SetSprite(itemValues[0].Item1);
        //            go.AddComponent<Weapon>(itemValues[0].Item2, itemValues[0].Item3);
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
            var itemValues = database.ReturnValues(weaponType);

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.SetSprite(itemValues.Item1);
            go.AddComponent<Weapon>(itemValues.Item1, itemValues.Item2, itemValues.Item3);

            return go;
        }

        public GameObject Create(int weaponID)
        {
            GameObject go = new GameObject();
            var itemValues = database.ReturnValuesWithID(weaponID);

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.SetSprite(itemValues.Item1);
            go.AddComponent<Weapon>(itemValues.Item1, itemValues.Item2, itemValues.Item3);

            return go;
        }

        public override GameObject Create()
        {
            GameObject go = new GameObject();

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.SetSprite("butterflyknife");
            go.AddComponent<Weapon>();
            return go;
        }
    }
}
