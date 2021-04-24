using CrossNull.Logic.Models;
using CrossNull.Logic.Services;
using CrossNull.Models;
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
            return Json(result.Value, JsonRequestBehavior.AllowGet);
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

            string[,] vs = new string[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (result.Value.State.Cells.SingleOrDefault(f => f.X == i && f.Y == j) == null)
                    {
                        vs[i, j] = "?";
                    }
                    else
                    {
                        vs[i, j] = result.Value.State.Cells.Single(f => f.X == i && f.Y == j).State.ToString();
                    }
                }
            }
            vs[1, 1] = "X";
            ViewBag.VS = vs;

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

                switch (stepResult.Value.Situation)
                {

                    case GameSituation.CellIsExist:
                        //возвращаем тоже самое игровое поле и сообщение "Cell is busy"
                        ViewBag.Message = "Cell is busy";
                        return View("NewGame", gameAfter);

                    case GameSituation.EndOfCells:
                        //просто сообщение "Game Over. Nobody win."
                        return View("NobodyWin");

                    case GameSituation.GameContinue:
                        //возвращаем обновленное игровое поле
                        Result<GameModel> convertResult = stepResult.Value.Model;
                        return View("NewGame", convertResult);

                    case GameSituation.PlayerWins:
                        //обновленное поле и сообщение "Player XXX Wins"
                        Result<GameModel> convertResultn = stepResult.Value.Model;
                        ViewBag.Message = $"Player {stepResult.Value.Model.PlayerActive.Name} Wins";
                        return View("NewGame", convertResultn);
                    default:

                        break;
                }

            }

            return View("Init");
        }
    }
}