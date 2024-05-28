using ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    internal class InteractCommand : ICommand
    {
        private Player player;
        public InteractCommand(Player player)
        {
            this.player = player;
        }
        public void Execute()
        {

        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
