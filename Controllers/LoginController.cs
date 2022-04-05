using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private IConfiguration _configuration;

    public LoginController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login([FromBody] UserLogin userLogin)
    {
        var user = Authenticate(userLogin);
        if (user != null)
        {
            var token = Generate(user);
            return Ok(token);
        }

        return NotFound("User not found");
    }

    private string Generate(UserModel user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Email, user.EmailAddres),
            new Claim(ClaimTypes.GivenName, user.GivenName),
            new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private UserModel Authenticate(UserLogin userLogin)
    {
        var currentUser = UserConstants.Users.FirstOrDefault(o => o.Username.ToLower() ==
        userLogin.Username.ToLower() && o.Password == userLogin.Password);

        if (currentUser != null)
        {
            return currentUser;
        }

        return null;
    }
}
