using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Contracts.v1.Requests;
using TweetBook4.Contracts.v1.Responses;
using TweetBook4.Contracts.vi;
using TweetBook4.Service;

namespace TweetBook4.Controllers.v1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<ActionResult> Login([FromBody] UserLoginRequest request)
        {
            // validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFaildResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var authResponse = await _identityService.LoginAsync(request.Email, request.Passwod);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFaildResponse
                {
                    Errors = authResponse.Errors
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken=authResponse.RefreshToken
            });
        }


        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<ActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            // validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFaildResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Passwod);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFaildResponse
                {
                    Errors=authResponse.Errors
                });
            }
            return Ok(new AuthSuccessResponse 
            { 
                Token=authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }


        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<ActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            // validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFaildResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var authResponse = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFaildResponse
                {
                    Errors = authResponse.Errors
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
    }
}
