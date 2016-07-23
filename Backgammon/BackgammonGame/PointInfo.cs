using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public class PointInfo
    {
        public PointInfo(int size, int index, PlayerId player)
        {
            Size = size;
            Index = index;
            Player = player;
        }

        public PlayerId Player { get; private set; }

        public int Size { get; private set; }

        public int Index { get; private set; }
    }
}
