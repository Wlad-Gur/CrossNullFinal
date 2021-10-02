using CrossNull.Logic.Models;
using CrossNull.Logic.Services;
using CrossNull.Web.Helpers;
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
            return result.ToHttpResult(this);
        }


        [Route(""), HttpPost]
        public IHttpActionResult AddUser([FromBody] UpdateUserModel updateUserModel)
        {
            var result = _userService.AddUser(updateUserModel);
            return result.ToHttpResult(this);
        }

        [Route("{id}"), HttpPut]
        public IHttpActionResult ChangeWholeUser(string id, [FromBody] UpdateUserModel updateUserModel)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Incorrect ID");
            }
            var result = _userService.ChangeWholeUser(id, updateUserModel);
            return Ok("ChanageWholeUser");
        }

        [Route("{id}"), HttpDelete]
        public IHttpActionResult DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Incorrect ID");
            }
            var result = _userService.DeleteUser(id);
            return Ok("DeleteUser");
        }

        [Route("{id}"), HttpPatch]
        public IHttpActionResult PartialChange(string id, string nameProp, string valueProp)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Incorrect ID");
            }
            var result = _userService.PartialChange(id, nameProp, valueProp);
            return Ok("User change partial");
        }
    }
}
