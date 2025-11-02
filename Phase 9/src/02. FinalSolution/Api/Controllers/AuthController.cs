namespace Api.Controllers;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Common.Models;

[ApiController]
public class AuthController : ApiController
{
    private readonly IConfiguration config;

    public AuthController(IConfiguration config) => this.config = config;

    [AllowAnonymous]
    [HttpPost]
    [Route("Authorise")]
    public IActionResult Authorise([FromBody] AuthModel auth)
    {
        IActionResult response = this.Unauthorized();
        var authenticate = this.Authenticate(auth);

        if (authenticate.Succeeded)
        {
            var tokenString = this.GenerateJSONWebToken();
            response = this.Ok(new { token = tokenString });
        }

        return response;
    }

    private string GenerateJSONWebToken()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(this.config["Jwt:Issuer"], this.config["Jwt:Issuer"], null, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private Result Authenticate(AuthModel auth) =>
        ////Demo Purpose, I have Passed HardCoded User Information
        auth.ApiKey == "940312b1cd649122b2f29fc2a68e47dbfaf12aca" ? Result.Success() : Result.Failure("Validation Error");
}
