using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using TasksManager.DataAccess.Interfaces.Users;
using TasksManager.ViewModel.Users;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private JwtOptions Options { get; }
        public UsersController(IOptions<JwtOptions> options)
        {
            Options = options.Value;
        }
    
        [HttpPost("auth")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Authentication([FromBody]AuthRequest request, [FromServices]IGetIdentityCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ClaimsIdentity identity = await command.ExecuteAsync(request);

                var jwt = new JwtSecurityToken(
                    issuer: Options.Issuer,
                    audience: Options.Audience,
                    notBefore: DateTime.Now,
                    claims: identity.Claims,
                    expires: DateTime.Now.Add(TimeSpan.FromMinutes(30)),
                    signingCredentials: new SigningCredentials(Options.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                return Ok(encodedJwt);
            }
            catch (AuthenticationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Registration([FromBody]RegistrationRequest request, [FromServices]IRegistrationCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await command.ExecuteAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
