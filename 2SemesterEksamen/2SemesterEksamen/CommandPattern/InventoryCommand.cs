using ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    internal class InventoryCommand : ICommand
{
        private Inventory inventory;
        public InventoryCommand(Inventory inventory)
        {
            this.inventory = inventory;
        }
        public void Execute()
        {
            //inventory.LoadItems();
            //if (inventory.Active)
            //{
            //    inventory.Active = false;
            //} else
            //{
            //    inventory.Active = true;
            //}
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
