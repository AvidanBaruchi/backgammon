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
        private Dice _dice = new Dice();
        private IEnumerable<MoveDescription> _possibleMoves = null;
        private MovesCalculator _movesCalculator = null;
        private int _movesCounter = 0;

        public BackgammonGameManager(string playerOneName, string playerTwoName)
        {
            IsGameOver = false;
            _dice.Roll();
            _movesCounter = _dice.Values.Count;
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

        public bool MakeMove(int from, int to)
        {
            var requestedMove = new MoveDescription(from, to, _currentPlayer.Direction, _currentPlayer.Status, _currentPlayer.PlayerId);

            if(!CanMakeMove(requestedMove))
            {
                return false;
            }

            if(requestedMove.PlayerStatus == PlayerStatus.Playing)
            {
                _board[from].Remove();
                _board[to].Add(requestedMove.PlayerId);
            }
            else if(requestedMove.PlayerStatus == PlayerStatus.FoldingOut)
            {
                _board[from].Remove();
            }
            else if(requestedMove.PlayerStatus == PlayerStatus.InJail)
            {
                _board[to].Add(requestedMove.PlayerId);
            }
            
            _movesCounter--;
            CheckPlayersStatuses();
            SwitchPlayer();
            CheckGameOver();

            return true;
        }



        public IEnumerable<int> GetDiceValues => _dice.Values;

        public Jail GetJail() => _board.Jail;

        //IEnumerable<PointInfo>
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

        private void CheckGameOver()
        {
            //throw new NotImplementedException();
        }

        private void CheckPlayersStatuses()
        {
            CheckPlayerStatus(_playerOne);
            CheckPlayerStatus(_playerTwo);
        }

        private void CheckPlayerStatus(Player player)
        {
            if(_board.Jail.IsInJail(player.PlayerId))
            {
                player.Status = PlayerStatus.InJail;
            }
            else if()
        }

        private void SwitchPlayer()
        {
            if (_movesCounter == 0)
            {
                _currentPlayer = _currentPlayer == _playerOne ? _playerTwo : _playerOne;
            }
        }

        private void NewRound()
        {
            _dice.Roll();
            ComputePossibleMoves();
        }

        private void ComputePossibleMoves()
        {
            _movesCalculator = new MovesCalculator(_board.Points);
            _possibleMoves = _movesCalculator.GetPossibleMoves(_currentPlayer, _dice);

            //if (_currentPlayer.Status == PlayerStatus.Playing)
            //{

            //}

            //if (_currentPlayer.Status == PlayerStatus.InJail)
            //{

            //}

            //if (_currentPlayer.Status == PlayerStatus.FoldingOut)
            //{

            //}
        }

        private bool CanMakeMove(MoveDescription move)
        {
            return _possibleMoves.Any(currentMove =>
            {
                return currentMove == move;
            });
        }
    }
}
