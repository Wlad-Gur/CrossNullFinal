using CrossNull.Logic.Services;
using CrossNull.Models;
using CrossNull.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View("Init", new InitViewModel() { NameOne = "1111111", NameTwo = "222222"});
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

            string playerActiveName = result.Value.PlayerActive.Name.ToString();
            ViewBag.Message = playerActiveName;
            return View(result);
        }

        [HttpGet]
        public ActionResult Step(int gameId, int x, int y)
        {
            return View("NewGame");
        }
    }
}