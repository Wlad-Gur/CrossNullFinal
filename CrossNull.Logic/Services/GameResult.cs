using CrossNull.Logic.Models;

namespace CrossNull.Logic.Services
{
    public class GameResult
    {
        public GameResult(GameSituation situation, GameModel model)
        {
            Situation = situation;
            Model = model;
        }
        public GameSituation Situation { get; private set; }
        public GameModel Model { get; private set; }

    }
}

