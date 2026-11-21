using Microsoft.AspNetCore.Mvc;
using FancyDressHireSystem.Api.Models;
namespace FancyDressHireSystem.Api.Controllers;

[ApiController]
[Route("api/auth") ]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { error = "Please provide a username and password" });
        }

        return Ok();
    }
}
