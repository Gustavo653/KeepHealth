using Common.DTO;
using Common.Functions;
using KeepHealth.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeepHealth.API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
            return StatusCode(user.Code, user.Object);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
            var user = await _accountService.Login(userLogin);
            return StatusCode(user.Code, user);
        }
    }
}