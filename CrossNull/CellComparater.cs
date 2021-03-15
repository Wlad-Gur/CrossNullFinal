using CrossNull.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull
{
    class CellComparater : IEqualityComparer<Cell>
    {
        public bool Equals(Cell x, Cell y)
        {
            return (x.X == y.X) && (x.Y == y.Y);
        }

        public int GetHashCode(Cell obj)
        {
            return obj.GetHashCode();
        }
    }
}
