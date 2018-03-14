using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/values")]
    public class ValuesController : Controller
    {
        private readonly IConfiguration Configuration;

        public ValuesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            var userName = User.Identity.Name;

            return Ok($"Super secret content, I hope you've got clearance for this {userName}...");
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {
            if (request.Username == "hung" && request.Password == "hung12345")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim("CompletedBasicTraining", ""),
                    
                };
                
                var ep = new EncryptingCredentials(
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey_2"])),
                    alg: SecurityAlgorithms.Aes128KW,
                    enc: SecurityAlgorithms.Aes128CbcHmacSha256);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey_1"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //var token = new JwtSecurityToken(
                //    issuer: "yourdomain.com",
                //    audience: "yourdomain.com",
                //    claims: claims,
                //    expires: DateTime.Now.AddMinutes(30),
                //    signingCredentials: creds
                //    );

                var handler = new JwtSecurityTokenHandler();

                var jwtSecurityToken = handler.CreateJwtSecurityToken(
                    issuer: "yourdomain.com",
                    audience: "yourdomain.com",
                    subject: new ClaimsIdentity(claims),
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),
                    issuedAt: DateTime.Now,
                    signingCredentials: creds,
                    encryptingCredentials: ep);
                
                return Ok(new
                {
                    //token = new JwtSecurityTokenHandler().WriteToken(token)
                    token = handler.WriteToken(jwtSecurityToken)
            });
            }

            return BadRequest("Could not verify username and password");
        }

    }

    public class TokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

