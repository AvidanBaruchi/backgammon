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
        private List<int> _currentDice;
        private IEnumerable<MoveDescription> _possibleMoves = null;
        private MovesCalculator _movesCalculator = null;

        public BackgammonGameManager(string playerOneName, string playerTwoName)
        {
            IsGameOver = false;
            _dice.Roll();
            _currentDice = new List<int>(_dice.Values);
            initPlayers(playerOneName, playerTwoName);
        }

        private void initPlayers(string playerOneName, string playerTwoName)
        {
            _playerOne = new Player(playerOneName, MoveDirection.Right,
                PlayerId.One,
                PlayerStatus.Playing);

            _playerTwo = new Player(playerTwoName, MoveDirection.Left,
                PlayerId.Two,
                PlayerStatus.Playing);

            _currentPlayer = _playerOne;
            ComputePossibleMoves();
        }

        public event Action BoardStateChanged;

        public PlayerInfo CurrentPlayer => new PlayerInfo(_currentPlayer.Name, _currentPlayer.PlayerId);

        public bool IsGameOver { get; private set; }

        public bool MakeMove(int from, int to)
        {
            if (IsGameOver) return false;

            var requestedMove = new MoveDescription(from, to, _currentPlayer.Direction, _currentPlayer.Status, _currentPlayer.PlayerId);

            if(!CanMakeMove(requestedMove))
            {
                return false;
            }

            if(requestedMove.PlayerStatus == PlayerStatus.Playing)
            {
                _board.Remove(from);
                _board.TryAdd(to, requestedMove.PlayerId);
            }
            else if(requestedMove.PlayerStatus == PlayerStatus.FoldingOut)
            {
                _board.Remove(from);
            }
            else if(requestedMove.PlayerStatus == PlayerStatus.InJail)
            {
                _board.ExitFromJail(to, requestedMove.PlayerId);
            }

            _currentDice.Remove(Math.Abs(requestedMove.From - requestedMove.To));
            CheckPlayersStatuses();
            SwitchPlayer();
            CheckGameOver();
            ComputePossibleMoves();
            OnBoardStateChanged();

            return true;
        }

        public IEnumerable<int> GetDiceValues => _dice.Values;

        public IJail GetJail() => _board;

        public IEnumerable<PointInfo> Points => _board;

        private void CheckGameOver()
        {
            if (!_board.IsInBoard(_playerOne.PlayerId))
            {
                IsGameOver = true;
            }
            else if(!_board.IsInBoard(_playerTwo.PlayerId))
            {
                IsGameOver = true;
            }
        }

        private void CheckPlayersStatuses()
        {
            CheckPlayerStatus(_playerOne);
            CheckPlayerStatus(_playerTwo);
        }

        private void CheckPlayerStatus(Player player)
        {
            if(_board.IsInJail(player.PlayerId))
            {
                player.Status = PlayerStatus.InJail;
            }
            else if(IsFoldingOut(player))
            {
                player.Status = PlayerStatus.FoldingOut;
            }
            else
            {
                player.Status = PlayerStatus.Playing;
            }
        }

        private bool IsFoldingOut(Player player)
        {
            int from = player.Direction == MoveDirection.Right ? 6 : 0;
            int to = player.Direction == MoveDirection.Right ? 23 : 17;

            if(_board.IsInJail(player.PlayerId))
            {
                return false;
            }

            for (int i = from; i <= to; i++)
            {
                if(_board[i].PlayerId == player.PlayerId)
                {
                    return false;
                }
            }

            return true;
        }

        private void SwitchPlayer()
        {
            if (_currentDice.Count == 0)
            {
                _currentPlayer = _currentPlayer == _playerOne ? _playerTwo : _playerOne;
                NewRound();
            }
        }

        private void NewRound()
        {
            _dice.Roll();
            _currentDice = new List<int>(_dice.Values);
            ComputePossibleMoves();
        }

        private void ComputePossibleMoves()
        {
            _movesCalculator = new MovesCalculator(_board.Points);
            _possibleMoves = _movesCalculator.GetPossibleMoves(_currentPlayer, _currentDice);

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
                return currentMove.Equals(move);
            });
        }

        private void OnBoardStateChanged()
        {
            BoardStateChanged?.Invoke();
        }
    }
}
