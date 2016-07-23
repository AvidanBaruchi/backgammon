using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public class Jail
    {
        private List<PlayerId> _jail = new List<PlayerId>();

        public void JailPlayer(PlayerId player)
        {
            _jail.Add(player);
        }

        public void FreePlayer(PlayerId player)
        {
            if (_jail.Contains(player))
            {
                _jail.Remove(player);
            }
        }

        public bool IsInJail(PlayerId player)
        {
            return _jail.Contains(player);
        }

        public int GetJailPlayerCount(PlayerId player)
        {
            return _jail.Where(p => p == player).Count();
        }

        public static Jail CopyFrom(Jail jail)
        {
            var newJail = new Jail();

            for (int i = 0; i < jail.GetJailPlayerCount(PlayerId.One); i++)
            {
                newJail.JailPlayer(PlayerId.One);
            }

            for (int i = 0; i < jail.GetJailPlayerCount(PlayerId.Two); i++)
            {
                newJail.JailPlayer(PlayerId.Two);
            }

            return newJail;
        }
    }
}
