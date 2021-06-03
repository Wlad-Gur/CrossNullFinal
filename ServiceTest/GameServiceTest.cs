using System;
using System.Collections.Generic;
using CrossNull.Data;
using CrossNull.Domain;
using CrossNull.Logic.Models;
using CrossNull.Logic.Services;
using CrossNull.Models;
using EntityFrameworkMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceTest
{
    [TestClass]
    public class GameServiceTest
    {
        IEnumerable<Cell> cells;

        Player _playerOne = new Player(PlayerTypes.X, "One");
        GameState gameState = new GameState();
        GameModel _gameModel = new GameModel()
        {
            Id = 1,
            PlayerOne = new Player(PlayerTypes.X, "One"),
            PlayerTwo = new Player(PlayerTypes.O, "Two"),
            PlayerActive = new Player(PlayerTypes.X, "One"),

        };
        [TestMethod]
        public void StepTestReturnGameContinue()
        {
            //Arrange
            var mockDBContext = new DbContextMock<GameContext>("wwwgame");
            GameService gameService = new GameService(mockDBContext.Object);
            //Act
            //var expected = gameService.NextTurn(_gameModel, 1, 1).;

        }
    }
}
