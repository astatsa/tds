using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TDSServer.Models;
using TDSServer.Services;

namespace TDSServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult Authenticate([FromBody]AuthModel authModel)
        {
            var token = userService.Authenticate(authModel.Username, authModel.Password);
            if (token == null)
                return BadRequest(new { message = "Username or password is incorrect!" });
            return Ok(token);
        }
    }
}