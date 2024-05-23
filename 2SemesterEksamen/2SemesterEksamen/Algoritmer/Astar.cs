using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using _2SemesterEksamen;
using ComponentPattern;

namespace Algoritmer
{
    class Astar
    {
        Dictionary<Point, Cell> cells;

        public Astar(Dictionary<Point, Cell> cells)
        {
            this.cells = cells;
        }

        //ÅBEN LISTE / OPEN LIST
        //  • En liste over noder, som endnu ikke er kandidater til at blive en del af stien
        //  • Man kan se på open listen, som en liste af noder, der skal undersøges
        private HashSet<Cell> openList = new HashSet<Cell>();

        //LUKKET LISTE / CLOSED LIST
        //  • Består af bekræftede kandidater til ruten, dvs. at noderne kan blive en del af den endelige sti
        //  • Disse noder er blevet undersøgt
        private HashSet<Cell> closedList = new HashSet<Cell>();

        /// <summary>
        /// Metoden bliver oprettet og skal returnere en liste over cellerene (felterne)
        /// </summary>
        /// <param name="startPoint">Hvilken celle algoritemen skal starte ved</param>
        /// <param name="endPoint">Hvilken celle algoritme skal slutte ved</param>
        /// <returns></returns>
        public List<Cell> FindPath(Point startPoint, Point endPoint)
        {
            //Start punktet bliver tilføjet til open list, ikke undersøgt og er endnu ikke kandidater
            openList.Add(cells[startPoint]);
            while (openList.Count > 0)
            {
                //Current cell bliver tilføjet på første plads på den åbne liste
                Cell curCell = openList.First();
                foreach (var t in openList)
                {
                    //Hvis cellens F værdi er mindre end currentCell || Hvis Cellens F værdi = Nuværende F && cellens H er mindre end nuværende H
                    if (t.F < curCell.F ||
                        t.F == curCell.F && t.H < curCell.H)
                    {
                        curCell = t;
                    }
                }
                //Og cellen bliver fjernet fra den åbne liste + tilføjet til den lukkede liste og er nu en kandidat, som er blevet undersøgt
                openList.Remove(curCell);
                closedList.Add(curCell);

                //Hvis den nuværende celle er den samme som målets, så er vejen fundet og ruten returneres
                if (curCell.Position.X == endPoint.X && curCell.Position.Y == endPoint.Y)
                {
                    //path found!
                    return RetracePath(cells[startPoint], cells[endPoint]);
                }

                //Listen over naboer bliver skabt ud fra GetNeighbors metoden, til at finde naboerne til nuværende celle
                List<Cell> neighbors = GetNeighbors(curCell);
                foreach (var neighbor in neighbors)
                {
                    //Hvis naboen er på den lukkede liste i forvejen, skal den ikke undersøges
                    if (closedList.Contains(neighbor))
                    {
                        continue;
                    }
                    //Værdien for at bevæge sig til en nabocelle instantieres
                    int newMovementCostToNeighbor = curCell.G + GetDistance(curCell.Position, neighbor.Position);
                    if (newMovementCostToNeighbor < neighbor.G || !openList.Contains(neighbor))
                    {
                        neighbor.G = newMovementCostToNeighbor;
                        //calulate h using manhatten principle
                        neighbor.H = ((Math.Abs(neighbor.Position.X - endPoint.X) + Math.Abs(endPoint.Y - neighbor.Position.Y)) * 10);
                        neighbor.Parent = curCell;

                        //Hvis naboen ikke findes på den åbne list, tilføjes den hertil
                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Function til at backtrack pathen 
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private List<Cell> RetracePath(Cell startPoint, Cell endPoint)
        {
            List<Cell> path = new List<Cell>();
            Cell currentNode = endPoint;

            //Når den nuværende node IKKE er den samme som start noden, tilføjes disse til listen 'path'
            while (currentNode != startPoint)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Add(startPoint);
            path.Reverse();
            foreach (var pathPoint in path)
            {
                if (pathPoint.Position == GameWorld.Instance.Cells[new Point(4, 8)].Position ||
                    pathPoint.Position == GameWorld.Instance.Cells[new Point(5, 8)].Position ||
                    pathPoint.Position == GameWorld.Instance.Cells[new Point(6, 8)].Position)
                {
                    //GameWorld.ForestClosed = true;
                }
            }
            return path;
        }

        /// <summary>
        /// Distancen bliver udregnet
        /// </summary>
        /// <param name="neighborPosition">Nabo til den nuværende celle</param>
        /// <param name="endPoint">den nuværrende slutposition</param>
        /// <returns></returns>
        private int GetDistance(Point neighborPosition, Point endPoint)
        {
            int dstX = Math.Abs(neighborPosition.X - endPoint.X);
            int dstY = Math.Abs(neighborPosition.Y - endPoint.Y);

            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            }
            return 14 * dstX + 10 * (dstY - dstX);
        }

        /// <summary>
        /// Her bliver der fundet naboer til den nuværende celle og checked og om man kan gå på dem eller ej
        /// </summary>
        /// <param name="curCell">Den nuværende celle i listen</param>
        /// <returns></returns>
        private List<Cell> GetNeighbors(Cell curCell)
        {
            List<Cell> neighbors = new List<Cell>(8);
            //var wallSprite = GameWorld.Instance.sprites["Wall"];
            //var treeSprite = GameWorld.Instance.sprites["Tree"];
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    //ignore own position.
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    //Skal være være inden for grid
                    Cell curNeighbor;
                    if (cells.TryGetValue(new Point(curCell.Position.X + i, curCell.Position.Y + j), out var cell))
                    {
                        curNeighbor = cell;
                    }
                    else
                    {
                        continue;
                    }

                    //Kan ikke være en væg eller et træ
                    //if (GameWorld.Instance.sprites["Wall"] == curNeighbor.Sprite)
                    //{
                    //    continue;
                    //}
                    //if (GameWorld.Instance.sprites["Tree"] == curNeighbor.Sprite)
                    //{
                    //    continue;
                    //}

                    //Checker for hjørner
                    switch (i)
                    {
                        //    //Øverst venstre
                        //    case -1 when j == 1 && (cells[curCell.Position + new Point(i, 0)].Sprite == wallSprite || cells[curCell.Position + new Point(0, j)].Sprite == wallSprite):
                        //    //Øverst højre
                        //    case 1 when j == 1 && (cells[curCell.Position + new Point(i, 0)].Sprite == wallSprite || cells[curCell.Position + new Point(0, j)].Sprite == wallSprite):
                        //    //Nederst venstre
                        //    case -1 when j == -1 && (cells[curCell.Position + new Point(i, 0)].Sprite == wallSprite || cells[curCell.Position + new Point(0, j)].Sprite == wallSprite):
                        //    //Nederst højre 
                        //    case 1 when j == -1 && (cells[curCell.Position + new Point(i, 0)].Sprite == wallSprite || cells[curCell.Position + new Point(0, j)].Sprite == wallSprite):
                        //        continue;
                        default:
                            neighbors.Add(curNeighbor);
                            break;
                    }
                    //switch (i)
                    //{
                    //    //Øverst venstre
                    //    case -1 when j == 1 && (cells[curCell.Position + new Point(i, 0)].Sprite == treeSprite || cells[curCell.Position + new Point(0, j)].Sprite == treeSprite):
                    //    //Øverst højre
                    //    case 1 when j == 1 && (cells[curCell.Position + new Point(i, 0)].Sprite == treeSprite || cells[curCell.Position + new Point(0, j)].Sprite == treeSprite):
                    //    //Nederst venstre
                    //    case -1 when j == -1 && (cells[curCell.Position + new Point(i, 0)].Sprite == treeSprite || cells[curCell.Position + new Point(0, j)].Sprite == treeSprite):
                    //    //Nederst højre
                    //    case 1 when j == -1 && (cells[curCell.Position + new Point(i, 0)].Sprite == treeSprite || cells[curCell.Position + new Point(0, j)].Sprite == treeSprite):
                    //        continue;
                    //    default:
                    //        neighbors.Add(curNeighbor);
                    //        break;
                    //}
                }
            }
            return neighbors;
        }
    }
}
