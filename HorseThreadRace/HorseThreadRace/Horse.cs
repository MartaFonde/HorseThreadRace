using System;
using System.Collections.Generic;
using System.Text;

namespace HorseThreadRace
{
    class Horse
    {
        public int Posicion { set; get; }
        public int Dorsal { set;get; }
        public int Row { set; get; }

        public Horse(int row, int dorsal)
        {
            Posicion = 0;
            Row = row;
            Dorsal = dorsal;
        }

        public int run()
        {
            Random n = new Random();
            return n.Next(1, 6);           
        }
    }
}
