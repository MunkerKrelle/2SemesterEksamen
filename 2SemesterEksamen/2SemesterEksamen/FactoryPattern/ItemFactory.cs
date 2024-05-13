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

        public enum WEAPONTYPE { WRENCH, STEELBAT, KATANA, LIGHTSABER }


        private static ItemFactory instance;
        private GameObject prototype;

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
        private ItemFactory()
        {

        }

        private UserRegistrationWithPattern database = new UserRegistrationWithPattern();
        

        
        public  GameObject Create(WEAPONTYPE type)
        {
            GameObject go = new GameObject();
            List<string> wrenchValues = database.ReturnValues("wrench");

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            go.AddComponent<Collider>();

            switch (type)
            {
                case WEAPONTYPE.WRENCH:
                    sr.SetSprite("wrench");
                    go.AddComponent<Weapon>(wrenchValues[0], wrenchValues[1], wrenchValues[2]);
                    break;
                case WEAPONTYPE.STEELBAT:
                    sr.SetSprite("steel bat");
                    break;
                case WEAPONTYPE.KATANA:
                    sr.SetSprite("katana");
                    break;
                case WEAPONTYPE.LIGHTSABER:
                    sr.SetSprite("lightsaber");
                    break;
                default:
                    break;
            }

            return (GameObject)prototype.Clone();
        }

        public override GameObject Create()
        {
            GameObject go = new GameObject();
            return go;
        }
    }
}
