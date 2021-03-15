using CrossNull.Logic.Models;
using CrossNull.Models;
using System.Collections.Generic;

namespace CrossNull.Logic.Services
{
    interface IGameService
    {
        GameModel StartNew(Player playerOne, Player playerTwo);
        GameModel Load(int id);
        GameModel Step(GameModel gameModels);
        Dictionary<int, GameModel> LoadAll();
    }
}