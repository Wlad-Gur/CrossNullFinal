using CrossNull.Logic.Models;
using CrossNull.Models;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using static CrossNull.Logic.Services.GameService;

namespace CrossNull.Logic.Services
{
    public interface IGameService
    {
        Result<GameModel> StartNew(Player playerOne, Player playerTwo);
        Result<GameModel> Load(int id);
        Result<GameResult> NextTurn(GameModel gameModels, int colum, int line);
        Result<Dictionary<int, GameModel>> LoadAll();
    }
}