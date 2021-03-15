using CrossNull.Data;
using CrossNull.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Domain
{
    public class Game
    {
        public int _id;
        public Game()
        {

        }
        public Game(GameState gameState)
        {
            State = gameState;

        }
        public Game(Player playerOne, Player playerTwo)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
        }
        public GameState State { get; private set; } = new GameState(Enumerable.Empty<Cell>());
        public Player PlayerOne { get; set; }
        public Player PlayerTwo { get; set; }
        public Player PlayerActive { get; set; }

        private bool CanContinue() => (State.Cells.Count() > 3);


        public void AddCell(Cell cell)
        {

            OnStepExecuted?.Invoke(this, new StepEventArgs(cell));

            var cells = State.Cells.ToList();
            cells.Add(cell);
            State = new GameState(cells);

            if (!CanContinue()) return;

            var row = State.Cells.Where(w => w.X == cell.X);
            if (row.Count() == 3 && row.All(a => a.State == cell.State))
            {
                OnGameOver?.Invoke(this, new GameEventArgs(cell.State, true));
                return;
            }

            var column = State.Cells.Where(w => w.Y == cell.Y);
            if (column.Count() == 3 && column.All(a => a.State == cell.State))
            {
                OnGameOver?.Invoke(this, new GameEventArgs(cell.State, true));
                return;
            }

            var diagonal = State.Cells.Where(w => w.X == w.Y);
            if (diagonal.Count() == 3 && diagonal.All(a => a.State == cell.State))
            {
                OnGameOver?.Invoke(this, new GameEventArgs(cell.State, true));
                return;
            }

            var diagonalNext = State.Cells.Where(w => w.X == (2 - w.Y));
            if (diagonalNext.Count() == 3 && diagonalNext.All(a => a.State == cell.State))
            {
                OnGameOver?.Invoke(this, new GameEventArgs(cell.State, true));
                return;
            }

            if (State.Cells.Count() == 9)
            {
                OnFieldFull?.Invoke(this, new FieldFullEventArgs(false));
                return;
            }
        }

        public void Load(int gameId)
        {
            using (var ctx = new GameContext())
            {
                var model = ctx.Games.SingleOrDefault(x => x.Id == gameId) ??
                    throw new ArgumentException("Game doesn't exist");
                var game = JsonConvert.DeserializeObject<Game>(model.Game);

                _id = gameId;
                this.State = game.State;
                this.PlayerOne = game.PlayerOne;
                this.PlayerTwo = game.PlayerTwo;
                this.PlayerActive = game.PlayerActive;
            }
        }

        public event EventHandler<GameEventArgs> OnGameOver;
        public event EventHandler<StepEventArgs> OnStepExecuted;
        public event EventHandler<FieldFullEventArgs> OnFieldFull;
        //public event EventHandler<GameSaveEventArgs> OnGameSave;

    }
}
