using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    
    internal class GameRules
    {
        private ReadOnlyCollection<Point> _points;
        private Dictionary<PlayerStatus, Func<MoveDescription, int ,bool>> _movesValidations = null;
        
        public GameRules(ReadOnlyCollection<Point> points)
        {
            _points = points;
            _movesValidations = new Dictionary<PlayerStatus, Func<MoveDescription, int, bool>>
            {
                { PlayerStatus.Playing, CanMove},
                { PlayerStatus.InJail, CanExitFromJail},
                { PlayerStatus.FoldingOut, CanFoldOut}
            };
        }

        public Func<MoveDescription, int, bool> CanMove(PlayerStatus status)
        {
            return _movesValidations[status];
        }

        private bool CanMove(MoveDescription move, int dieValue)
        {
            if (!InBounds(move)) return false;

            if (_points[move.From].PlayerId == _points[move.To].PlayerId
                || _points[move.To].Status == PointStatus.Single
                || _points[move.To].Status == PointStatus.Empty)
            {
                return true;
            }

            return false;
        }

        private bool CanExitFromJail(MoveDescription move, int dieValue)
        {
            if (!InBounds(move)) return false;

            if (move.From == move.To && (move.PlayerId == _points[move.To].PlayerId
                || _points[move.To].Status != PointStatus.Multi))
            {
                return true;
            }

            return false;
        }

        private bool CanFoldOut(MoveDescription move, int dieValue)
        {
            // Check distances. it can be normal move too.
            if (!InBounds(move)) return false;

            if (_points[move.From].PlayerId != move.PlayerId)
            {
                return false;
            }

            int foldIndex = move.Direction == MoveDirection.Left ? dieValue - 1 :
                24 - dieValue;

            if(move.From != foldIndex)
            {
                if(move.Direction == MoveDirection.Left)
                {
                    return move.From <= foldIndex;
                }
                else
                {
                    return move.From >= foldIndex;
                }

                //return false;
            }

            return true;
        }

        private bool InBounds(MoveDescription move)
        {
            if (move.From < 0 || move.From > 23) return false;

            if (move.To < 0 || move.To > 23) return false;

            return true;
        }
    }
}
