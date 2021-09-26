using CrossNull.Logic.Models;
using CrossNull.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CrossNull.Web.Controllers.API
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [Route(""), HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            if (result.Value == null)
            {
                return Content(HttpStatusCode.NoContent, result);
            }
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            switch (result.Error.ErrorType)
            {
                case ErrorTypes.InternalException:
                    return InternalServerError(new Exception(result.Error.Message));
                default:
                    throw new Exception("Unknown error type");
            }
        }

        [Route("email"), HttpGet]// names must be equal
        public IHttpActionResult GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is incorrect.");
            }

            var result = _userService.FindUserByEmail(email);
            if (result.IsSuccess)
                return Ok<User>(result.Value);

            switch (result.Error.ErrorType)
            {
                case ErrorTypes.Invalid:
                    return BadRequest(result.Error.Message);
                case ErrorTypes.NotFound:
                    return NotFound();
                case ErrorTypes.InternalException:
                    return InternalServerError(new Exception(result.Error.Message));
                default:
                    throw new Exception("Unknown error type");
            }
        }
        [Route("registration"), HttpPost]
        public IHttpActionResult AddUser([FromBody] RegisterModel registerModel)
        {
            var result = _userService.AddUser(registerModel);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok("User added successfully");
        }
        [Route("change"), HttpPut]
        public IHttpActionResult ChanageUser(string ID, [FromBody] RegisterModel registerModel)
        {
            return Ok("ChanageUser");
        }

        [Route("delete"), HttpDelete]
        public IHttpActionResult DeleteUser(string ID)
        {
            return Ok("DeleteUser");
        }

    }
}
