using PbxApiControl.Interface;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

namespace PbxApiControl.Services
{
    public class TokenValidationService : ITokenValidationService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secretKey;
        public TokenValidationService(IConfiguration configuration)
        {
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _secretKey = configuration["Jwt:Key"];
            
        }

        public bool ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            
            try
            {
                var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_secretKey));

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(hmac.Key),
                    RequireSignedTokens = false,
                    ValidateTokenReplay = false
                };

                SecurityToken validatedToken;
                
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

                return true;
            }
            catch (Exception e)
            {              
                return false;
            }
        }
    }
}

