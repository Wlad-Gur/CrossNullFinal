﻿using CrossNull.Data;
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
        public Result<Dictionary<int, GameModel>> LoadAll()
        {
            Dictionary<int, GameModel> gamesDict = new Dictionary<int, GameModel>();
            try
            {

                //TODO at home подумать как оптимизировать код с помощью LINQ Select , toDictionary
                var modelGame = _db.Games.ToList();
                foreach (var item in modelGame)
                {
                    var game = JsonConvert.DeserializeObject<GameModel>(item.Game);
                    if (game == null)
                    {
                        continue;
                    }
                    gamesDict.Add(item.Id, game);
                }
                return gamesDict;
            }
            catch (Exception ex) when (ex is DBConcurrencyException || ex is DataException)
            {
                return Result.Failure<Dictionary<int, GameModel>>("Can't connect to database");
            }

        }
        /// <summary>
        /// Start new game
        /// </summary>
        /// <param name="playerOne"> Nickname first player</param>
        /// <param name="playerTwo"> Nickname second player</param>
        /// <returns>Returns model of game</returns>
        public Result<GameModel> StartNew(Player playerOne, Player playerTwo)
        {
            if (playerOne == null)
            {
                return Result.Failure<GameModel>("Incorrect name first player");
            }
            if (playerTwo == null)
            {
                return Result.Failure<GameModel>("Incorrect name second player");
            }
            _gameProg = new GameModel();
            _gameProg.PlayerOne = playerOne;
            _gameProg.PlayerActive = playerOne;
            _gameProg.PlayerTwo = playerTwo;
            Result result = SaveStep(_gameProg);
            if (result.IsSuccess)
            {
                return Result.Success<GameModel>(_gameProg);
            }
            return Result.Failure<GameModel>(result.Error);
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
        public Result<GameModel> Step(GameModel gameModels)
        {

            throw new NotImplementedException();
        }

        private Result SaveStep(GameModel _gameProg)
        {
            if (_gameProg.Id <= 0)
            {
                return SaveFirstStep(_gameProg);

            }
            return Result.Success(SaveOrdinaryStep(_gameProg));
        }
        private Result SaveFirstStep(GameModel _gameProg)
        {
            try
            {
                _gameStateDb = new GameStateDb();
                _db.Games.Add(_gameStateDb);
                _db.SaveChanges();
                _gameProg.Id = _gameStateDb.Id;
                _gameStateDb.Game = JsonConvert.SerializeObject(_gameProg);
                _db.SaveChanges();
            }
            catch (DBConcurrencyException)
            {
                return Result.Failure("Two or more users try to change a record");
            }
            catch (DataException)
            {
                return Result.Failure("Connection doesn't fail");
            }
            return Result.Success();
        }
        private Result SaveOrdinaryStep(GameModel _gameProg)
        {
            try
            {
                var model = _db.Games.Find(_gameProg.Id);
                if (model == null)
                {
                    throw new ArgumentException("Id doesn't exist.");
                }
                model.Game = JsonConvert.SerializeObject(_gameProg);
                _db.SaveChanges();
            }
            catch (DBConcurrencyException)
            {
                return Result.Failure("Two or more users try to change a record");
            }
            catch (DataException)
            {
                return Result.Failure("Connection doesn't fail");
            }
            return Result.Success();
        }
    }
}
