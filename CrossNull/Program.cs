using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CrossNull.Models;
using CrossNull.Domain;
using System.Text.RegularExpressions;
using CrossNull.Data;
using System.Data.Entity;
using Newtonsoft.Json;
using DustInTheWind.ConsoleTools.InputControls;
using DustInTheWind.ConsoleTools.TabularData;
using DustInTheWind.ConsoleTools.Menues;
using DustInTheWind.ConsoleTools.Menues.MenuItems;
using CrossNull.Commands;

namespace CrossNull
{
    class Program
    {

        private static bool fieldNoFull = true;
        private static Player playerActive;
        private static ConsoleKeyInfo consoleKey;
        private static Game _gameProg = new Game();
        private static bool _exitGame = true;
        static void Main(string[] args)
        {
            while (_exitGame)
            {
                Console.Clear();
                DisplayMenu();
            }
        }

        private static void SuperPlay()
        {
            Console.Clear();
            fieldNoFull = true;

            //do
            //{
            if (_gameProg.PlayerActive != null)
                {
                    playerActive = _gameProg.PlayerActive;
                }
                else
                {
                    Player playerOne = new Player(PlayerTypes.X, Program.GetPlayerName("Input your name Player1"));
                    Player playerTwo = new Player(PlayerTypes.O,
                        Program.GetPlayerName("Input your name Player2", playerOne.Name));
                    playerActive = playerOne;
                    _gameProg = new Game(playerOne, playerTwo);
                }
                Console.WriteLine(GameFild.ToStringStatic());
                _gameProg.OnFieldFull += Game_OnFieldFull;
                _gameProg.OnStepExecuted += Game_OnStepExecuted;
                _gameProg.OnGameOver += Game_OnGameOver;

                Program.PlayGame(_gameProg);

            //} while (PlayAgain());
        }

        private static void LoadGameList()
        {
            using (var ctx = new GameContext())
            {
                int id = 0;
                Dictionary<int, Game> gamesDict = new Dictionary<int, Game>();
                var models = ctx.Games.ToList();
                foreach (var item in models)
                {
                    var game = JsonConvert.DeserializeObject<Game>(item.Game);
                    Console.WriteLine($"{item.Id} - {game.PlayerOne.Name} - {game.PlayerTwo.Name} " +
                        $"{Environment.NewLine}{game.State}");
                    gamesDict.Add(item.Id, game);
                }
                while (id == 0)
                {
                    Console.WriteLine("Input ID of game");
                    string what = Console.ReadLine();
                    id = Convert.ToInt32(what);
                }
                playerActive = _gameProg.PlayerActive;
                _gameProg = gamesDict[id];
                SuperPlay();
            }
        }

        //private static bool PlayAgain()
        //{
        //    YesNoQuestion yesNoQuestion = new YesNoQuestion("Do you want to play again?");
        //    YesNoAnswer yesNoAnswer = yesNoQuestion.ReadAnswer();
        //    if (yesNoAnswer.Equals(YesNoAnswer.Yes))
        //    {
        //        fieldNoFull = true;
        //    }
        //    return (yesNoAnswer.Equals(YesNoAnswer.Yes));
        //}
        private static string GetPlayerName(string message, string existingName = "")
        {
            string name;
            do
            {
                Console.WriteLine(message);
                name = Console.ReadLine().ToLower();
            } while (name.Trim().Length == 0 || existingName.Equals(name, StringComparison.OrdinalIgnoreCase));
            return name;
        }

        private static void PlayGame(Game game)
        {
            while (fieldNoFull)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine(GameFild.ToStringStatic());
                    Console.WriteLine("_____________");
                    Console.WriteLine(game.State.ToString());
                    Console.WriteLine($"Your step Player {playerActive.Name}");
                    Console.WriteLine("If do you want save game? Press [S] ");
                    consoleKey = Console.ReadKey();
                    Console.WriteLine();
                    if (consoleKey.KeyChar.Equals('S'))
                    {
                        Program.SaveGameToDb(game);
                        return;
                    }
                } while (!IsNumberKeyPressed(consoleKey));

                Cell cell = GameFild.State[consoleKey.Key](playerActive.PlayerType);
                try
                {
                    game.AddCell(cell);
                }
                catch (CellFilledExcention ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

                playerActive = (playerActive.Equals(game.PlayerOne)) ? game.PlayerTwo : game.PlayerOne;
                game.PlayerActive = playerActive;

            }
        }

