using AutoMapper;
using FinanceManager.Requests.v1;
using FinanceManager.Responses.v1;
using FinanceManager.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.Tasks;

namespace FinanceManager.Controllers.v1
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
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerUserRequest)
        {
            var newUser = _mapper.Map<IdentityUser>(registerUserRequest);
            var authResponse = await _identityService.RegisterUserAsync(newUser);

            if(!authResponse.Success)
            {
                var authFailed = _mapper.Map<AuthenticationFailedResponse>(authResponse);
                return BadRequest(authFailed.Errors);
            }

            var authSucceeded = _mapper.Map<AuthenticationSucceededResponse>(authResponse);
            return Ok(authSucceeded);
        }
    }
}
