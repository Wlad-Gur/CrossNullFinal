using CrossNull.Logic.Services;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace CrossNull.Web.Helpers
{
    public static class ApiExtensions
    {
        public static IHttpActionResult ToHttpResult(this Result result)
        {
            if (result.IsSuccess)
            {
                return new OkResult(new System.Net.Http.HttpRequestMessage());
            }
            return new BadRequestResult(new System.Net.Http.HttpRequestMessage());
        }

        public static IHttpActionResult ToHttpResult<T>(this Result<T> result,
        ApiController controller)
        {
            if (result.IsSuccess)
            {
                return (IHttpActionResult)new OkNegotiatedContentResult<T>(result.Value, controller);
            }

            return new BadRequestResult(new System.Net.Http.HttpRequestMessage());

        }
        public static IHttpActionResult ToHttpResult<T, E>(this Result<T, E> result,
            ApiController controller) where E : ApiError
        {
            if (result.IsSuccess)
            {
                return (IHttpActionResult)new OkNegotiatedContentResult<T>(result.Value, controller);
            }

            switch (result.Error.ErrorType)
            {
                case ErrorTypes.Invalid:
                    return new BadRequestResult(new System.Net.Http.HttpRequestMessage());
                case ErrorTypes.NotFound:
                    return new NotFoundResult(controller);
                case ErrorTypes.InternalException:
                    return new InternalServerErrorResult(controller);
                default:
                    throw new Exception("Unknown error type");
            }
        }
    }

}
