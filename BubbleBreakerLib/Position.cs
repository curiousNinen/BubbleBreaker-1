using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleBreakerLib
{
    public class Position
    {
        private int _zeile;
        private int _spalte;
        public int Zeile { get { return _zeile; } set { _zeile = value; } }
        public int Spalte { get { return _spalte; } set { _spalte = value; } }
        public int Top { get { return _zeile; } set { _zeile = value; } }
        public int Left { get { return _spalte; } set { _spalte = value; } }

        public Position(Position source)
        {
            _zeile = source._zeile;
            _spalte = source._spalte;
        }

        public Position(int ZeileTop = 0, int SpalteLeft = 0)
        {
            _zeile = ZeileTop;
            _spalte = SpalteLeft;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return this._zeile == ((Position)obj)._zeile && this._spalte == ((Position)obj)._spalte;
        }

       
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return base.GetHashCode();
        }

        public override string ToString() => $"{_zeile},{_spalte}";
    }
}
