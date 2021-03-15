using DustInTheWind.ConsoleTools.Menues;
using System;

namespace CrossNull
{
    internal class NewGame : ICommand
    {
        private Action _superPlay;

        public NewGame(Action superPlay)
        {
            if (superPlay == null)
            {
                throw new ArgumentNullException("Object requires in argument superPlay");
            }

            this._superPlay = superPlay;
        }

        public bool IsActive => true;

        public void Execute()
        {
            _superPlay.Invoke();
        }
    }
}