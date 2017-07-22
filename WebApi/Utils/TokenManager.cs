using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Utils
{
    public class TokenManager
    {
        private TokenSettings settings;
        private JwtSecurityTokenHandler handler;

        public TokenManager(TokenSettings settings)
        {
            this.settings = settings;

            handler = new JwtSecurityTokenHandler();
        }

        public SymmetricSecurityKey GetSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.Secret));
        }

        public SigningCredentials GetCredentials()
        {
            return new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256);
		}

        public string CreateJwtToken(string name, string email, DateTime? expires = null)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email),
            };

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = this.settings.Issuer,
                Audience = this.settings.Audience,
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = GetCredentials(),
                Expires = expires
            };

            var token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }

        public JwtSecurityToken GetToken(string token){
            return handler.ReadToken(token) as JwtSecurityToken;
        }
    }
}
