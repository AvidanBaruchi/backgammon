using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public class BackgammonGameManager
    {
        private Board _board = new Board();
        private Player _playerOne;
        private Player _playerTwo;
        private Player _currentPlayer;

        public BackgammonGameManager(string playerOneName, string playerTwoName)
        {
            IsGameOver = false;
            initPlayers(playerOneName, playerTwoName);
        }

        private void initPlayers(string playerOneName, string playerTwoName)
        {
            _playerOne = new Player(playerOneName, MoveDirection.Left,
                PlayerId.One,
                PlayerStatus.Playing);

            _playerTwo = new Player(playerTwoName, MoveDirection.Right,
                PlayerId.Two,
                PlayerStatus.Playing);

            _currentPlayer = _playerOne;
        }

        public PlayerInfo CurrentPlayer => new PlayerInfo(_currentPlayer.Name, _currentPlayer.PlayerId);

        public bool IsGameOver { get; private set; }

        public void MakeMove(MoveDescription move)
        {

        }

        public IEnumerable<MoveDescription> GetPossibleMoves()
        {

        }

        public Jail GetJail() => _board.Jail;

        public PointInfo[] Points
        {
            get
            {
                var pointsInfo = new PointInfo[24];
                var points = _board.Points;
                Point currentPoint = null;

                for (int i = 0; i < points.Count; i++)
                {
                    currentPoint = _board[i];
                    pointsInfo[i] = new PointInfo(currentPoint.Size, i, currentPoint.Player);
                }

                return pointsInfo;
            }
        }
    }
}
