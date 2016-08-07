using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public enum PointStatus {
        Empty,
        Single,
        Multi
    }

    public class Point
    {
        private readonly Stack<PlayerId> _checkers = new Stack<PlayerId>();

        public Point(int index)
        {
            Index = index;
            Status = PointStatus.Empty;
        }

        public int Index { get; private set; }

        public int Size
        {
            get { return _checkers.Count; }
        }

        public PointStatus Status { get; private set; }

        public PlayerId PlayerId
        {
            get
            {
                if (Size > 0)
                {
                    return _checkers.Peek();
                }
                else {
                    return PlayerId.None;
                }
            }
        }

        public void Add(PlayerId player)
        {
            if (Status == PointStatus.Empty)
            {
                _checkers.Push(player);
                Status = PointStatus.Single;
            }
            else if (Status == PointStatus.Single)
            {
                if (player != _checkers.Peek())
                {
                    _checkers.Pop();
                }

                _checkers.Push(player);
                Status = Size == 1 ? PointStatus.Single : PointStatus.Multi;
            }
            else if (Status == PointStatus.Multi)
            {
                if (player != _checkers.Peek())
                {
                    throw new InvalidOperationException("Cannot add checker into a multi opponent's point!");
                }

                _checkers.Push(player);
            }
        }

        public void Remove()
        {
            if (Size > 0)
            {
                _checkers.Pop();
                Status = Size == 0 ? PointStatus.Empty : (Size == 1 ? PointStatus.Single : PointStatus.Multi);
            }
        }
    }
}
