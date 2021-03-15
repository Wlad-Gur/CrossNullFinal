using System;

namespace CrossNull.Domain
{
    public class StepEventArgs : EventArgs
    {
        public StepEventArgs(Cell cell)
        {
            Cell = cell;
        }
        public Cell Cell { get; private set; }

    }
}
