using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CrossNull.Logic.Models;
using CrossNull.Logic.Services;
using CrossNull.Data;
using EntityFrameworkMock;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using CrossNull.Models;
using CrossNull.Domain;
using Newtonsoft.Json;

namespace ServiceTest
{
    [TestClass]
    public class ServiceTests
    {
        public ServiceTests()
        {
            _gameStateDb = Enumerable.Range(1, 4).Select(s => new GameModel() { Id = s }).
                Select(s => JsonConvert.SerializeObject(s)).
                Select((s, i) => new GameStateDb { Id = i, Game = s, UserId = "111" }).ToList();
        }
        private IEnumerable<GameScoreDb> _gameScoreDb = new List<GameScoreDb>{
            new GameScoreDb {  Id = 1, Name = "nnn", CountWin = 5 } ,
            new GameScoreDb {  Id = 2, Name = "wwww", CountWin = 8 },
            new GameScoreDb {  Id = 3, Name = "ttt", CountWin = 12 },
        };

        private IEnumerable<GameStateDb> _gameStateDb;

        //private IEnumerable<GameStateDb> _stateDbsForLoafAll = new List<GameStateDb>
        //{
        //    new GameStateDb { Id = 0, Game = "{"Id":1,"UserId":"606416f0-8a0a-40fa-929c-e4a188649307","State":{"Cells":[{"State":0,"X":0,"Y":1},{"State":1,"X":0,"Y":2},{"State":0,"X":1,"Y":1},{"State":1,"X":1,"Y":2},{"State":0,"X":2,"Y":1}]},"PlayerOne":{"Name":"11111111","PlayerType":0},"PlayerTwo":{"Name":"222222","PlayerType":1},"PlayerActive":{ "Name":"11111111","PlayerType":0}"}
        //    }
        [TestMethod]
        public void TestGameServiceSaveStep()
        {
            //Arrange
            var mockDbContext = new DbContextMock<GameContext>("222");
            var mockDbSet = mockDbContext.CreateDbSetMock(c => c.Games, _gameStateDb);
            Player playerOne =
            new CrossNull.Models.Player(CrossNull.Models.PlayerTypes.X, "One");
            Player playerTwo = new Player(CrossNull.Models.PlayerTypes.O, "Two");

            //Act
            var gameServ = new GameService(mockDbContext.Object);
            var actual = gameServ.StartNew(playerOne, playerTwo, "123");

            //Assert
            Assert.IsTrue(mockDbContext.Object.Games.Count() == 4);
            Assert.IsFalse(actual.IsSuccess);
        }
        [TestMethod]
        public void TestStatisticService()
        {
            //Arrange
            var mockDbContext = new DbContextMock<GameContext>("hhh");
            var mockDbSet = mockDbContext.CreateDbSetMock(x => x.GameScores, _gameScoreDb);

            //Act
            var statServ = new StatisticService(mockDbContext.Object);
            var actual = statServ.LoadGameScoreStatistic();
            var actualSel = _gameScoreDb.Select(s => new GameScore { Name = s.Name, CountWin = s.CountWin }).ToList();
            // var actualAny = actualSel.Except<GameScore>(actual, new GameScoreComparer());//.DefaultIfEmpty()..Any()

            //Assert
            Assert.IsTrue(actualSel.SequenceEqual(actual, new GameScoreComparer()));
            Assert.IsFalse(actualSel.Except<GameScore>(actual, new GameScoreComparer()).Any());//.DefaultIfEmpty()
            Assert.IsTrue(actual.Count() == 4);
            Assert.IsTrue(actualSel.Count() == 4);
        }

        [TestMethod]
        public void TestLoadAllSuccess()
        {
            //Arrange
            var mockContext = new DbContextMock<GameContext>("ggg");
            var mockDbSet = mockContext.CreateDbSetMock(c => c.Games, _gameStateDb);

            //Act
            var dicGames = new GameService(mockContext.Object);
            var actual = dicGames.LoadAll("111");

            //Assert
            Assert.AreEqual(actual.Value.Count, 4);

        }
    }


}
