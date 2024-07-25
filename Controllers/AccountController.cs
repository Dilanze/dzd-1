using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseTrackerApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ExpenseTrackerApp.Dto;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ExpenseTrackerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ExpenseTrackerDbContext _dbContext;
        private readonly IConfiguration _config;

        public AccountController(ExpenseTrackerDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            string secret = _config["JwtTokenConfig:Secret"];
            string issuer = _config["JwtTokenConfig:Issuer"];
            string audience = _config["JwtTokenConfig:Audience"];
            int accessTokenExpire = Convert.ToInt32(_config["JwtTokenConfig:AccessTokenExpire"]);

            var key = Encoding.UTF8.GetBytes(secret);

            var pass = HashText("123456", secret);

            if (model == null)
            {
                return NotFound();
            }

            var user = await _dbContext.User.FirstOrDefaultAsync(p => p.Email == model.Username && p.IsActive);

            if (user == null)
            {
                return NotFound("Kullanıcı yok");
            }

            if (!VerifyHashedText(model.Password, user.Password, secret))
            {
                return BadRequest("Hatalı kullanıcı bilgisi");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.GivenName, user.Fullname)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescription = new SecurityTokenDescriptor
            {
                Audience = audience,
                Issuer = issuer,
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(accessTokenExpire),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);

            var result = new
            {
                Id = user.Id,
                Name = user.Email,
                Fullname = user.Fullname,
                Token = tokenHandler.WriteToken(token)
            };

            return Ok(result);
        }

        private static string HashText(string clearText, string secret)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(secret);
            byte[] hashBytes = KeyDerivation.Pbkdf2(
                password: clearText,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
            );
            return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }

        private static bool VerifyHashedText(string clearText, string hashedText, string secret)
        {
            string result = HashText(clearText, secret);
            return hashedText == result;
        }
    }
}
