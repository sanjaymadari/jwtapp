using System.Security.Claims;
using JwtApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpGet("Admins")]
    [Authorize(Roles = "Administrator")]
    public IActionResult AdminsEndpoint()
    {
        var currentUser = GetCurrentUser();
        return Ok($"Hi, {currentUser.GivenName}, you are an {currentUser.Role}!");
    }

    [HttpGet("Sellers")]
    [Authorize(Roles = "Seller")]
    public IActionResult SellersEndpoint()
    {
        var currentUser = GetCurrentUser();
        return Ok($"Hi, {currentUser.GivenName}, you are an {currentUser.Role}!");
    }

    [HttpGet("AdminsSellers")]
    [Authorize(Roles = "Administrator,Seller")]
    public IActionResult AdminsAndSellersEndpoint()
    {
        var currentUser = GetCurrentUser();
        return Ok($"Hi, {currentUser.GivenName}, you are an {currentUser.Role}!");
    }

    [HttpGet("Public")]
    public IActionResult Public()
    {
        return Ok("Hi, this is a public method!");
    }


    private UserModel GetCurrentUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        if (identity != null)
        {
            var userClaims = identity.Claims;

            return new UserModel
            {
                Username = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                EmailAddres = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                GivenName = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                Surname = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                Role = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
            };
        }
        return null;
    }
}