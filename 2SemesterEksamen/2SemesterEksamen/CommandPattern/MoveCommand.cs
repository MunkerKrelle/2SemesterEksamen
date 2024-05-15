using _2SemesterEksamen;
using ComponentPattern;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandPattern
{
    internal class MoveCommand : ICommand
    {
        private Player player;
        private Vector2 velocity;

        //public MoveCommand(Player player, Vector2 velocity)
        //{
        //    this.player = player;
        //    this.velocity = velocity;
        //}

        public MoveCommand(Player player, Vector2 velocity)
        {
            this.player = player;
            this.velocity = velocity;
            //player.GameObject.Transform.Position = new Vector2(1250 , 700);
            // player.GameObject.Transform.CellMovement(velocity, velocity);
        }
        public void Execute()
        {
            player.Move(velocity);
        }

        public void Undo()
        {

        }
    }
}
