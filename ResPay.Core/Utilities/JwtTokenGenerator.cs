using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ResPay.Core.Utilities
{
    public static class JwtTokenGenerator
    {
        public static object GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _configuration)
        {
            var token = new JwtSecurityToken
                (
                    issuer: _configuration.GetIssuer(),
                    audience: _configuration.GetAudience(),
                    expires: DateTime.Now.AddDays(1),
                    claims: claims,
                    signingCredentials: _configuration.GetSigningCredentials()
                );

            var validToken = new JwtSecurityTokenHandler().WriteToken(token);
            return validToken;
        }



        public static string GetAudience(this IConfiguration configuration)
        {
            var result = configuration.GetValue<string>("JwtSettings:Audience");
            return result;
        }

        public static string GetIssuer(this IConfiguration configuration)
        {
            var result = configuration.GetValue<string>("JwtSettings:Issuer");
            return result;
        }


        public static SigningCredentials GetSigningCredentials(this IConfiguration configuration)
        {
            return new SigningCredentials(configuration.GetSymmetricSigningKey(), SecurityAlgorithms.HmacSha256);
        }

        public static SymmetricSecurityKey GetSymmetricSigningKey(this IConfiguration configuration)
        {
            var securityKey = configuration.GetIssuerSecurityKey();
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

        }

        public static string GetIssuerSecurityKey(this IConfiguration configuration)
        {
            var result = configuration.GetValue<string>("JwtSettings:Secret");
            return result;
        }

    }
}
