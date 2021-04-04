using CrossNull.Logic.Models;
using CrossNull.Models;
using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace CrossNull.Logic.Services
{
    public interface IGameService
    {
        GameModel StartNew(Player playerOne, Player playerTwo);
        Result<GameModel> Load(int id);
        GameModel Step(GameModel gameModels);
        Dictionary<int, GameModel> LoadAll();
    }
}