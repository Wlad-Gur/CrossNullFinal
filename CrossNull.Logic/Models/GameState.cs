using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossNull.Domain
{
    public class GameState
    {
        public IEnumerable<Cell> Cells { get; set; }

        public GameState() : this(null)   //This constructor is nessesary for serialization?
        {

        }
        public GameState(IEnumerable<Cell> cells)
        {
            this.Cells = cells ?? Enumerable.Empty<Cell>();
        }
        public override string ToString()
        {
            IEnumerable<(int, string)> result = Cells.GroupBy(g => g.X).OrderBy(g => g.Key).
                Select(s => (s.Key, string.Format("{0} | {1} | {2}",
                s.SingleOrDefault(r => r.Y == 0)?.ToString() ?? "?",
                s.SingleOrDefault(r => r.Y == 1)?.ToString() ?? "?",
                s.SingleOrDefault(r => r.Y == 2)?.ToString() ?? "?")));



            IList<string> ret = new List<string>();
            ret.Add((result.SingleOrDefault(r => r.Item1 == 0)).Item2 ?? "? | ? | ?");
            ret.Add((result.SingleOrDefault(r => r.Item1 == 1)).Item2 ?? "? | ? | ?");
            ret.Add((result.SingleOrDefault(r => r.Item1 == 2)).Item2 ?? "? | ? | ?");

            return string.Join(Environment.NewLine, ret);
        }
    }
}
