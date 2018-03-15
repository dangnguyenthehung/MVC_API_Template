using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.Interfaces;

namespace Api.Security
{
    public class Helpers
    {
        public static string GetToken(ILogin request, IConfiguration configuration)
        {
            if (request.UserName == "hung" && request.Password == "hung12345")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.UserName),
                    new Claim("Authenticated", ""),

                };

                var ep = new EncryptingCredentials(
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecurityKey_1"])),
                    alg: SecurityAlgorithms.Aes256KW,
                    enc: SecurityAlgorithms.Aes256CbcHmacSha512);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecurityKey_2"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

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

                return handler.WriteToken(jwtSecurityToken);
            }
            else
            {
                return string.Empty;
            }

        }
    }
}
