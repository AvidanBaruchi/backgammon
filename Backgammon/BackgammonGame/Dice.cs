using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonGame
{
    internal class Dice
    {
        private Random _random = new Random();

        public Dice()
        {
            IsDouble = false;
            Values = new List<int>();
        }

        public List<int> Values { get; private set; }

        public bool IsDouble { get; private set; }

        public void Roll()
        {
            Values.Clear();

            int first = _random.Next(1, 7);
            int second = _random.Next(1, 7);

            Values.Add(first);
            Values.Add(second);

            if(first == second)
            {
                IsDouble = true;
                Values.Add(first);
                Values.Add(first);
            }
        }
    }
}
