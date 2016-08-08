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
            var query = (from point in _points
                        where IsValidPoint(player.PlayerId, player.Status, player.Direction, point)//where point.PlayerId == player.PlayerId
                        from diceValue in dice
                        let fromIndex = point.Index
                        let toIndex = CalcToIndex(player.Status, player.Direction, fromIndex, diceValue)
                        let move = new MoveDescription(fromIndex, toIndex, player.Direction, player.Status, player.PlayerId)
                        where _rules.CanMove(player.Status)(move, diceValue)
                        select move).ToList();

            if(player.Status == PlayerStatus.FoldingOut)
            {
                // calculate normal moves too
                var normalMoves = (from point in _points
                                  where IsValidPoint(player.PlayerId, PlayerStatus.Playing, player.Direction, point)
                                  from dieValue in dice
                                  let fromIndex = point.Index
                                  let toIndex = CalcToIndex(PlayerStatus.Playing, player.Direction, fromIndex, dieValue)
                                  let move = new MoveDescription(fromIndex, toIndex, player.Direction, PlayerStatus.Playing, player.PlayerId)
                                  where _rules.CanMove(PlayerStatus.Playing)(move, dieValue)
                                  select move).ToArray();

                

                // if there are moves that correlate exactly to dice value, remove the rest!
                var foldingMovesCorrelateToDice = query.Where(move => dice.Contains(move.From + 1) || dice.Contains(24 - move.From));

                if (foldingMovesCorrelateToDice.Any())
                {
                    query.RemoveAll(move => !foldingMovesCorrelateToDice.Contains(move));
                }
                else
                {
                    int minMax = -1;

                    if(player.Direction == MoveDirection.Left)
                    {
                        minMax = query.Max(move => move.From);
                    }
                    else
                    {
                        minMax = query.Min(move => move.From);
                    }

                    query.RemoveAll(move => move.PlayerStatus == PlayerStatus.FoldingOut && move.From != minMax);
                }

                query.AddRange(normalMoves);
            }

            return query;
        }

        private bool IsValidPoint(PlayerId id ,PlayerStatus playerStatus, MoveDirection direction, Point point)
        {
            if (playerStatus == PlayerStatus.InJail)
            {
                var homeFrom = direction == MoveDirection.Right ? 0 : 18;
                var homeTo = direction == MoveDirection.Right ? 5 : 23;

                if (point.Index <= homeFrom || point.Index >= homeTo) return false;

                return point.PlayerId == id ||
                    point.Status != PointStatus.Multi; 
            }

            return point.PlayerId == id;
        }

        private int CalcToIndex(PlayerStatus playerStatus, MoveDirection direction, int from, int diceValue)
        {
            diceValue = direction == MoveDirection.Right ? diceValue : -diceValue;

            if (playerStatus == PlayerStatus.FoldingOut)
            {
                return from;
            }

            if (playerStatus == PlayerStatus.InJail)
            {
                return direction == MoveDirection.Right ? diceValue - 1 : 24 + diceValue;
            }

            return from + diceValue;
        }
    }
}
