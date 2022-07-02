using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        ApplicationContext db;
        private readonly IConfiguration configuration;
        private readonly ITokenService tokenService;
        private readonly IPasswordHashService passwordHashService;


        public AuthController(IConfiguration configuration, ApplicationContext context, ITokenService tokenService, IPasswordHashService passwordHashService)
        {
            this.configuration = configuration;
            this.tokenService = tokenService;
            this.passwordHashService = passwordHashService;
            db = context;
        }

        [HttpPost("registration")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            if (ModelState.IsValid)
            {

                User? candidate = GetData(request);

                if (candidate != null)
                {
                    return BadRequest("user with email already exist");
                }

                var user = NewUser(request);

                return Ok("user created");
            }
            else
            {
                return BadRequest("model is not valid");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserDto request)
        {
            if (ModelState.IsValid)
            {
                User? user = GetData(request);

                if (user == null) return BadRequest("User not found.");

                if (user.Email != request.Email)
                {
                    return BadRequest("User not found.");
                }

                if (!passwordHashService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    return BadRequest("Wrong password.");
                }

                string token = tokenService.CreateToken(user);

                var response = new
                {
                    access_token = token,
                    user_res = user
                };

                return Ok(response);
            }
            else
            {
                return BadRequest("model is not valid");
            }
        }

        [Authorize]
        [HttpGet("auth")]
        public async Task<ActionResult> Auth()
        {
            User? user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            var response = new
            {
                user_res = user
            };

            return Ok(response);
        }

        public User GetData(UserDto request)
        {
            User? user = db.Users.FirstOrDefault(x => x.Email == request.Email);
            return user;
        }

        public async Task<User> NewUser(UserDto request)
        {
            passwordHashService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User? user = new User();
            Basket basket = new Basket();

            basket.User = user;

            user.Email = request.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = request.Role;
            user.Basket = basket;

            db.Users.Add(user);
            db.Baskets.Add(basket);
            await db.SaveChangesAsync();

            return user;
        }
    }
}
