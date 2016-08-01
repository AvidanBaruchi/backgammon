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
        private Dictionary<PlayerStatus, Func<MoveDescription, bool>> _movesValidations = null;
        
        public GameRules(ReadOnlyCollection<Point> points)
        {
            _points = points;
            _movesValidations = new Dictionary<PlayerStatus, Func<MoveDescription, bool>>
            {
                { PlayerStatus.Playing, CanMove},
                { PlayerStatus.InJail, CanExitFromJail},
                { PlayerStatus.FoldingOut, CanFoldOut}
            };
        }

        public Func<MoveDescription, bool> CanMove(PlayerStatus status)
        {
            return _movesValidations[status];
        }

        private bool CanMove(MoveDescription move)
        {
            if (!InBounds(move)) return false;

            if (_points[move.From].Player == _points[move.To].Player
                || _points[move.To].Status == PointStatus.Single
                || _points[move.To].Status == PointStatus.Empty)
            {
                return true;
            }

            return false;
        }

        private bool CanExitFromJail(MoveDescription move)
        {
            if (!InBounds(move)) return false;

            if (move.PlayerId == _points[move.To].Player
                || _points[move.To].Status != PointStatus.Multi)
            {
                return true;
            }

            return false;
        }

        private bool CanFoldOut(MoveDescription move)
        {
            // Check distances. it can be normal move too.
            if (!InBounds(move)) return false;

            if (_points[move.From].Player == move.PlayerId)
            {
                return true;
            }

            return false;
        }

        private bool InBounds(MoveDescription move)
        {
            if (move.From < 0) return false;

            if (move.To > 23) return false;

            return true;
        }
    }
}
