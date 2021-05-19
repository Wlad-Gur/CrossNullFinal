using CrossNull.Logic.Models;
using CrossNull.Logic.Services;
using CrossNull.Models;
using CrossNull.Web.Helpers;
using CrossNull.Web.Model;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CrossNull.Logic.Services.GameService;

namespace CrossNull.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameSevice;
        public GameController(IGameService gameSevice)
        {
            this._gameSevice = gameSevice;
        }
        public ActionResult LoadGame(int id)
        {
            var result = _gameSevice.Load(id);
            if (result.IsFailure)
            {
                return Content(result.Error);
            }
            return View("NewGame", result);
        }
        // GET: Game
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult NewGame()
        {
            ViewBag.Message = "My best form works";
            return View("Init", new InitViewModel() { NameOne = "1111111", NameTwo = "222222" });
        }

        // POST
        [HttpPost]
        public ActionResult NewGame(InitViewModel initViewModel)
        {
            if (initViewModel == null)
            {
                return RedirectToAction("NewGame");
            }
            Player playerOne = new Player(PlayerTypes.X, initViewModel.NameOne);
            Player playerTwo = new Player(PlayerTypes.O, initViewModel.NameTwo);

            var result = _gameSevice.StartNew(playerOne, playerTwo);

            return View(result);
        }

        [HttpGet]
        public ActionResult Step(int gameId = 0, int x = -1, int y = -1)
        {
            Result<GameModel> currentGame = _gameSevice.Load(gameId);
            if (currentGame.IsFailure)
            {
                return View("Init", new InitViewModel());
            }
            if (x < 0 || x > 2 || y < 0 || y > 2)
            {
                return currentGame.IsSuccess ? View("NewGame", currentGame) : View("Init",
                    new InitViewModel()
                    {
                        NameOne = currentGame.Value.PlayerOne.Name,
                        NameTwo = currentGame.Value.PlayerTwo.Name
                    });
            }

            if (currentGame.IsSuccess)
            {
                var stepResult = _gameSevice.NextTurn(currentGame.Value, x, y);
                if (stepResult.IsFailure)
                {
                    //возвращаем тоже самое игровое поле и сообщение "Incorrect data"
                    ViewBag.Message = "Incorrect data";
                    return View("NewGame", currentGame);
                }

                List<Result<ActionResult>> resultsList = new List<Result<ActionResult>>();

                resultsList.Add(stepResult.When(r => (r.Situation == GameSituation.CellIsExist)).
                AddData(a => ViewBag.Message = "Cell is busy").
                AddData(a => ViewBag.Id = currentGame.Value.Id).ReturnView(() => View("Error")));

                resultsList.Add(stepResult.When(r => (r.Situation == GameSituation.EndOfCells)).
                    ReturnView(() => View("NobodyWin")));

                resultsList.Add(stepResult.When(r => (r.Situation == GameSituation.PlayerWins)).
                      AddData(a => ViewBag.Message = $"Player {stepResult.Value.Model.PlayerActive.Name} Wins").
                      ReturnView(() => View("PlayerWin")));

                Result<GameModel> convertResult = null;

                resultsList.Add(stepResult.When(r => (r.Situation == GameSituation.GameContinue)).
                        AddData(a => convertResult = stepResult.Value.Model).
                        ReturnView(() => View("NewGame", convertResult)));

                var resultOr = stepResult.When(r => (r.Situation == GameSituation.GameContinue)).
                    AddData(a => ViewBag.Message = "Cell is busy").
                AddData(a => ViewBag.Id = currentGame.Value.Id).ReturnView(() => View("NewGame", convertResult));

                return (resultsList.SingleOrDefault(w => w.IsSuccess).IsFailure ? View("Init",
                    new InitViewModel()
                    {
                        NameOne = currentGame.Value.PlayerOne.Name,
                        NameTwo = currentGame.Value.PlayerTwo.Name
                    }) : resultsList.SingleOrDefault(w => w.IsSuccess).Value);

            }

            return View("Init",
                    new InitViewModel()
                    {
                        NameOne = currentGame.Value.PlayerOne.Name,
                        NameTwo = currentGame.Value.PlayerTwo.Name
                    });
        }
    }
}