using System;
using System.Collections;
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

    internal class Board : IJail, IEnumerable<PointInfo>
    {
        private readonly Point[] _points = new Point[24];
        private readonly Dictionary<PlayerId, int> _jails = new Dictionary<PlayerId, int>();

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

            //FillPoint(0, 2, PlayerId.One);
            //FillPoint(11, 5, PlayerId.One);
            //FillPoint(16, 3, PlayerId.One);
            //FillPoint(18, 5, PlayerId.One);

            //FillPoint(23, 2, PlayerId.Two);
            //FillPoint(12, 5, PlayerId.Two);
            //FillPoint(7, 3, PlayerId.Two);
            //FillPoint(5, 5, PlayerId.Two);

            FillPoint(23, 2, PlayerId.One);
            FillPoint(22, 3, PlayerId.One);
            FillPoint(21, 3, PlayerId.One);
            FillPoint(20, 3, PlayerId.One);
            FillPoint(19, 3, PlayerId.One);
            FillPoint(17, 1, PlayerId.One);

            FillPoint(0, 2, PlayerId.Two);
            FillPoint(1, 3, PlayerId.Two);
            FillPoint(2, 3, PlayerId.Two);
            FillPoint(3, 3, PlayerId.Two);
            FillPoint(4, 3, PlayerId.Two);
            FillPoint(6, 1, PlayerId.Two);

            _jails[PlayerId.One] = 0;
            _jails[PlayerId.Two] = 0;
        }

        public bool TryAdd(int index, PlayerId id)
        {
            if (!CheckBounds(index))
            {
                return false;
            }

            try
            {
                if (this[index].PlayerId != id && this[index].Status == PointStatus.Single)
                {
                    var playerId = this[index].PlayerId;
                    this[index].Add(id);
                    //Jail.JailPlayer(this[index].Player);
                    _jails[playerId]++;
                }
                else {
                    this[index].Add(id);
                }

                return true;
            }
            catch (InvalidOperationException e)
            {
                return false;
            }
        }

        public bool Remove(int index)
        {
            if(!CheckBounds(index))
            {
                return false;
            }

            this[index].Remove();
            return true;
        }

        public bool ExitFromJail(int to, PlayerId id)
        {
            if (_jails.ContainsKey(id) && _jails[id] > 0)
            {
                var isAdded = TryAdd(to, id);
                
                if(isAdded)
                {
                    _jails[id]--;
                }

                return isAdded;
            }
            else
            {
                return false;
            }
        }

        public bool IsInJail(PlayerId id)
        {
            return GetJailCount(id) > 0;
        }

        public bool IsInBoard(PlayerId playerId)
        {
            if (IsInJail(playerId)) return true;

            foreach(var point in this)
            {
                if(point.Player == playerId)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetJailCount(PlayerId id)
        {
            if(_jails.ContainsKey(id))
            {
                return _jails[id];
            }

            return 0;
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

        public IEnumerator<PointInfo> GetEnumerator()
        {
            for (int i = 0; i < 24; i++)
            {
                yield return new PointInfo(this[i].Size, i, this[i].PlayerId);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void FillPoint(int index, int count, PlayerId player)
        {
            for (int i = 0; i < count; i++)
            {
                this[index].Add(player);
            }
        }

        private bool CheckBounds(int index)
        {
            if (index < 0 || index > 23)
            {
                return false;
            }

            return true;
        }
    }
}
