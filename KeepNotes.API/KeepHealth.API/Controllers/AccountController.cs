using Common.DTO;
using Common.Functions;
using KeepHealth.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeepHealth.API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService,
                                 ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser")]
        public async Task<ResponseDTO> GetUser()
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await _accountService.GetUserByUserNameAsync(User.GetUserName());
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDTO userDto)
        {
            var user = await _accountService.CreateAccountAsync(userDto);
            return StatusCode(user.Code, user);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
            var user = await _accountService.Login(userLogin);
            return StatusCode(user.Code, user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserDTO userUpdateDto)
        {
            var user = await _accountService.UpdateAccount(userUpdateDto);
            return StatusCode(user.Code, user);
        }
    }
}