using CrossNull.Logic.Models;
using System.Collections.Generic;

namespace CrossNull.Logic.Services
{
    public interface IStatisticService
    {
        IEnumerable<GameScore> LoadGameScoreStatistic();
        Dictionary<int, GameModel> LoadGameStatistic();
    }
}