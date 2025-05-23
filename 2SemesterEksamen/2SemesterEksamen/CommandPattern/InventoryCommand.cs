﻿using ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    /// <summary>
    /// Gør at spilleren kan skifte våben
    /// </summary>
    internal class InventoryCommand : ICommand
{
        private Player player;
        public InventoryCommand(Player player)
        {
            this.player = player;
        }
        public void Execute()
        {
            player.ChangeItem();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
