using System;

namespace CrossNull.Domain
{
    public class FieldFullEventArgs : EventArgs
    {
        public FieldFullEventArgs(bool win = true)
        {
            Win = win;
        }

        public bool Win { get; private set; }
    }
}
