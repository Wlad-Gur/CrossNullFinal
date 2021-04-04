using CrossNull.Logic.Models;
using CrossNull.Models;
using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace CrossNull.Logic.Services
{
    public interface IGameService
    {
        Result<GameModel> StartNew(Player playerOne, Player playerTwo);
        Result<GameModel> Load(int id);
        Result<GameModel> Step(GameModel gameModels);
        Result<Dictionary<int, GameModel>> LoadAll();
    }
}