namespace CrossNull.Domain
{
    public class Cell
    {
        public Cell(CellStates state, int x, int y)
        {
            State = state;
            X = x;
            Y = y;
        }
        public CellStates State { get; }

        public int X { get; }

        public int Y { get; }

        public override string ToString()
        {
            return State == CellStates.O ? "O" : "X";
        }
    }
}
