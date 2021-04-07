using CrossNull.Logic.Services;
using CrossNull.Models;
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
        // POST
        [HttpPost]
        public ActionResult NewGame(string nameOne, string nameTwo)
        {
            Player playerOne = new Player(PlayerTypes.X,nameOne);
            Player playerTwo = new Player(PlayerTypes.O, nameTwo);
            if (playerOne == null || playerTwo == null)
            {
                return RedirectToAction("Index");
            }
            var result = _gameSevice.StartNew(playerOne, playerTwo);
            string playerActiveName = result.Value.PlayerActive.Name;
            ViewBag.Message = playerActiveName;
            return View(result);
        }
    }
}