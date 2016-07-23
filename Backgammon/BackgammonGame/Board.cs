using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public enum PlayerId {
        None,
        One,
        Two
    }

    public class Board
    {
        private readonly Point[] _points = new Point[24];
        private readonly Jail _jail = new Jail();

        public Board()
        {
            init();
        }

        private void init()
        {
            for (int i = 0; i < 24; i++)
            {
                _points[i] = new Point(i);
            }

            FillPoint(0, 2, PlayerId.One);
            FillPoint(11, 5, PlayerId.One);
            FillPoint(16, 3, PlayerId.One);
            FillPoint(18, 5, PlayerId.One);

            FillPoint(23, 2, PlayerId.Two);
            FillPoint(12, 5, PlayerId.Two);
            FillPoint(7, 3, PlayerId.Two);
            FillPoint(5, 5, PlayerId.Two);
        }

        public Point this[int index]
        {
            get
            {
                if (index < 0 || index > 23)
                {
                    throw new IndexOutOfRangeException("Accepting index from 0 to 23");
                }
                else
                {
                    return _points[index];
                }
            }
            private set
            {
                this[index] = value;
            }
        }

        public ReadOnlyCollection<Point> Points => new ReadOnlyCollection<Point>(_points);

        public Jail Jail => Jail.CopyFrom(_jail);

        private void FillPoint(int index, int count, PlayerId player)
        {
            for (int i = 0; i < count; i++)
            {
                this[index].Add(player);
            }
        }
    }
}
