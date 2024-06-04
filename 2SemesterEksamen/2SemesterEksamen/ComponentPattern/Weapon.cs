    using _2SemesterEksamen;
    using FactoryPattern;
    using Microsoft.Xna.Framework;
    using RepositoryPattern;

    namespace ComponentPattern
    {
        /// <summary>
        /// Oprelse af våben som spiller kan bruge til at attck og armsDealern kan sælge
        /// </summary>
        public class Weapon : Component
        {
            public GameObject button;
            private Database database = new Database();
            private string name;
            private int damage, price;
            public string Name { get { return name; } }
            public int Damage { get { return damage; } }
            public int Price {  get { return price; } }

            public Weapon(GameObject gameObject, string name, int damage, int price) : base(gameObject)
            {
                this.name = name;
                this.damage = damage;
                this.price = price;
            }

        public override void Awake()
        {

        }
        /// <summary>
        /// Lav knapper som kan trykkes på for at købe våben
        /// </summary>
        public void CreateButtons()
            {
                button = ButtonFactory.Instance.Create(GameObject.Transform.Position, Name, () => Buy());
                GameWorld.Instance.Instantiate(button);
                button.Transform.Scale = new Vector2(0.2f, 0.4f);
                button.Transform.Color = Color.Black;
            }

            /// <summary>
            /// Siger til databasen at der skal købes våben
            /// </summary>
            public void Buy()
            {
                bool canBuy = database.TradeWeapon(GameObject.GetComponent <Weapon>() as Weapon);
                if (canBuy)
                {
                    GameWorld.Instance.Destroy(button);
                    GameWorld.Instance.Destroy(GameObject);
                }
            }

            public override void Update(GameTime gameTime)
            {
            }
        }
    }
