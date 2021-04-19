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
        Result<GameModel> Step(GameModel gameModels, int colum, int line);
        Result<Dictionary<int, GameModel>> LoadAll();
    }
}