using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Suma.Authen.Dtos;
using Suma.Authen.Dtos.Accounts;
using Suma.Authen.Services;

namespace Suma.Authen.Controllers
{
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Signin([FromBody] SignInRequest reqModel)
        {
            var res = await _accountService.SignInAsync(reqModel);
            return Ok(res);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest reqModel)
        {
            await _accountService.SignUpAsync(reqModel);
            return Ok();
        }
    }
}