using ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    internal class AttackCommand : ICommand
    {
        private Player player;
        public AttackCommand(Player player)
        {
            this.player = player;
        }
        public void Execute()
        {
            player.Attack();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
