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

    public class MoveDescription
    {
        public MoveDescription(int from, int to, MoveDirection direction, PlayerStatus playerStatus, PlayerId playerId)
        {
            From = from;
            To = to;
            Direction = direction;
            PlayerStatus = PlayerStatus;
            PlayerId = playerId;
        }

        public int From { get; set; }

        public int To { get; set; }

        public MoveDirection Direction { get; set; }

        public PlayerStatus PlayerStatus { get; set; }

        public PlayerId PlayerId { get; set; }
    }
}
