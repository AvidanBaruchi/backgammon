using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public enum PlayerStatus
    {
        Playing,
        InJail,
        FoldingOut
    }

    internal class Player
    {
        public Player(string name, MoveDirection direction, PlayerId playerId, PlayerStatus status)
        {
            Name = name;
            Direction = direction;
            PlayerId = playerId;
            Status = status;
        }

        public string Name { get; private set; }

        public MoveDirection Direction { get; private set; }

        public PlayerId PlayerId { get; private set; }

        public PlayerStatus Status { get; set; }
    }
}
