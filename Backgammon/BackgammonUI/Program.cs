﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackgammonGame;

namespace BackgammonGame
{
    class Program
    {
        static void Main(string[] args)
        {
            BackgammonUI ui = new BackgammonUI();

            ui.Start();

            Console.ReadLine();
        }
    }
}
