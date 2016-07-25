using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    internal class MovesCalculator
    {
        private ReadOnlyCollection<Point> _points;

        public MovesCalculator(ReadOnlyCollection<Point> points)
        {
            _points = points;
        }

        public List<MoveDescription> GetPossibleMoves(Player player, Dice dice)
        {
            var diceValues = dice.Values;


        }
    }
}
