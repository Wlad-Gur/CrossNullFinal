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

        public static Result<GameResult> When
            (this Result<GameResult> result,
            Func<GameResult, bool> cellIsBusy)
        {
            if (result.IsFailure) return result;
            if (cellIsBusy == null) return Result.Failure<GameResult>("Condition is wrong");
            return (cellIsBusy(result.Value)) ? result :
             Result.Failure<GameResult>("Condition is wrong");
        }
        public static Result<GameResult> AddData
            (this Result<GameResult> result, Action<GameResult> action)
        {
            if (result.IsFailure) return result;
            if (action == null) return Result.Failure<GameResult>("Action is wrong");
            try
            {
                action(result.Value);
            }
            catch (Exception)
            {
                return Result.Failure<GameResult>("Exeption");
            }
            return result;
        }
        //TODO
        public static ActionResult ReturnView
            (this Result<GameResult> result)
        {
            return (result.Item1, result.Item2, result.Item3, "Error");
        }
    }

}