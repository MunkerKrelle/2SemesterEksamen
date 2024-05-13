using System;
using ComponentPattern;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryPattern
{
    class ItemFactory : Factory
    {

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
        public override GameObject Create()
        {
            return (GameObject)prototype.Clone();
        }
    }
}
