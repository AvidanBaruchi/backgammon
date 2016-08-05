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

    public class MoveDescription : IEquatable<MoveDescription>
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

        public override bool Equals(object obj)
        {
            var move = obj as MoveDescription;

            if(obj == null)
            {
                return false;
            }

            return Equals(move);
        }

        public bool Equals(MoveDescription other)
        {
            if(other == null)
            {
                return false;
            }

            if(PlayerId != other.PlayerId)
            {
                return false;
            }

            if(Direction != other.Direction)
            {
                return false;
            }

            if(PlayerStatus != other.PlayerStatus)
            {
                return false;
            }

            if(PlayerStatus == PlayerStatus.InJail)
            {
                return To == other.To;
            }
            
            if(PlayerStatus == PlayerStatus.FoldingOut)
            {
                return From == other.From;
            }

            return From == other.From && To == other.To;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
