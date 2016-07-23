using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public enum MoveDirection
    {
        Left,
        Right
    }

    public enum MoveType
    {
        OnBoard,
        ExitFromJail,
        FoldOut
    }

    public class MoveDescription
    {
        public MoveDescription(int from, int to, MoveDirection direction, MoveType moveType)
        {
            From = from;
            To = to;
            Direction = direction;
            MoveType = moveType;
        }

        public int From { get; set; }

        public int To { get; set; }

        public MoveDirection Direction { get; set; }

        public MoveType MoveType { get; set; }
    }
}
