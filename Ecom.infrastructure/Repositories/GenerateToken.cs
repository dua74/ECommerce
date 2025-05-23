using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.Entities;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Ecom.infrastructure.Repositories
{
    public class GenerateToken : IGenerateToken
    {
        private readonly IConfiguration configuration;
        public GenerateToken(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GetAndCreateToken(AppUser user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),


                new Claim(ClaimTypes.Email, user.Email)
            };
            var Security = configuration["Token: Secret"];
            var Key= Encoding.ASCII.GetBytes(Security);
            SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey (Key), SecurityAlgorithms.HmacSha256);
            SecurityTokenDescriptor tokenDescriotor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = configuration["Token:Issuer"],
                SigningCredentials = credentials,
                NotBefore = DateTime.Now,


            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token= handler.CreateToken(tokenDescriotor);
            return  handler.WriteToken(token);
        }
    }
}
