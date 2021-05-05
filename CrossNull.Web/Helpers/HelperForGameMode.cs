using CrossNull.Logic.Services;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrossNull.Web.Helpers
{
    public static class HelperForGameMode
    {
        public static bool _tf = false;
        public static string _message;
        public static int _id;
        public static string _view;

        public static (bool, Result<GameResult>) When
            (this Result<GameResult> result, GameSituation gameSituation)
        {
            if (result.Value.Situation == gameSituation)
            {
                return (true, result);
            }
            return (false, result);
        }
        public static (int, string, Result<GameResult>) AddData
            (this (bool, Result<GameResult>) result)
        {
            if (result.Item1)
            {
                return (result.Item2.Value.Model.Id, "Cell is busy", result.Item2);// new {Id = gameafter.value.id, mesage = ""}
            }
            return (result.Item2.Value.Model.Id, "", result.Item2);
        }
        public static ((int, string, Result<GameResult>), string view) ReturnView
            (this (int, string, Result<GameResult>) result)
        {
            return (result, "Error");
        }
    }

}