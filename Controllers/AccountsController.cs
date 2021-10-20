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
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest reqModel)
        {
            await _accountService.SignUpAsync(reqModel);
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
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest reqModel)
        {
            var res = await _accountService.RefreshToken(reqModel);
            if (res is null)
            {
                return Unauthorized("invalid token.");
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