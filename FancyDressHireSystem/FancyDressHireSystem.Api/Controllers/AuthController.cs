using Microsoft.AspNetCore.Mvc;
using FancyDressHireSystem.Api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FancyDressHireSystem.Api.Controllers;

[ApiController]
[Route("api/auth")]
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
    
    // Logs the user out by deleting their login cookie
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
    
    // Returns the user who is currently logged in
    [Authorize]
    [HttpGet("me")]
    public IActionResult CurrentUser()
    {
        string? username = User.Identity?.Name;
        string? role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new { username, role });
    }
}
