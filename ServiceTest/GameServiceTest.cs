using System;
using System.Collections.Generic;
using CrossNull.Domain;
using CrossNull.Logic.Models;
using CrossNull.Models;
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
            gameState,
        };
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
