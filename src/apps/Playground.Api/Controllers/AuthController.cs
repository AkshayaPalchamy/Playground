using System.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playground.Api.Data;

namespace Playground.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Document")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService) 
        {
            _context = context;
            _jwtService = jwtService;
        }

        //POST - register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var Exists = await _context.Users.AnyAsync(u=>u.UserName == user.UserName);
            if (Exists)
                return BadRequest("UserName already Exist");
            if(user.Role != "Admin" && user.Role != "Developer")
            {
                return BadRequest("Role must be Admin or Developer");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginRequest request)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username && u.Password == request.Username);

            if(User == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var Token = _jwtService.GenerateToken(User);

            return Ok(new { Token = Token, Username = User.UserName, Role = User.Role, Message = "Login Successfull" });
        }
    }
}
