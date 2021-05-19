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

        public static Result<GameResult> When
            (this Result<GameResult> result,
            Func<GameResult, bool> cellIsBusy)
        {
            if (result.IsFailure) return result;
            if (cellIsBusy == null) return Result.Failure<GameResult>("Condition is wrong");
            return (cellIsBusy(result.Value)) ? result :
             Result.Failure<GameResult>("Condition is wrong");//Cell is busy
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
        public static Result<ActionResult> ReturnView
            (this Result<GameResult> result, Func<ActionResult> action)
        {
            if (action == null)
            {
                return Result.Failure<ActionResult>("Action is wrong");
            }
            if (result.IsFailure)
            {
                return Result.Failure<ActionResult>("Condition is wrong");
            }
            return action();
        }
        public static Result<GameResult> Or
            (this Result<ActionResult> result)
        {
            if (result.IsSuccess)
            {
               // GameResult gameResult = new GameResult(result.Value.ExecuteResult());
                //GameResult gameResult = new GameResult(result.OnSuccess<ActionResult, GameResult>
                //    (r => r.ExecuteResult() );
                return new Result<GameResult>();
            }
            return Result.Failure<GameResult>("Next situation");
        }
    }

}