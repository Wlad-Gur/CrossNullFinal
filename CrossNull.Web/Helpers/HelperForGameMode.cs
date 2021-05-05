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

        public static Result<GameResult> When(this Result<GameResult> result/*, GameSituation gameSituation*/)
        {
            if (result.Value.Situation == GameSituation.CellIsExist)
            {
                _tf = true;
                return (result);
            }
            return (result);
        }
        public  static Result<GameResult> AddData(this Result<GameResult> result)
        {
            if (_tf)
            {
                _id = result.Value.Model.Id;
                _message = "Cell is busy";
                return (result);// new {Id = gameafter.value.id, mesage = ""}
            }
            return (result);
        }
        public static ( bool ts, string message, int id, string view) ReturnView(this Result<GameResult> result)
        {
            if (_tf)
            {
                _view = "Error";
            }
            return (_tf, _message, _id, _view);
        }
    }

}