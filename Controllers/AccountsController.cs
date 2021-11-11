using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Suma.Authen.Dtos;
using Suma.Authen.Dtos.Accounts;
using Suma.Authen.Exceptions;
using Suma.Authen.Services;

namespace Suma.Authen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest reqModel, CancellationToken cancellationToken)
        {
            await _accountService.SignUpAsync(reqModel, cancellationToken);
            return Ok();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Signin([FromBody] SignInRequest reqModel)
        {
            try
            {
                return Ok(await _accountService.SignInAsync(reqModel));
            }
            catch (SignInException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest reqModel, CancellationToken cancellationToken)
        {
            var res = await _accountService.RefreshToken(reqModel, cancellationToken);
            if (res is null)
            {
                return Unauthorized("invalid refresh token.");
            }

            return Ok(res);
        }

        [Authorize]
        [HttpGet("test")]
        public string Test()
        {
            return "Test";
        }
    }
}