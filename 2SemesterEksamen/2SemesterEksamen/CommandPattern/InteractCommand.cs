using ComponentPattern;
using System;

namespace CommandPattern
{
    /// <summary>
    /// Command for at interagere med andre GameObjects - bliver ikke brugt endnu
    /// </summary>
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
