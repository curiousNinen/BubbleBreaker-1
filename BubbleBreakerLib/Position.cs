using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBreakerLib
{
    public struct Position
    {
        private int _zeile;
        private int _spalte;
        public int Zeile { get { return _zeile; } set { _zeile = value; } }
        public int Spalte { get { return _spalte; } set { _spalte = value; } }
        public int Top { get { return _zeile; } set { _zeile = value; } }
        public int Left { get { return _spalte; } set { _spalte = value; } }
    }
}
