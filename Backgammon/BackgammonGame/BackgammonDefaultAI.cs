using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public class BackgammonDefaultAI
    {
        private Random _random = new Random();

        public MoveHolder GetMove(IEnumerable<IMoveDescription> possibleMoves, IEnumerable<PointInfo> points)
        {
            var movesCollection = possibleMoves.ToList();
            int index = _random.Next(0, movesCollection.Count);

            var move = movesCollection[index];

            return new MoveHolder(move.From, move.To);
        }
    }
}
