using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public class PlayerInfo
    {
        public PlayerInfo(string name, PlayerId playerId)
        {
            Name = name;
            PlayerId = playerId;
        }

        public string Name { get; private set; }

        public PlayerId PlayerId { get; private set; }
    }
}
