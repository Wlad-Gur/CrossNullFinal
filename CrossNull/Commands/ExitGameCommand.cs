using DustInTheWind.ConsoleTools.Menues;
using System;

namespace CrossNull
{
    internal class ExitGame : ICommand
    {
        private Action _exitGame;

        public ExitGame(Action exitGame)
        {
            if (exitGame == null)
            {
                throw new ArgumentNullException("Object requires argument exitGame");
            }
            this._exitGame = exitGame;
        }

        public bool IsActive => true;

        public void Execute()
        {
            _exitGame();
        }
    }
}