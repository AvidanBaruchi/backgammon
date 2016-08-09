using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public interface IPlayerInfo
    {
        string Name { get; }
        PlayerId PlayerId { get; }
        bool IsHuman { get; }
    }

    public class PlayerInfo : IPlayerInfo
    {
        public PlayerInfo(string name, PlayerId playerId, bool isHuman)
        {
            Name = name;
            PlayerId = playerId;
            IsHuman = isHuman;
        }

        public string Name { get; private set; }

        public PlayerId PlayerId { get; private set; }

        public bool IsHuman { get; private set; }
    }
}
