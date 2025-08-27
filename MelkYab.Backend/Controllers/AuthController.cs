using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MelkYab.Backend.Data.Dtos;
using MelkYab.Backend.Data.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MelkYab.Backend.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly LinkGenerator _linkGenerator;
        private readonly IConfiguration _config;  // ← اینجا تزریق میشه

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config,LinkGenerator linkGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _linkGenerator = linkGenerator;
        }

        private string ApiVersion => HttpContext.GetRequestedApiVersion()?.ToString() ?? "1";

        // POST: api/v1/auth
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Fullname = model.Fullname,
                Phone = model.Phone,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new
                {
                    errors = result.Errors,
                    links = new[]
                    {
                        new { rel = "register", method = "POST", href = _linkGenerator.GetPathByAction("Register", "Auth", new { version = ApiVersion }) }
                    }
                });

            await _signInManager.SignInAsync(user, isPersistent: false);

            var meUrl = _linkGenerator.GetPathByAction("GetCurrentUser", "Auth", new { version = ApiVersion });

            return Created(meUrl!, new
            {
                message = "User registered successfully",
                user.Email,
                links = new[]
                {
                    new { rel = "self", method = "GET", href = meUrl },
                    new { rel = "logout", method = "DELETE", href = _linkGenerator.GetPathByAction("Logout", "Auth", new { version = ApiVersion }) },
                    new { rel = "login", method = "POST", href = _linkGenerator.GetPathByAction("Login", "Auth", new { version = ApiVersion }) }
                }
            });
        }

        // POST: api/v1/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized(new { message = "Invalid login." });

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new { message = "Invalid login." });

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                message = "Login successful",
                token,
                user = new { user.Id, user.Email, user.Fullname }
            });
        }

        // GET: api/v1/auth/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(new { user.Id, user.Email, user.Fullname });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(_userManager.Users.ToList());
        }

        // DELETE: api/v1/auth/logout
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new
            {
                message = "Logout successful",
                links = new[]
                {
                    new { rel = "login", method = "POST", href = _linkGenerator.GetPathByAction("Login", "Auth", new { version = ApiVersion }) },
                    new { rel = "register", method = "POST", href = _linkGenerator.GetPathByAction("Register", "Auth", new { version = ApiVersion }) }
                }
            });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim("fullname", user.Fullname ?? ""),
        new Claim(ClaimTypes.Role, "User") // یا از Identity Role واقعی بخون
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
