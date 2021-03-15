using CrossNull.Data;
using CrossNull.Domain;
using DustInTheWind.ConsoleTools.Menues;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CrossNull.Commands
{
    class SaveGameCommand : ICommand
    {
        private Action<Game> _actionSave;
        private Game _game;
        public SaveGameCommand(Action<Game> action, Game game)
        {
            if (action == null)
            {
                throw new ArgumentException("Object SaveGameCommand must have action");
            }
            if (game == null)
            {
                throw new ArgumentException("Object SaveGameCommand must have object game");
            }
            _actionSave = action;
            _game = game;
            //
        }
        public bool IsActive => true;

        public void Execute()
        {
            _actionSave.Invoke(_game);
        }
    }
}
