using ComponentPattern;
using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterEksamen;

namespace BuilderPattern
{
    class Director
    {
        private IBuilder builder;

        public Director(IBuilder builder)
        {
            this.builder = builder;
        }

        public GameObject Construct()
        {
            builder.BuildGameObject();

            return builder.GetResult();
        }
    }
}
