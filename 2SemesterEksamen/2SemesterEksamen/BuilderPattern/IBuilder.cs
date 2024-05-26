using ComponentPattern;
using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterEksamen;

namespace BuilderPattern
{
    /// <summary>
    /// Interface til at definere, hvordan GameObject'er skal bygges.
    /// </summary>
    interface IBuilder
    {
        /// <summary>
        /// Metode til at bygge et GameObject med dets komponenter.
        /// </summary>
        void BuildGameObject();

        /// <summary>
        /// Metode til at få det færdigbyggede GameObject.
        /// </summary>
        /// <returns>Det færdigbyggede GameObject.</returns>
        GameObject GetResult();
    }
}

