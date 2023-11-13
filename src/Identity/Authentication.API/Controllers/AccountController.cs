using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;

        public AccountController(JwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
        }

        [HttpPost]

        public ActionResult<AuthenticationResponse?> Authenticate([FromBody] AuthenticationRequest authentication)
        {
            var authenticationResponse = _jwtTokenHandler.GenerateJwtToken(authentication);
            if(authenticationResponse == null) return Unauthorized();

            return authenticationResponse;

        } 


    }
}
