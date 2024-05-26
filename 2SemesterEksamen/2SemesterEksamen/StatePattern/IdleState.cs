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
    public class IdleState : Istate<Enemy>
    {
        private Enemy parrent;
        public void Enter(Enemy parrent)
        {
            this.parrent = parrent;
        }
        public void Execute()
        {
            parrent.animator.PlayAnimation("CyborgIdle");
        }
        public void Exit()
        {

        }
    }
}
