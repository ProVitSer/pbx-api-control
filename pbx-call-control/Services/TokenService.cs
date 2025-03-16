using PbxApiControl.Interface;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Grpc.Core;

namespace PbxApiControl.Services
{
    public class TokenService :  ITokenValidationService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secretKey;

    public TokenService(IConfiguration configuration)
        {
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _secretKey = configuration["Jwt:Key"];
        }

        public bool ValidateToken(string token)
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                return true;
            }
            catch (SecurityTokenSignatureKeyNotFoundException)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "The signature key was not found"));
            }
            catch (SecurityTokenExpiredException)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "The token has expired"));
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid token signature"));
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Unknown, $"Token validation error: {ex.Message}"));
            }
        }

        public void GenerateToken()
        {
            var gtokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "provider"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var gtoken = gtokenHandler.CreateToken(tokenDescriptor);
            var tokenString = gtokenHandler.WriteToken(gtoken);

            Console.WriteLine($"Generated Token: {tokenString}");

        }

    }
}

