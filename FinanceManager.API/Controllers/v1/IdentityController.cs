using AutoMapper;
using FinanceManager.API.Extensions;
using FinanceManager.API.Requests.v1;
using FinanceManager.API.Responses.v1;
using FinanceManager.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinanceManager.API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/identity/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public IdentityController(IIdentityService identityService, IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserRequest registerUserRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var newUser = _mapper.Map<IdentityUser>(registerUserRequest);
            var authResponse = await _identityService.RegisterUserAsync(newUser);

            if (!authResponse.Success)
            {
                var authFailed = _mapper.Map<AuthenticationFailedResponse>(authResponse);
                return BadRequest(authFailed.Errors);
            }

            var authSucceeded = _mapper.Map<AuthenticationSucceededResponse>(authResponse);
            return Ok(authSucceeded);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserRequest loginUserRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var authResponse = await _identityService.LoginUserAsync(loginUserRequest.UserName, loginUserRequest.Password);

            if (!authResponse.Success)
            {
                var authFailed = _mapper.Map<AuthenticationFailedResponse>(authResponse);
                return BadRequest(authFailed.Errors);
            }

            var authSucceeded = _mapper.Map<AuthenticationSucceededResponse>(authResponse);
            return Ok(authSucceeded);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var authResponse = await _identityService.RefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken);

            if (!authResponse.Success)
            {
                var authFailed = _mapper.Map<AuthenticationFailedResponse>(authResponse);
                return BadRequest(authFailed.Errors);
            }

            var authSucceeded = _mapper.Map<AuthenticationSucceededResponse>(authResponse);
            return Ok(authSucceeded);
        }
    }
}
