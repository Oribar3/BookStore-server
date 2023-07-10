using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using book_store_server_side.Models;
using book_store_server_side.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace book_store_server_side.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;


        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet("is-admin")]
        [Authorize]

        public async Task<bool> isAdmin()
        {
            var userName = User.Identity.Name;
            if (userName == null) return false;
            var res = await _accountRepository.isAdmin(userName);
            return res;

        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> Signup([FromBody] SignupModel signupModel)
        {
            var res = await _accountRepository.SignUp(signupModel);
            if (res.Succeeded)
            {
                return Ok(res.Succeeded);
            }
            return Unauthorized();
        }
     
        [HttpPost("log-in")]
        public async Task<IActionResult> Login([FromBody] LoginModel signinModel)
        {
            var res = await _accountRepository.Login(signinModel);
            if (string.IsNullOrWhiteSpace(res))
            {
                return Unauthorized();
            }
            return Ok(res);
        }


        [HttpPatch("myAccount")]
        [Authorize]

        public async Task<IActionResult> updateUserPropety([FromBody] UpdateUserPropertyModel updateUserPropertyModel)
        {
            var userName=User.Identity.Name;
            var res = await _accountRepository.changeUserProperty(updateUserPropertyModel.property, updateUserPropertyModel.newVal,userName);

            if (res==null)
            {
                return BadRequest();
            }

            return Ok(res);
        }

        [HttpPatch("myAccount/password")]
        [Authorize]
        public async Task<IActionResult> updateUserPassword([FromBody] UpdateUserPasswordModel updateUserPasswordModel)
        {
            var userName = User.Identity.Name;
            var res = await _accountRepository.changeUserPassword(updateUserPasswordModel.oldPassword, updateUserPasswordModel.newPassword,userName);

            if (!res)
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}

