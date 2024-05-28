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

        private Database database = new Database();



        public GameObject Create(string weaponType)
        {
            GameObject go = new GameObject();
            var itemValues = database.ReturnValues(weaponType);

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.SetSprite(itemValues.Item1);
            go.Transform.Layer = 0.7f;
            go.AddComponent<Weapon>(itemValues.Item1, itemValues.Item2, itemValues.Item3);

            return go;
        }

        public GameObject Create(int weaponID)
        {
            GameObject go = new GameObject();
            var itemValues = database.ReturnValuesWithID(weaponID);

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.SetSprite(itemValues.Item1);
            go.Transform.Layer = 0.7f;
            go.AddComponent<Weapon>(itemValues.Item1, itemValues.Item2, itemValues.Item3);

            return go;
        }

        public override GameObject Create()
        {
            GameObject go = new GameObject();

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.SetSprite("butterflyknife");
            go.Transform.Layer = 0.7f;
            go.AddComponent<Weapon>();
            return go;
        }
    }
}
