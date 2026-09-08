using Game.Core.BoardBase;
using System.Collections.Generic;

namespace Game.Mechanics
{
    public class MatchNeighbourFinder
    {
        private readonly bool[,] _visitedCells = new bool[Board.Rows, Board.Cols];

        public List<Cell> FindMatchNeighbours(List<Cell> matchedCells)
        {
            var resultCells = new List<Cell>();
            ClearVisitedCells();
            FindMatchNeighbours(matchedCells, resultCells);

            return resultCells;
        }

        private void FindMatchNeighbours(List<Cell> matchedCells, List<Cell> resultCells)
        {
            for (int i = 0; i < matchedCells.Count; i++)
            {
                var currentMatchCell = matchedCells[i];
                _visitedCells[currentMatchCell.X, currentMatchCell.Y] = true;
            }
            for (int i = 0; i < matchedCells.Count; i++)
            {
                var currentMatchCell = matchedCells[i];
                var neighbours = currentMatchCell.Neighbours;
                for (int j = 0; j < neighbours.Count; j++)
                {
                    var currentNeighbour = neighbours[j];
                    if (!_visitedCells[currentNeighbour.X, currentNeighbour.Y])
                    {
                        _visitedCells[currentNeighbour.X, currentNeighbour.Y] = true;
                        resultCells.Add(currentNeighbour);
                    }
                }
            }
        }

        private void ClearVisitedCells()
        {
            for (var x = 0; x < _visitedCells.GetLength(0); x++)
            {
                for (var y = 0; y < _visitedCells.GetLength(1); y++)
                {
                    _visitedCells[x, y] = false;
                }
            }
        }

    }
}