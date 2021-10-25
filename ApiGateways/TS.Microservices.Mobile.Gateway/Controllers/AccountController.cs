using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TS.Microservices.Mobile.Gateway.Controllers
{
    [ApiController]
    [Route("account/[action]")]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Login()
        {
            return await Task.FromResult("请先登录");
        }

        [HttpGet]
        public async Task<IActionResult> JwtLogin([FromServices] SymmetricSecurityKey securityKey,[FromQuery] string userName)
        {

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Name", userName));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            var t = new JwtSecurityTokenHandler().WriteToken(token);
            return Content(t);
        }

        [HttpGet]
        [Authorize]
        public IActionResult TestJwt()
        {
            return Content(User.FindFirst("Name").Value);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult TestJwt1()
        {
            return Content(User.FindFirst("Name").Value);
        }
    }
}
