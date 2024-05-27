using ComponentPattern;

namespace BuilderPattern
{
    /// <summary>
    /// Directoren styrer byggeprocessen for et GameObject ved hjælp af en specifik builder.
    /// </summary>
    class Director
    {
        private IBuilder builder;

        /// <summary>
        /// Konstruktør til at initialisere directoren med en builder.
        /// </summary>
        /// <param name="builder">Den builder, der skal bruges til at konstruere GameObject.</param>
        public Director(IBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>
        /// Metode til at konstruere et GameObject ved hjælp af den angivne builder.
        /// </summary>
        /// <returns>Det færdigbyggede GameObject.</returns>
        public GameObject Construct()
        {
            builder.BuildGameObject();

            return builder.GetResult();
        }
    }
}

