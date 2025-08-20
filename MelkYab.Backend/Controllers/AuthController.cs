using MelkYab.Backend.Data.Dtos;
using MelkYab.Backend.Data.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, LinkGenerator linkGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            if (user == null)
                return Unauthorized(new
                {
                    message = "Invalid login attempt.",
                    links = new[]
                    {
                        new { rel = "register", method = "POST", href = _linkGenerator.GetPathByAction("Register", "Auth", new { version = ApiVersion }) }
                    }
                });

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, false, false);
            if (!result.Succeeded)
                return Unauthorized(new
                {
                    message = "Invalid login attempt.",
                    links = new[]
                    {
                        new { rel = "register", method = "POST", href = _linkGenerator.GetPathByAction("Register", "Auth", new { version = ApiVersion }) }
                    }
                });

            return Ok(new
            {
                message = "Login successful",
                user = new
                {
                    user.Id,
                    user.Email,
                    user.Fullname
                },
                links = new[]
                {
                    new { rel = "me", method = "GET", href = _linkGenerator.GetPathByAction("GetCurrentUser", "Auth", new { version = ApiVersion }) },
                    new { rel = "logout", method = "DELETE", href = _linkGenerator.GetPathByAction("Logout", "Auth", new { version = ApiVersion }) }
                }
            });
        }

        // GET: api/v1/auth/me
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound(new
                {
                    message = "User not found.",
                    links = new[]
                    {
                        new { rel = "login", method = "POST", href = _linkGenerator.GetPathByAction("Login", "Auth", new { version = ApiVersion }) },
                        new { rel = "register", method = "POST", href = _linkGenerator.GetPathByAction("Register", "Auth", new { version = ApiVersion }) }
                    }
                });

            return Ok(new
            {
                user.Id,
                user.Email,
                user.Fullname,
                user.Phone,
                user.CreatedAt,
                links = new[]
                {
                    new { rel = "logout", method = "DELETE", href = _linkGenerator.GetPathByAction("Logout", "Auth", new { version = ApiVersion }) }
                }
            });
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
    }
}
