using BackgammonGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonUI
{
    public class BackgammonUI
    {
        private BackgammonGameManager _game = new BackgammonGameManager();
        private StringBuilder _builder = new StringBuilder();
        private ConsoleColor _defaultColor = Console.ForegroundColor;

        public BackgammonUI()
        {

        }

        public void Start()
        {
            while (!_game.IsGameOver)
            {
                PrintBoard();
                Console.ReadLine();
            }
        }

        private void PrintBoard()
        {
            var points = _game.Points;
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

            //foreach (var item in _game.Points)
            //{
            //    Console.WriteLine($"Index = {item.Index}, Size = {item.Size}, Player = {item.Player}");
            //}
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
                color = ConsoleColor.White;
            }

            return color;
        }
    }
}
