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
        // get countries/show
        // post countries/show?id=2
        // post cities/allCitiesShow?country=2

        // get countries
        // get countries/2
        // get countries/2/cities

        [Route(""), HttpGet]
        public IHttpActionResult GetAllUsers ()
        {
            return Ok(new List<User>() { new User() { Id = 1, Name = "QQQ" }  } );
        }
    }
}
