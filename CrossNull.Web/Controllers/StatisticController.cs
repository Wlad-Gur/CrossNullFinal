using CrossNull.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrossNull.Web.Controllers
{
    public class StatisticController : Controller
    {
        private IStatisticService _statService;
        public StatisticController(IStatisticService statisticService)
        {
            _statService = statisticService;
        }
        // GET: Statistic

        public ActionResult Index()
        {
            ViewBag.Message = "Table of result";

            return View(_statService.LoadGameScoreStatistic());
        }
    }
}