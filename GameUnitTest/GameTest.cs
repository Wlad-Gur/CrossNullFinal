using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossNull.Domain;
using System.Collections.Generic;

namespace GameUnitTest
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void TestOverToString()
        {
            //Arrange
            IEnumerable<Cell> cellsIsNull = null;
            var gameField = new GameState(cellsIsNull);
            //Act
            string result = gameField.ToString();
            string expected = $"? | ? | ?{Environment.NewLine}? | ? | ?{Environment.NewLine}? | ? | ?";
            //Assert
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void RandomGameTest()
        {
            //Arrange
            IEnumerable<Cell> cellsIsNull = new List<Cell>() { new Cell(CellStates.X, 0, 0),
                new Cell(CellStates.X, 0, 1), new Cell(CellStates.O, 1, 2) };
            var gameField = new GameState(cellsIsNull);
            //Act
            string result = gameField.ToString();
            string expected = $"X | X | ?{Environment.NewLine}? | ? | O{Environment.NewLine}? | ? | ?";
            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ColumnGameTest()
        {
            //Arrange
            IEnumerable<Cell> columnIsWin = new List<Cell>() { new Cell(CellStates.X, 0, 0),
                new Cell(CellStates.X, 1, 0), new Cell(CellStates.O, 1, 2),
                new Cell(CellStates.O, 2, 2), new Cell(CellStates.X, 2, 0),};
            var gameField = new GameState(columnIsWin);
            //Act
            string result = gameField.ToString();
            string expected = $"X | ? | ?{Environment.NewLine}X | ? | O{Environment.NewLine}X | ? | O";
            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]

        public void EventTest()
        {
            //Arrange
            Game game = new Game();
            game.AddCell(new Cell(CellStates.X, 0, 0));
            game.AddCell(new Cell(CellStates.O, 1, 2));
            game.AddCell(new Cell(CellStates.X, 1, 0));
            game.AddCell(new Cell(CellStates.O, 2, 2));
            GameEventArgs gameEventArgs = null;
            game.OnGameOver += (obj, args) => gameEventArgs = args;
            //Act
            game.AddCell(new Cell(CellStates.X, 2, 0));

            //Assert
            Assert.IsNotNull(gameEventArgs);
            Assert.IsTrue(gameEventArgs.Win);
            Assert.AreEqual(gameEventArgs.CellState, CellStates.X);
        }

        [TestMethod]
        public void EventTestWhenWin_O()
        {
            //Arrange
            Game game = new Game();
            game.AddCell(new Cell(CellStates.O, 0, 0));
            game.AddCell(new Cell(CellStates.X, 1, 2));
            game.AddCell(new Cell(CellStates.O, 1, 0));
            game.AddCell(new Cell(CellStates.X, 2, 2));
            GameEventArgs gameEventArgs = null;
            game.OnGameOver += (obj, args) => gameEventArgs = args;

            //Act
            game.AddCell(new Cell(CellStates.O, 2, 0));

            //Assert
            Assert.IsNotNull(gameEventArgs);
            Assert.IsTrue(gameEventArgs.Win);
            Assert.AreEqual(gameEventArgs.CellState, CellStates.O);
        }

        [TestMethod]
        public void EventTestStringWinX()
        {
            //Arrange
            Game game = new Game();
            game.AddCell(new Cell(CellStates.X, 1, 0));
            game.AddCell(new Cell(CellStates.O, 0, 0));
            game.AddCell(new Cell(CellStates.X, 1, 1));
            game.AddCell(new Cell(CellStates.O, 2, 2));
            GameEventArgs gameEventArgs = null;
            game.OnGameOver += (obj, args) => gameEventArgs = args;

            //Act
            game.AddCell(new Cell(CellStates.X, 1, 2));

            //Assert
            Assert.IsNotNull(gameEventArgs);
            Assert.IsTrue(gameEventArgs.Win);
            Assert.AreEqual(gameEventArgs.CellState, CellStates.X);

        }

        [TestMethod]
        public void EventTestStringWhenWinO()
        {
            //Arrange
            Game game = new Game();
            game.AddCell(new Cell(CellStates.X, 0, 2));
            game.AddCell(new Cell(CellStates.O, 2, 0));
            game.AddCell(new Cell(CellStates.X, 0, 0));
            game.AddCell(new Cell(CellStates.O, 2, 1));
            game.AddCell(new Cell(CellStates.X, 1, 1));
            GameEventArgs gameEventArgs = null;
            game.OnGameOver += (obj, args) => gameEventArgs = args;

            //Act
            game.AddCell(new Cell(CellStates.O, 2, 2));

            //Assert
            Assert.IsNotNull(gameEventArgs);
            Assert.IsTrue(gameEventArgs.Win);
            Assert.AreEqual(gameEventArgs.CellState, CellStates.O);
        }

        [TestMethod]
        public void EventTestDiagonalEqual()
        {
            //Arrange
            Game game = new Game();
            game.AddCell(new Cell(CellStates.X, 0, 0));
            game.AddCell(new Cell(CellStates.O, 0, 1));
            game.AddCell(new Cell(CellStates.X, 1, 1));
            game.AddCell(new Cell(CellStates.O, 1, 0));
            GameEventArgs gameEventArgs = null;
            game.OnGameOver += (obj, args) => gameEventArgs = args;

            //Act
            game.AddCell(new Cell(CellStates.X, 2, 2));

            //Assert
            Assert.IsNotNull(gameEventArgs);
            Assert.IsTrue(gameEventArgs.Win);
            Assert.AreEqual(gameEventArgs.CellState, CellStates.X);
        }

        [TestMethod]
        public void EventTestDiagonalNext()
        {
            //Arrange
            Game game = new Game();
            game.AddCell(new Cell(CellStates.X, 0, 2));
            game.AddCell(new Cell(CellStates.O, 0, 1));
            game.AddCell(new Cell(CellStates.X, 1, 1));
            game.AddCell(new Cell(CellStates.O, 1, 0));
            GameEventArgs gameEventArgs = null;
            game.OnGameOver += (obj, args) => gameEventArgs = args;

            //Act
            game.AddCell(new Cell(CellStates.X, 2, 0));

            //Assert
            Assert.IsNotNull(gameEventArgs);
            Assert.IsTrue(gameEventArgs.Win);
            Assert.AreEqual(gameEventArgs.CellState, CellStates.X);
        }

        [TestMethod]
        public void EventTestFieldIsFull()
        {
            //Arrange
            Game game = new Game();
            game.AddCell(new Cell(CellStates.X, 0, 0));
            game.AddCell(new Cell(CellStates.O, 0, 1));
            game.AddCell(new Cell(CellStates.X, 0, 2));
            game.AddCell(new Cell(CellStates.O, 1, 0));
            game.AddCell(new Cell(CellStates.X, 1, 1));
            game.AddCell(new Cell(CellStates.O, 1, 2));
            game.AddCell(new Cell(CellStates.O, 2, 0));
            game.AddCell(new Cell(CellStates.X, 2, 1));
            FieldFullEventArgs fieldFullEventArgs = null;
            game.OnFieldFull += (obj, args) => fieldFullEventArgs = args;

            //Act
            game.AddCell(new Cell(CellStates.O, 2, 2));

            //Assert
            Assert.IsNotNull(fieldFullEventArgs);
            Assert.IsFalse(fieldFullEventArgs.Win);
        }
    }
}
