using ComponentPattern;
using RepositoryPattern;

namespace FactoryPattern
{
    /// <summary>
    /// Fabrik til opbygning af våben
    /// </summary>
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


        private GameObject prototype;

        /// <summary>
        /// Bygger et nyt våben i forhold til våbnes navn og finder det i databasen
        /// </summary>
        /// <param name="weaponType">Navnet på våbnet, så det kan finde i databasen</param>
        /// <returns></returns>
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

        /// <summary>
        /// Bygger et nyt våben i forhold til våbnes ID og finder det i databasen
        /// </summary>
        /// <param name="weaponID">ID på våbnet, så det kan finde i databasen</param>
        /// <returns></returns>
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

        /// <summary>
        /// Default create til fabrik interface
        /// </summary>
        /// <returns></returns>
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
