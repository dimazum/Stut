using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StopStatAuth_6_0.Entities;
using stutvds.Controllers.Base;

namespace stutvds.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    // =========================
    // REGISTER
    // =========================

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var user = new ApplicationUser()
        {
            UserName = model.Username,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                message = string.Join(", ",
                    result.Errors.Select(e => e.Description))
            });
        }

        await _userManager.AddToRoleAsync(user, "User");

        return Ok(new { message = "User registered successfully" });
    }

    // =========================
    // LOGIN
    // =========================

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user == null ||
            !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(user);
        
        await _signInManager.SignInAsync(user, true);

        return Ok(new UserInfoDto
        {
            user_name = user.UserName,
            user_role = string.Join(",", roles),
            logged_in = true
        });
    }

    // =========================
    // LOGOUT
    // =========================

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return Ok(new { logged_out = true });
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return Unauthorized(new 
            { 
                code = ErrorCodes.MeUnauthorized.Code, 
                message = ErrorCodes.MeUnauthorized.Message 
            });
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return Unauthorized(new 
            { 
                code = ErrorCodes.MeUnauthorized.Code, 
                message = ErrorCodes.MeUnauthorized.Message 
            });
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new UserInfoDto
        {
            user_name = user.UserName,
            user_role = string.Join(",", roles),
            logged_in = true
        });
    }

    // =========================
    // DTOs
    // =========================

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

public class RegisterDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserInfoDto
{
    public string user_name { get; set; }
    public string user_role { get; set; }
    public bool logged_in { get; set; }
}