using System;

namespace CrossNull.Domain
{
    public class GameSaveEventArgs : EventArgs
    {
        public GameSaveEventArgs(ConsoleKey consoleKey = ConsoleKey.Escape)
        {
            ConsoleKeyForSave = consoleKey;
        }

        public ConsoleKey ConsoleKeyForSave { get; private set; }
    }
}