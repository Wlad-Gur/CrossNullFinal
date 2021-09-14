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

        // get countries/show
        // post countries/show?id=2
        // post cities/allCitiesShow?country=2

        // get api/countries
        // get countries/id
        // get countries/2/cities


        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [Route(""), HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            //TODO привести в рабочее состояние
            return Ok(new List<User>() { new User()
            { Id = "1", UserName = "QQQ", Email = "qqq@mail.com" } ,
            new User()
            { Id = "1", UserName = "WED", Email = "wed@gmail.com" } });
        }

        [Route("/email/{email}"), HttpGet]// names must be equal
        public IHttpActionResult GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is incorrect.");
            }

            var result = _userService.FindUserByEmail(email);

            if (result.Error == "Invalid email.")
            {
                return BadRequest("Invalid email.");
            }

            if (result.Error == "User not found")
            {
                return NotFound();
            }

            return Ok<User>(result.Value);
        }

    }
}
