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
        public int HashCode { get; private set; }

        public bool Equals(GameScore x, GameScore y)
        {
            return (x.CountWin == y.CountWin && x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase));
        }

        public int GetHashCode(GameScore obj)
        {
            return (obj.Name.GetHashCode() + obj.CountWin.GetHashCode());
        }
    }
}
