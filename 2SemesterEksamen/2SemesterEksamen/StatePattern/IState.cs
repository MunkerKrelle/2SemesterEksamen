using System;
using ComponentPattern;
using Microsoft.Xna.Framework;
using StatePattern;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StatePattern
{
    public interface Istate<T>
    {
        void Enter(T parrent);
        void Execute();
        void Exit();
    }
}
