using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Account;
using Service.Services.Interfaces;

namespace App.Controllers.UI
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _accountService.SignUpAsync(request);

            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _accountService.GetUsersAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByUsername([FromQuery] string username)
        {
            return Ok(await _accountService.GetUserByUsernameAsync(username));
        }
    }
}