        private static void SaveGameToDb(Game game)
        {
            Console.WriteLine("Write to DB ");
            // game.PlayerActive = (game.PlayerActive == game.PlayerOne) ? game.PlayerTwo : game.PlayerOne;
            using (var context = new GameContext())
            {
                var existing = new GameStateDb()
                { Game = JsonConvert.SerializeObject(game) };
                context.Games.Add(existing);
                context.SaveChanges();
            }
            fieldNoFull = false;
        }

        private static void SaveScoreToDb(Player player)
        {
            using (var context = new GameContext())
            {
                var existing = context.GameScores.SingleOrDefault(s => s.Name == player.Name);
                //GameScoreDb existing = null;
                if (existing == null)
                {
                    existing = new GameScoreDb() { Name = player.Name };
                    context.GameScores.Add(existing);
                }
                existing.CountWin++;
                context.SaveChanges();
            }
        }
        private static void ShowScore()
        {
            Console.Clear();
            using (var context = new GameContext())
            {
                DataGrid dataGrid = new DataGrid("Table of Records");

                dataGrid.Columns.Add("Name");
                dataGrid.Columns.Add("Score");
                foreach (var item in context.GameScores.ToList())
                {
                    dataGrid.Rows.Add(item.Name, item.CountWin);
                }
                dataGrid.DisplayBorderBetweenRows = true;
                dataGrid.DisplayColumnHeaders = true;
                dataGrid.HeaderColor = ConsoleColor.Red;
                dataGrid.HeaderBackgroundColor = ConsoleColor.White;
                dataGrid.BorderColor = ConsoleColor.Yellow;
                dataGrid.BorderTemplate = BorderTemplate.DoubleLineBorderTemplate;
                dataGrid.Display();
            }
            Console.ReadKey();
        }
        private static void Game_OnGameOver(object sender, GameEventArgs e)
        {
            fieldNoFull = false;
            Console.Clear();
            Game game = sender as Game;
            Console.WriteLine(game.State.ToString());
            Console.WriteLine($"Win player {playerActive.Name}");
            SaveScoreToDb(playerActive);
            _gameProg = new Game();
            Console.ReadKey();
        }

        private static void Game_OnStepExecuted(object sender, StepEventArgs e)
        {
            Game game = sender as Game;
            if (game == null) return;

            if (game.State.Cells.Contains(e.Cell, new CellComparater()))
            {
                throw new CellFilledExcention("Cell is busy");
            }
        }

        private static void Game_OnFieldFull(object sender, FieldFullEventArgs e)
        {
            Console.WriteLine("Field is full");
            Console.WriteLine("Nobody won! Press any key.");
            Console.ReadLine();
            fieldNoFull = e.Win;
            _gameProg = new Game();
        }

        private static bool IsNumberKeyPressed(ConsoleKeyInfo consoleKeyInfo)
        {
            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.NumPad2:
                case ConsoleKey.NumPad3:
                case ConsoleKey.NumPad4:
                case ConsoleKey.NumPad5:
                case ConsoleKey.NumPad6:
                case ConsoleKey.NumPad7:
                case ConsoleKey.NumPad8:
                case ConsoleKey.NumPad9:
                    return true;
                default:
                    {
                        return false;
                    }
            }
        }

        private static void ExitGame()
        {
            _exitGame = false;
        }

        private static void DisplayMenu()
        {
            ScrollMenu scrollMenu = new ScrollMenu()
            {
                Margin = 7,
                HorizontalAlignment = DustInTheWind.ConsoleTools.HorizontalAlignment.Center,
                EraseAfterClose = true
            };

            scrollMenu.AddItems(new IMenuItem[]
            {
        new LabelMenuItem
        {
            Text = "New Game",
            Command = new NewGame(SuperPlay)
        },
        //new YesNoMenuItem
        //{
        //    Text = "Save Game",
        //    Command = new SaveGameCommand(SaveGameToDb,  _gameProg)
        //},
        new LabelMenuItem
        {
            Text = "Load&Play",
            Command = new LoadGameCommand(LoadGameList)
        },
        new LabelMenuItem
        {
            Text = "Show Score",
            Command = new ShowScoreMenu(ShowScore)
        },
        new LabelMenuItem
        {
        Text = "  Exit  ",
        Command = new ExitGame(ExitGame)
        }
    });

            scrollMenu.Display();
        }
    }
}
