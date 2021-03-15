using CrossNull.Data;
using CrossNull.Logic.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Logic.Services
{
    class StatisticService : IStatisticService
    {
        private GameContext _db;
        public StatisticService(GameContext gameContext)
        {
            _db = gameContext;
        }
        public IEnumerable<GameScore> LoadGameScoreStatistic() //LoadGameStatistic()
        {
            var score = _db.GameScores
                 .Select(s => new GameScore() { Name = s.Name, CountWin = s.CountWin }).ToList();
            return score;
        }

        public Dictionary<int, GameModel> LoadGameStatistic()
        {
            Dictionary<int, GameModel> gamesDict = new Dictionary<int, GameModel>();
            var modelGame = _db.Games.ToList();
            foreach (var item in modelGame)
            {
                var game = JsonConvert.DeserializeObject<GameModel>(item.Game);
                gamesDict.Add(item.Id, game);
            }
            return gamesDict;
        }
    }
}
