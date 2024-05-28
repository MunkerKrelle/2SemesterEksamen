using ComponentPattern;
using System;

namespace CommandPattern
{
    /// <summary>
    /// The AttackCommand is used for the player attacking enemies
    /// </summary>
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
