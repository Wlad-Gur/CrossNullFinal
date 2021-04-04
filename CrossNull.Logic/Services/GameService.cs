using CrossNull.Data;
using CrossNull.Domain;
using CrossNull.Logic.Models;
using CrossNull.Models;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
        /// Loads a batch of games with the corresponding Id
        /// </summary>
        /// <param name="id"> Id of the required game </param>
        /// <returns> Returns the desired game </returns>
        public Result<GameModel> Load(int id)
        {
            if (id <= 0)
            {
                var r = Result.Failure<GameModel>("Id is incorrect. Id can't be less or equal 0.");

                return r;
            }
            try
            {
                var modelGame = _db.Games.SingleOrDefault(s => s.Id == id);
                if (modelGame == null)
                {
                    return Result.Success<GameModel>(null);
                }
                return JsonConvert.DeserializeObject<GameModel>(modelGame.Game);
            }
            catch (DBConcurrencyException)
            {
                return Result.Failure<GameModel>("Two or more users try to change a record");
            }
            catch (DataException)
            {
                return Result.Failure<GameModel>("Connection doesn't fail");
            }
        }

        /// <summary>
        /// Method loads a collection of all played games from the database
        /// </summary>
        /// <returns>Dictionary with ID and list of games</returns>
        public Dictionary<int, GameModel> LoadAll()
        {
            Dictionary<int, GameModel> gamesDict = new Dictionary<int, GameModel>();
            //TODO at home подумать как оптимизировать код с помощью LINQ Select , toDictionary
            var modelGame = _db.Games.ToList();
            foreach (var item in modelGame)
            {
                var game = JsonConvert.DeserializeObject<GameModel>(item.Game);
                gamesDict.Add(item.Id, game);
            }
            return gamesDict;
        }
        /// <summary>
        /// Start new game
        /// </summary>
        /// <param name="playerOne"> Nickname first player</param>
        /// <param name="playerTwo"> Nickname second player</param>
        /// <returns>Returns model of game</returns>
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
        /// <summary>
        ///
        /// </summary>
        /// <param name="gameModels"></param>
        /// <returns></returns>
        public GameModel Step(GameModel gameModels)
        {

            throw new NotImplementedException();
        }

        private void SaveStep(GameModel _gameProg)
        {
            if (_gameProg.Id <= 0)
            {
                SaveFirstStep(_gameProg);
                return;
            }
            SaveOrdinaryStep(_gameProg);
        }
        private void SaveFirstStep(GameModel _gameProg)
        {
            _gameStateDb = new GameStateDb();
            _db.Games.Add(_gameStateDb);
            _db.SaveChanges();
            _gameProg.Id = _gameStateDb.Id;
            _gameStateDb.Game = JsonConvert.SerializeObject(_gameProg);
            _db.SaveChanges();
        }
        private void SaveOrdinaryStep(GameModel _gameProg)
        {
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
