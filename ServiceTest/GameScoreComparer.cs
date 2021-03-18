using CrossNull.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTest
{
    class GameScoreComparer : IEqualityComparer<GameScore>
    {
        public bool Equals(GameScore x, GameScore y)//проверка на налл
        {
            if (x == null || y == null)  //try cach is nessesary?
            {
                throw new NullReferenceException("Object in method Equals don't cteate.");
            }
            return (x.CountWin == y.CountWin && x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase));
        }

        public int GetHashCode(GameScore obj)
        {
            if (obj == null) throw new NullReferenceException("Object in method GetHashCode don't cteate.");
            return (obj.Name.GetHashCode() ^ obj.CountWin.GetHashCode());
        }
    }
}
