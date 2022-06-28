using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace shop
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        ApplicationContext db;
        private readonly IPasswordHashService passwordHashService;

        public UserController(ApplicationContext context, IPasswordHashService passwordHashService)
        {
            db = context;
            this.passwordHashService = passwordHashService;
        }

        [HttpGet("getme")]
        public async Task<ActionResult<User>> GetMe()
        {
            if (User.Identity == null) return NotFound("хде jwt");

            User? user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            if (user == null) return NotFound("user not found");

            return Ok(user);
        }

        [HttpPut("updateuser")]
        public async Task<ActionResult> UpdateUser(PasswordDto request)
        {
            if (User.Identity == null) return NotFound("хде jwt");

            User? user = db.Users.FirstOrDefault(x => x.Id == request.Id);

            if (user == null) return NotFound("user not found");

            passwordHashService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
    }
}