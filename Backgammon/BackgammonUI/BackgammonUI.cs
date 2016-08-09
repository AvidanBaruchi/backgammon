using BackgammonGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BackgammonGame
{
    public class BackgammonUI
    {
        private BackgammonGameManager _game = null;
        private StringBuilder _builder = new StringBuilder();
        private ConsoleColor _defaultColor = Console.ForegroundColor;
        private BackgammonDefaultAI _artificialIntelligence;
        private GameType _gameType;

        public BackgammonUI()
        {
            
        }

        public void Start()
        {
            PrepareGame();
            PrintBoard();

            while (!_game.IsGameOver)
            {
                if(_game.CurrentPlayer.IsHuman)
                {
                    HumanPlayerProcedure();
                }
                else
                {
                    AIProcedure();
                }
            }

            DisplayWinner();
        }

        private void PrepareGame()
        {
            string firstPlayerName = string.Empty;
            string secondPlayerName = string.Empty;

            Console.WriteLine("Please Enter First Player Name:");
            firstPlayerName = GetString();
            GetGameType();

            if(_gameType == GameType.TwoPlayers)
            {
                Console.WriteLine("Please Second First Player Name:");
                secondPlayerName = GetString();
            }
            else
            {
                secondPlayerName = "OK Computer";
            }

            _game = new BackgammonGameManager(firstPlayerName, secondPlayerName, _gameType);
            _game.BoardStateChanged += PrintBoard;
            _game.NoPossibleMoves += NotifyNoPossibleMoves;
            _artificialIntelligence = new BackgammonDefaultAI();
        }

        private void GetGameType()
        {
            Console.WriteLine("Select Game Type:");
            var gameTypes = Enum.GetNames(typeof(GameType));
            string parsedName = null;

            for (int i = 1; i <= gameTypes.Length; i++)
            {
                parsedName = Regex.Replace(gameTypes[i - 1], "[A-Z]", (Match match) =>
                {
                    return " " + match.Value;
                });
                Console.WriteLine($"{i}. {parsedName}");
            }

            bool isSelected = false;
            string input = null;
            int typeNumber = 0;

            while (!isSelected)
            {
                input = GetString();
                isSelected = int.TryParse(input, out typeNumber);

                if (typeNumber < 1 || (typeNumber > gameTypes.Length))
                {
                    isSelected = false;
                }

                if(!isSelected)
                {
                    Console.WriteLine("Not a Valid Choice");
                }
            }

            Enum.TryParse(gameTypes[typeNumber - 1], out _gameType);
        }

        private string GetString()
        {
            string input = string.Empty;

            while (string.IsNullOrEmpty(input))
            {
                input = Console.ReadLine();

                input = input.Trim();

                if(string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Not a Valid Name");
                }
            }

            return input;
        }

        private void AIProcedure()
        {
            MoveHolder holder = _artificialIntelligence.GetMove(_game.GetPossibleMoves(), _game.Points);
            Thread.Sleep(1500);
            _game.MakeMove(holder);
        }

        private void HumanPlayerProcedure()
        {
            bool isOk = false;
            MoveHolder move;

            while (!isOk)
            {
                move = GetPlayerMove();
                isOk = _game.MakeMove(move);

                if(!isOk)
                {
                    Console.WriteLine("Wrong Move!");
                }
            }
        }

        private MoveHolder GetPlayerMove()
        {
            bool isParsed = false;
            string input = string.Empty;
            MoveHolder move = new MoveHolder();

            Console.WriteLine("Please select a move: (AZ, KB for regular move, X for exit from jail of folding out)");

            while(!isParsed)
            {
                input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) continue;

                if(input.Length == 1)
                {
                    input = input + input;
                }

                input = input.ToUpper();

                if (char.IsLetter(input[0]) && char.IsLetter(input[1]))
                {
                    move.From = input[0] - 'A';
                    move.To = input[1] - 'A';
                    isParsed = true;
                }

                if(!isParsed)
                {
                    Console.WriteLine("Please Try Again!");
                }
            }

            return move;
        }

        private void DisplayWinner()
        {
            var winner = _game.Winner;
            Console.Clear();
            Console.ForegroundColor = GetColorByPlayer(winner.PlayerId);
            Console.WriteLine($"The Winner is {winner.Name}");
        }

        private void NotifyNoPossibleMoves(IPlayerInfo player, IEnumerable<int> dice)
        {
            Console.WriteLine($"{Environment.NewLine}Player {player.Name} has no possible moves for his dice!");

            foreach (var die in dice)
            {
                Console.Write(die +  ", ");
            }

            Thread.Sleep(4000);
        }

        private void PrintBoard()
        {
            var points = _game.Points.ToArray();
            var jail = _game.GetJail();
            char representativeChar = 'A';

            Console.Clear();
            Console.ForegroundColor = _defaultColor;
            _builder.Clear();

            for (int i = 0; i < 12; i++)
            {
                if(i == 6)
                {
                    _builder.Append($"{"| |", 6}");
                }

                _builder.AppendFormat($"{representativeChar, 4}");
                representativeChar++;
            }

            Console.WriteLine(_builder.ToString());
            Console.WriteLine();

            for (int i = 0; i < 12; i++)
            {
                if(i == 6)
                {
                    Console.ForegroundColor = _defaultColor;
                    Console.Write($"{"| |", 6}");
                }

                Console.ForegroundColor = GetColorByPlayer(points[i].Player);
                Console.Write($"{points[i].Size, 4}");
            }

            Console.WriteLine();
            Console.WriteLine();

            for (int i = 23; i > 11; i--)
            {
                if (i == 17)
                {
                    Console.ForegroundColor = _defaultColor;
                    Console.Write($"{"| |", 6}");
                }

                Console.ForegroundColor = GetColorByPlayer(points[i].Player);
                Console.Write($"{points[i].Size,4}");
            }

            Console.WriteLine();
            Console.ForegroundColor = _defaultColor;
            _builder.Clear();
            representativeChar = 'X';

            for (int i = 23; i > 11; i--)
            {
                if (i == 17)
                {
                    _builder.Append($"{"| |", 6}");
                }
                _builder.AppendFormat($"{representativeChar,4}");
                representativeChar--;
            }

            Console.WriteLine();
            Console.WriteLine(_builder.ToString());
            Console.WriteLine();

            PrintJail();
            PrintCurrentPlayerAndDice();
        }

        private void PrintJail()
        {
            var jail = _game.GetJail();
            Console.Write($"Jail ==> ");

            if(jail.IsInJail(PlayerId.One))
            {
                Console.ForegroundColor = GetColorByPlayer(PlayerId.One);
                Console.Write($"{jail.GetJailCount(PlayerId.One)} ");
            }

            if (jail.IsInJail(PlayerId.Two))
            {
                Console.ForegroundColor = GetColorByPlayer(PlayerId.Two);
                Console.Write($"{jail.GetJailCount(PlayerId.Two)}");
            }

            Console.ForegroundColor = _defaultColor;
            Console.WriteLine();
        }

        private void PrintCurrentPlayerAndDice()
        {
            var dice = _game.GetDiceValues.ToArray();
            Console.ForegroundColor = GetColorByPlayer(_game.CurrentPlayer.PlayerId);
            Console.WriteLine($"Current Player: {_game.CurrentPlayer.Name}");
            Console.ForegroundColor = _defaultColor;
            Console.Write("Dices: ");

            for (int i = 0; i < dice.Length; i++)
            {
                Console.Write($"{dice[i]}, ");
            }

            Console.WriteLine();
        }

        private ConsoleColor GetColorByPlayer(PlayerId player)
        {
            var color = _defaultColor;

            if (player == PlayerId.One)
            {
                color = ConsoleColor.Red;
            }
            else if(player == PlayerId.Two)
            {
                color = ConsoleColor.Green;
            }

            return color;
        }
    }
}
