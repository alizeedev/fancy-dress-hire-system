using Microsoft.AspNetCore.Mvc;
using FancyDressHireSystem.Api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FancyDressHireSystem.Core.Data;
using FancyDressHireSystem.Core.Models;
using FancyDressHireSystem.Core.Services;

namespace FancyDressHireSystem.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    
    private readonly UserRepository _users;

    public AuthController(UserRepository users)
    {
        _users = users;
    }
    
    // Checks the username and password against the database
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { error = "Please provide a username and password" });
        }

        User? user = _users.GetByUsername(request.UserName);

        if (user == null)
        {
            return Unauthorized(new { error = "Invalid username or password" });
        }

        if (!PasswordHasher.Verify(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Unauthorized(new { error = "Invalid username or password" });
        }
        
        Claim[] claims =
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.UserRole)
        };
        
        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return Ok(new { userName = user.UserName, userRole = user.UserRole });
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

        return Ok(new { userName = username, userRole = role });
    }
}
