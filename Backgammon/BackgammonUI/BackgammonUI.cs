using BackgammonGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackgammonUI
{
    public class BackgammonUI
    {
        private BackgammonGameManager _game = new BackgammonGameManager("mama", "daddy");
        private StringBuilder _builder = new StringBuilder();
        private ConsoleColor _defaultColor = Console.ForegroundColor;

        public BackgammonUI()
        {
            _game.BoardStateChanged += PrintBoard;
            _game.NoPossibleMoves += NotifyNoPossibleMoves;
        }

        public void Start()
        {
            PrintBoard();

            while (!_game.IsGameOver)
            {
                PlayerProcedure();
            }
        }

        private void PlayerProcedure()
        {
            bool isOk = false;
            MoveHolder move;

            while (!isOk)
            {
                move = GetPlayerMove();
                isOk = _game.MakeMove(move.From, move.To);

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

        private void NotifyNoPossibleMoves(PlayerInfo player, IEnumerable<int> dice)
        {
            Console.WriteLine($"{Environment.NewLine}Player {player.Name} has no possible moves for his dice!");

            foreach (var die in dice)
            {
                Console.Write(die +  ", ");
            }

            Thread.Sleep(2000);
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
                _builder.AppendFormat($"{representativeChar, 4}");
                representativeChar++;
            }

            Console.WriteLine(_builder.ToString());
            Console.WriteLine();

            for (int i = 0; i < 12; i++)
            {
                Console.ForegroundColor = GetColorByPlayer(points[i].Player);
                Console.Write($"{points[i].Size, 4}");
            }

            Console.WriteLine();
            Console.WriteLine();

            for (int i = 23; i > 11; i--)
            {
                Console.ForegroundColor = GetColorByPlayer(points[i].Player);
                Console.Write($"{points[i].Size,4}");
            }

            Console.WriteLine();
            Console.ForegroundColor = _defaultColor;
            _builder.Clear();
            representativeChar = 'X';

            for (int i = 23; i > 11; i--)
            {
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
