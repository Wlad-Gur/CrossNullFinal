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

namespace ServiceTest
{
    [TestClass]
    public class ServiceTests
    {
        private IEnumerable<GameScoreDb> _gameScoreDb = new List<GameScoreDb>{
            new GameScoreDb {  Id = 1, Name = "nnn", CountWin = 5 } ,
            new GameScoreDb {  Id = 2, Name = "wwww", CountWin = 8 },
            new GameScoreDb {  Id = 3, Name = "ttt", CountWin = 12 },
        };

        private IEnumerable<GameStateDb> _gameStateDb = new List<GameStateDb>
        {
            new GameStateDb { Id = 1, Game = " " }, /*{"_id":0,"State":{"Cells":[{"State":0,"X":0,"Y":0},{"State":1,"X":0,"Y":1},{"State":0,"X":0,"Y":2},{"State":1,"X":1,"Y":1}]},"PlayerOne":{"Name":"12345","PlayerType":0},"PlayerTwo":{"Name":"88888","PlayerType":1},"PlayerActive":{ "Name":"12345","PlayerType":0}}""},*/
            new GameStateDb { Id = 2, Game = "lool"},
        };

        [TestMethod]
        public void TestGameServiceSaveStep()
        {
            //Arrange
            var mockDbContext = new DbContextMock<GameContext>("222");
            var mockDbSet = mockDbContext.CreateDbSetMock(c => c.Games, _gameStateDb);
            GameModel gameModel = new GameModel()
            {
                Id = 3,
                PlayerOne =
                new CrossNull.Models.Player(CrossNull.Models.PlayerTypes.X, "One"),
                PlayerTwo = new Player(CrossNull.Models.PlayerTypes.O, "Two"),
                PlayerActive = new Player(CrossNull.Models.PlayerTypes.X, "One"),
                State =
            };

            //Act
            var gameServ = new GameService(mockDbContext.Object);
            var actual =

            //Assert
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
            Assert.IsTrue(actual.Count() == 3);
            Assert.IsTrue(actualSel.Count() == 3);


        }


    }


}
