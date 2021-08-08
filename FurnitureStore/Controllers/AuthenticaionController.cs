using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FurnitureStore.Entity;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FurnitureStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        //private readonly IOptions<EmailSettingsModel> _emailSettings;

        public AuthenticationController(ApplicationDbContext db, IConfiguration config /*IOptions<EmailSettingsModel> emailSettings*/)
        {
            _db = db;
            _configuration = config;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginModel lmodel)
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            var user = _db.User.Where(u => u.Username == lmodel.Username).FirstOrDefault();
            if (user != null)
            {
                if (user.Password == lmodel.Password)
                {
                    
                        string userRole = user.Admin == "Yes" ? "Admin" : "Customer";
                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim("Role", userRole)
                        };

                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                        var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddHours(3),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    
                    //else
                    //{
                    //    return Unauthorized(new Response { Status = "unAuthorised", Message = "Please complete E-mail verification to login" });
                    //}
                }
                else
                {
                    return Unauthorized(new Response { StatusCode = "unAuthorised", Message = "UserName or Password incorrect" });
                }

            }
            else
            {
                return Unauthorized(new Response { StatusCode = "UnAuthorised", Message = "Please register and complete E-mail verification to login" });
            }
        }
    }
}