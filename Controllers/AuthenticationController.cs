using dotnetapp.Models;
using dotnetapp.Data;
using dotnetapp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using log4net;
using System.Reflection;
using Microsoft.AspNetCore.Identity;

namespace dotnetapp.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        public AuthenticationController(ApplicationDbContext context, IAuthService authService)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            log.Info($"Login attempt initiated for user: {model.Email}");
            try
            {
                var result = await _authService.Login(model);

                if (result.Item1 == 0)
                {
                    log.Warn($"Login failed for user: {model.Email}. Reason: {result.Item2}");
                    return BadRequest(new { Message = result.Item2 });
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    log.Error($"User not found after successful login for: {model.Email}");
                    return BadRequest(new { Message = "User details not found." });
                }

                log.Info($"Login successful for user: {model.Email}");

                // The fix: Capitalize 'User' for the returned property
                return Ok(new
                {
                    token = result.Item2,
                    User = new
                    {
                        UserId = user.UserId,
                        Email = user.Email,
                        Username = user.Username,
                        MobileNumber = user.MobileNumber,
                        UserRole = user.UserRole
                    }
                });
            }
            catch (Exception ex)
            {
                log.Error($"Unexpected error during login for user: {model.Email}", ex);
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(User model)
        {
            log.Info($"Registration attempt initiated for user: {model.Email}");

            try
            {
                if (!ModelState.IsValid)
                {
                    log.Warn($"Registration failed due to invalid model state for user: {model.Email}");
                    return BadRequest(ModelState);
                }

                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    log.Warn($"Registration failed: Email already exists for user: {model.Email}");
                    return BadRequest(new { Message = "Email already exists. Please use a different email." });
                }

                if (await _context.Users.AnyAsync(u => u.MobileNumber == model.MobileNumber))
                {
                    log.Warn($"Registration failed: Mobile number already exists for user: {model.MobileNumber}");
                    return BadRequest(new { Message = "Mobile number already exists. Please use a different number." });
                }

                if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                {
                    log.Warn($"Registration failed: Username already exists for user: {model.Username}");
                    return BadRequest(new { Message = "Username already exists. Please choose a different username." });
                }

                var result = await _authService.Registration(model, model.UserRole);
                if (result.Item1 != 1)
                {
                    return StatusCode(result.Item1, new { Message = result.Item2 });
                }

                log.Info($"Registration successful for user: {model.Email}");
                return Ok(new { Message = result.Item2 });
            }
            catch (Exception ex)
            {
                log.Error($"Unexpected error during registration for user: {model.Email}", ex);
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }


        [HttpPut("updateProfile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            // Update Email and MobileNumber
            user.Email = updatedUser.Email;
            user.MobileNumber = updatedUser.MobileNumber;

            // Change password if provided
            if (!string.IsNullOrWhiteSpace(updatedUser.Password))
            {
                var passwordHasher = new PasswordHasher<User>();
                user.Password = passwordHasher.HashPassword(user, updatedUser.Password);
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Profile updated successfully." });
        }

    }
}