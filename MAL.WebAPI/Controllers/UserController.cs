using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MAL.DTO;
using MAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MAL.WebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private IUserService _userService;

        public UserController(IUserService userService, IMapper mapper) : base(mapper)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult GetAuthToken([FromBody] UserLoginViewModel login)
        //{
        //    var tokenString = _userService.LoginUser(_mapper.Map<UserDto>(login));

        //    if (!String.IsNullOrEmpty(tokenString))
        //    {
        //        return Ok(new { token = tokenString });
        //    }

        //    return Unauthorized();
        //}

    }
}
