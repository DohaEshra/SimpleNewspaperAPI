using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newspaper.Data;
using Newspaper.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        NewspaperdbContext db;    
        public LoginController(NewspaperdbContext _db)
        {
            this.db = _db;
        }

        [HttpPost]
        public IActionResult LoginPost(string username, string password, string role)
        {
            User u = db.Users.Where(n => n.Username == username && n.Password == password && n.Role == role).FirstOrDefault();
            if(u!=null)// an existing user
            {
                //generate token 
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKey00000"));    

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var data = new List<Claim>();
                data.Add(new Claim("ID", u.UsrID.ToString()));
                data.Add(new Claim("Username", u.Username));
                data.Add(new Claim("Password", u.Password));
                data.Add(new Claim(ClaimTypes.Role, u.Role));

                var token = new JwtSecurityToken(
                  claims:data,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));

            }
            else// not registered
            {
                return Unauthorized(); //401
            }
        }
    }
}
