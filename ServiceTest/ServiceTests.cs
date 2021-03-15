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
