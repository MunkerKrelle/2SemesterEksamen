using _2SemesterEksamen;
using ComponentPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Algoritmer
{
    public class CellManager
{
        public void SetUpCells(int cellCountA, int cellCountB) //flyttes over til Cells
        {
            for (int y = 1; y < cellCountA; y++)
            {
                for (int x = 1; x < cellCountB; x++)
                {
                    GameObject cellGrid = new GameObject();
                    Cell cell = cellGrid.AddComponent<Cell>(new Point(x, y));
                    GameWorld.Instance.Cells.Add(new Point(x, y), cell);
                    SpriteRenderer sr = cellGrid.AddComponent<SpriteRenderer>();
                    GameWorld.Instance.Instantiate(cellGrid);
                    //GameWorld.Instance.gameObjects.Add(cellGrid);
                    sr.SetSprite("cellGrid");

                    cellGrid.Transform.Layer = 0f;
                    //GameWorld.Instance.Cells[new Point(x, y)].Sprite = GameWorld.Instance.sprites["cellGrid"];
                    cellGrid.Transform.Scale = new Vector2(1, 1);
                    Point pos = new Point(x, y);
                    cellGrid.Transform.Position = new Vector2(pos.X * 100, pos.Y * 100);
                }
            }
        }
    }

}
