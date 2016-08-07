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
        private GameRules _rules = null;

        public MovesCalculator(ReadOnlyCollection<Point> points)
        {
            _points = points;
            _rules = new GameRules(_points);
        }

        public List<MoveDescription> GetPossibleMoves(Player player, List<int> dice)
        {
            var query = from point in _points
                        where IsValidPoint(player, point)//where point.PlayerId == player.PlayerId
                        from diceValue in dice
                        let fromIndex = point.Index
                        let toIndex = CalcToIndex(player, fromIndex, diceValue)
                        let move = new MoveDescription(fromIndex, toIndex, player.Direction, player.Status, player.PlayerId)
                        where _rules.CanMove(player.Status)(move)
                        select move;

            return query.ToList();
        }

        private bool IsValidPoint(Player player, Point point)
        {
            if (player.Status == PlayerStatus.InJail)
            {
                var homeFrom = player.Direction == MoveDirection.Right ? 0 : 18;
                var homeTo = player.Direction == MoveDirection.Right ? 5 : 23;

                if (point.Index <= homeFrom || point.Index >= homeTo) return false;

                return point.PlayerId == player.PlayerId ||
                    point.Status != PointStatus.Multi;
            }

            return point.PlayerId == player.PlayerId;
        }

        private int CalcToIndex(Player player, int from, int diceValue)
        {
            diceValue = player.Direction == MoveDirection.Right ? diceValue : -diceValue;

            if (player.Status == PlayerStatus.FoldingOut)
            {
                return from;
            }

            if (player.Status == PlayerStatus.InJail)
            {
                return player.Direction == MoveDirection.Right ? diceValue - 1 : 24 + diceValue;
            }

            return from + diceValue;
        }
    }
}
