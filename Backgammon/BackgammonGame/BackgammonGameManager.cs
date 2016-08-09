using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public enum GameType
    {
        TwoPlayers,
        PlayerVsComputer
    }

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

        public event Action BoardStateChanged;
        public event Action<IPlayerInfo, IEnumerable<int>> NoPossibleMoves;

        public BackgammonGameManager(string playerOneName, string playerTwoName, GameType gameType)
        {
            IsGameOver = false;
            _dice.Roll();
            _currentDice = new List<int>(_dice.Values);
            initPlayers(playerOneName, playerTwoName, gameType);
            Winner = null;
        }

        private void initPlayers(string playerOneName, string playerTwoName, GameType gameType)
        {
            _playerOne = new Player(playerOneName, MoveDirection.Right,
                PlayerId.One,
                PlayerStatus.Playing, 
                true);

            if(gameType == GameType.TwoPlayers)
            {
                _playerTwo = new Player(playerTwoName, MoveDirection.Left,
                PlayerId.Two,
                PlayerStatus.Playing,
                true);
            }
            else
            {
                _playerTwo = new Player(playerTwoName, MoveDirection.Left,
                    PlayerId.Two,
                    PlayerStatus.Playing,
                    false);
            }

            _currentPlayer = _playerOne;
            ComputePossibleMoves();
        }

        public IPlayerInfo CurrentPlayer => _currentPlayer;

        public bool IsGameOver { get; private set; }

        public IPlayerInfo Winner { get; private set; }

        /// <summary>
        /// Tries to make a move based on two indices numbers.
        /// Equal 'from' and 'to' numbers, determines a special move request, like folding out or exit from jail.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>True if success, otherwise, false.</returns>
        public bool MakeMove(MoveHolder moveHolder)
        {
            if (IsGameOver) return false;

            var requestedMove = new MoveDescription(moveHolder.From, moveHolder.To, _currentPlayer.Direction, _currentPlayer.Status, _currentPlayer.PlayerId);

            if (!CanMakeMove(requestedMove))
            {
                return false;
            }

            if (requestedMove.PlayerStatus == PlayerStatus.Playing || requestedMove.From != requestedMove.To)
            {
                _board.Remove(moveHolder.From);
                _board.TryAdd(moveHolder.To, requestedMove.PlayerId);
            }
            else if (requestedMove.PlayerStatus == PlayerStatus.FoldingOut)
            {
                _board.Remove(moveHolder.From);
            }
            else if (requestedMove.PlayerStatus == PlayerStatus.InJail)
            {
                _board.ExitFromJail(moveHolder.To, requestedMove.PlayerId);
            }

            RemoveDiceValue(requestedMove);
            CheckPlayersStatuses();
            SwitchPlayer();
            CheckGameOver();
            ComputePossibleMoves();
            OnBoardStateChanged();

            return true;
        }

        private void RemoveDiceValue(MoveDescription requestedMove)
        {
            if(requestedMove.From != requestedMove.To)
            {
                _currentDice.Remove(Math.Abs(requestedMove.From - requestedMove.To));
            }
            else
            {
                if (requestedMove.PlayerStatus == PlayerStatus.FoldingOut)
                {
                    int die = requestedMove.Direction == MoveDirection.Left ? requestedMove.From + 1 :
                                24 - requestedMove.From;
                    bool isRemoved = _currentDice.Remove(die);

                    if (!isRemoved)
                    {
                        _currentDice.Remove(_currentDice.Max());
                    } 
                }
                else
                {
                    int die = requestedMove.Direction == MoveDirection.Right ? requestedMove.From + 1 :
                        24 - requestedMove.From;
                    _currentDice.Remove(die);
                }
            }
        }

        public IEnumerable<int> GetDiceValues => _dice.Values;

        public IJail GetJail() => _board;

        public IEnumerable<PointInfo> Points => _board;

        public IEnumerable<IMoveDescription> GetPossibleMoves()
        {
            foreach (var move in _possibleMoves)
            {
                yield return move;
            }
        }

        private void CheckGameOver()
        {
            if (!_board.IsInBoard(_playerOne.PlayerId))
            {
                IsGameOver = true;
                Winner = _playerOne;
            }
            else if(!_board.IsInBoard(_playerTwo.PlayerId))
            {
                IsGameOver = true;
                Winner = _playerTwo;
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
            int from = player.Direction == MoveDirection.Left ? 6 : 0;
            int to = player.Direction == MoveDirection.Left ? 23 : 17;

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
                Roll();
            }
        }

        private void Roll()
        {
            _dice.Roll();
            _currentDice = new List<int>(_dice.Values);
        }

        private void ComputePossibleMoves()
        {
            _movesCalculator = new MovesCalculator(_board.Points);
            _possibleMoves = _movesCalculator.GetPossibleMoves(_currentPlayer, _currentDice);

            if(!_possibleMoves.Any())
            {
                OnNoPossibleMoves();
                _currentDice.Clear();
                SwitchPlayer();
                ComputePossibleMoves();
            }
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

        private void OnNoPossibleMoves()
        {
            NoPossibleMoves?.Invoke(new PlayerInfo(
                _currentPlayer.Name, 
                _currentPlayer.PlayerId, 
                _currentPlayer.IsHuman), 
                new List<int>(_currentDice));
        }
    }
}
