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
        public ActionResult Step(int gameId, int x, int y)
        {
            if (gameId < 1)
            {
                return View("Init");
            }
            Result<GameModel> gameAfter = _gameSevice.Load(gameId);
            if (x < 0 || x > 2 || y < 0 || y > 2)
            {
                return gameAfter.IsSuccess ? View("NewGame", gameAfter) : View("Init");
            }

            if (gameAfter.IsSuccess)
            {
                var stepResult = _gameSevice.Step(gameAfter.Value, x, y);
                if (stepResult.IsFailure)
                {
                    //возвращаем тоже самое игровое поле и сообщение "Incorrect data"
                    ViewBag.Message = "Incorrect data";
                    return View("NewGame", gameAfter);
                }

                var res = stepResult.When().AddData().ReturnView();

                switch (stepResult.Value.Situation)
                {

                    case GameSituation.CellIsExist:
                        // TODO stepResult.When(GameSituation.CellIsExist)
                        // .AddData(new {Id = gameafter.value.id, mesage = ""})
                        // .ReturnView("Error").Or().When(...).;
                        //возвращаем тоже самое игровое поле и сообщение "Cell is busy"
                        ViewBag.Message = "Cell is busy";
                        ViewBag.Id = gameAfter.Value.Id;
                        return View("Error");

                    case GameSituation.EndOfCells:
                        //просто сообщение "Game Over. Nobody win."
                        return View("NobodyWin");

                    case GameSituation.GameContinue:
                        //возвращаем обновленное игровое поле
                        Result<GameModel> convertResult = stepResult.Value.Model;
                        return View("NewGame", convertResult);

                    case GameSituation.PlayerWins:
                        //обновленное поле и сообщение "Player XXX Wins"
                        ViewBag.Message = $"Player {stepResult.Value.Model.PlayerActive.Name} Wins";
                        return View("PlayerWin");
                    default:

                        break;
                }

            }

            return View("Init");
        }
    }
}