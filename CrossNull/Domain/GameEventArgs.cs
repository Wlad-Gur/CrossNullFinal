using System;

namespace CrossNull.Domain
{
    public class GameEventArgs : EventArgs
    {
        public GameEventArgs(CellStates cellState, bool win)
        {
            CellState = cellState;
            Win = win;
        }
        public CellStates CellState { get; set; }

        public bool Win { get; set; }
    }
}
