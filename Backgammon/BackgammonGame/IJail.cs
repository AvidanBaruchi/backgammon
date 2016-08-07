using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public interface IJail
    {
        bool IsInJail(PlayerId id);

        int GetJailCount(PlayerId id);
    }
}