using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using stutvds.Emails;
using stutvds.Emails.Dto;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IEmailSender _emailSender;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration config,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
        _emailSender = emailSender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        bool enableEmailAuthentication = bool.TryParse(_config["Email:EnableEmailAuthentication"], out _);
        
        var user = new IdentityUser
        {
            UserName = model.Username,
            Email = model.Email,
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (!result.Succeeded)
        {
            return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });
        }
        
        await _userManager.AddToRoleAsync(user, "User");

        if (enableEmailAuthentication)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            await _emailSender.SendConfirmationEmail(user.Email, user.Id, encodedToken);
        
            return Ok(new { message = "Registration successful. Please confirm your email." });
        }
        
        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);

        if (user == null)
        {
            return BadRequest(new { message = "User not found" });
        }
        
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
        
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (!result.Succeeded)
        {
            return BadRequest(new {message = "Invalid or expired token"});
        }
        
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        bool enableEmailAuthentication = bool.TryParse(_config["Email:EnableEmailAuthentication"], out _);
        
        var user = await _userManager.FindByNameAsync(model.Username);
        
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        if (enableEmailAuthentication && !user.EmailConfirmed)
        {
            return Unauthorized(new { message = "Email not confirmed" });
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
    
    [HttpPost("send-reset-password")]
    public async Task<IActionResult> SendResetPassword([FromBody] SendResetPasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            return Ok();
        }
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        await _emailSender.SendResetPasswordEmail(user.Email, user.Id, encodedToken);
        
        return Ok();
    }
    
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);

        if (user == null)
        {
            return BadRequest(new { message = "User not found" });
        }
        
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });
        }
        
        return Ok();
    }
    
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
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new UserInfoDto
        {
            user_name = user.UserName,
            user_role = string.Join(",", roles),
            logged_in = true
        });
    }
    
    private string GenerateJwtToken(List<Claim> claims)
    {
        var key = Encoding.UTF8.GetBytes(
            _config["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMonths(1),
            Issuer = _config["Jwt:Issuer"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

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
