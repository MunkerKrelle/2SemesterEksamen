using ComponentPattern;
using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterEksamen;

namespace BuilderPattern
{
    interface IBuilder
    {
        void BuildGameObject();

        GameObject GetResult();
    }
}
