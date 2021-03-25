using CrossNull.Data;
using CrossNull.Domain;
using CrossNull.Logic.Models;
using CrossNull.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Logic.Services
{
    class GameService : IGameService
    {
        private GameModel _gameProg;
        private GameContext _db;
        private GameStateDb _gameStateDb;
        public GameService(GameContext gameContext)
        {
            _db = gameContext;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GameModel Load(int id)
        {
            Dictionary<int, GameModel> gamesDict = new Dictionary<int, GameModel>();
            var modelGame = _db.Games.ToList();
            foreach (var item in modelGame)
            {
                var game = JsonConvert.DeserializeObject<GameModel>(item.Game);
                gamesDict.Add(item.Id, game);
            }
            return gamesDict.ElementAtOrDefault(id).Value;
        }

        public Dictionary<int, GameModel> LoadAll()
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
        //TODO    SaveStep();
        public GameModel StartNew(Player playerOne, Player playerTwo)
        {
            _gameProg = new GameModel();
            _gameProg.PlayerOne = playerOne;
            _gameProg.PlayerActive = playerOne;
            _gameProg.PlayerTwo = playerTwo;
            SaveStep(_gameProg);
            return _gameProg;
        }

        private void Game_OnFieldFull(object sender, FieldFullEventArgs e)
        {
            //fieldNoFull = e.Win;
            //_gameProg = new Game();
        }

        public GameModel Step(GameModel gameModels)
        {

            throw new NotImplementedException();
        }

        private void SaveStep(GameModel _gameProg)
        {
            if (_gameProg.Id <= 0)
            {
                _gameStateDb = new GameStateDb();
                _db.Games.Add(_gameStateDb);
                _db.SaveChanges();
                _gameProg.Id = _gameStateDb.Id;
                _gameStateDb.Game = JsonConvert.SerializeObject(_gameProg);
                _db.SaveChanges();
                return;
            }

            var model = _db.Games.Find(_gameProg.Id);
            if (model == null)
            {
                throw new ArgumentException("Id doesn't exist.");
            }

            model.Game = JsonConvert.SerializeObject(_gameProg);
            _db.SaveChanges();

        }
    }
}
