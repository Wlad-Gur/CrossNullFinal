using CrossNull.Domain;
using DustInTheWind.ConsoleTools.InputControls;
using DustInTheWind.ConsoleTools.Menues;
using System;

namespace CrossNull
{
    internal class LoadGameCommand : ICommand
    {
        private Action _loadGameList;

        public LoadGameCommand(Action loadGameList)
        {
            if (loadGameList == null)
            {
                throw new ArgumentException("Object requires argument loadGameList");
            }
            this._loadGameList = loadGameList;
        }

        public bool IsActive => true;

        public void Execute()
        {
            YesNoQuestion yesNoQuestion = new YesNoQuestion("Do you want to play previous game?");
            YesNoAnswer answer = yesNoQuestion.ReadAnswer();

            if (answer.Equals(YesNoAnswer.Yes))
            {
                _loadGameList();
            }
        }
    }
}