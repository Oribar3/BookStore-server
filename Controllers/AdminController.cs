using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using book_store_server_side.Models;
using book_store_server_side.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace book_store_server_side.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IAccountRepository _accountRepository;


        public AdminController(IAdminRepository adminRepository, IAccountRepository accountRepository)
        {
            _adminRepository = adminRepository;
            _accountRepository = accountRepository;

        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignupAsAdmin([FromBody] SignupModel signupModel)
        {
            var res = await _adminRepository.SignUp(signupModel);
            if (res.Succeeded)
            {
                return Ok(res.Succeeded);
            }
            return Unauthorized();
        }


        [HttpPost("set-discount/{discount}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> SetDiscount([FromRoute] int discount)
        {
            var res = await _adminRepository.SetDiscount(discount);
            if (res == discount)
            {
                return Ok();
            }
            return Unauthorized();
        }
        [HttpGet("get-discount")]
        public async Task<int> GetDiscount()
        {
            var res = await _adminRepository.GetDiscount();
            return res;
        }

        [HttpDelete("delete-user/{userName}")]
        public async Task <bool> deleteUser([FromRoute] string userName)
        {
            var res = await _adminRepository.DeleteUser(userName);
            return res;
        }
   
    }
}
