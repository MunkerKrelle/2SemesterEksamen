using _2SemesterEksamen;
using ComponentPattern;
using Microsoft.Xna.Framework;

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
                    if (x == 9 && y == 1)
                    {
                        sr.SetSprite("EnterShop");
                        cellGrid.Transform.Transformer(cellGrid.Transform.Position, 0, new Vector2(1, 1), Color.White, 0.3f);
                    }

                    else if (x == 10 && y == 1)
                    {
                        sr.SetSprite("ExitShop");
                        cellGrid.Transform.Transformer(cellGrid.Transform.Position, 0, new Vector2(1, 1), Color.White, 0.3f);
                    }

                    else 
                    {
                        sr.SetSprite("cellGrid");
                        cellGrid.Transform.Transformer(cellGrid.Transform.Position, 0, new Vector2(1, 1), Color.White, 0f);
                    }

                    cellGrid.Transform.Scale = new Vector2(1, 1);
                    Point pos = new Point(x, y);
                    cellGrid.Transform.Position = new Vector2(pos.X * 100, pos.Y * 100);
                }
            }
        }
    }

}
