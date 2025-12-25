using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoList.Application.Interfaces;
using TodoList.Config;
using TodoList.Domain.Entities;

namespace TodoList.Application.Services;

public class TokenService(IOptions<JwtConfigs> _configs) : ITokenService
{
    public string CreateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configs.Value.Secret)), SecurityAlgorithms.HmacSha256)
        {

        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier,user.ID.ToString()),]),
            Expires = DateTime.UtcNow.AddHours(_configs.Value.ExpirationInHour),
            SigningCredentials = credentials,
            Issuer = _configs.Value.Issuer,
            Audience = _configs.Value.Audience

        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
