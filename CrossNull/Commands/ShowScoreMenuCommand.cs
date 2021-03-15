using DustInTheWind.ConsoleTools.Menues;
using System;

namespace CrossNull
{
    internal class ShowScoreMenu : ICommand
    {
        private Action _showScore;

        public ShowScoreMenu(Action showScore)
        {
            if (showScore == null)
            {
                throw new ArgumentNullException("Object requires argument showScore");
            }
            this._showScore = showScore;
        }

        public bool IsActive => true;

        public void Execute()
        {
            _showScore();
        }
    }
}