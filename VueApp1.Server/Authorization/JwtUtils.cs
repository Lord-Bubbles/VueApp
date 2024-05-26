using VueApp1.Server.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;


namespace VueApp1.Server.Authorization;

public interface IJwtUtils
{
  public string GenerateToken(User user);
  public int ValidateToken(string token);
}

public class JwtUtils : IJwtUtils
{
  private static readonly byte[] secret = RandomNumberGenerator.GetBytes(16);

  public string GenerateToken(User user)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity([new Claim("id", user.ID.ToString())]),
      Expires = DateTime.UtcNow.AddDays(1),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha512Signature),
      IssuedAt = DateTime.UtcNow
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

  public int ValidateToken(string token)
  {
    if (token == null)
    {
      return -1;
    }

    var tokenHandler = new JwtSecurityTokenHandler();
    try
    {
      tokenHandler.ValidateToken(token, new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secret),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
      }, out SecurityToken validatedToken);

      var jwtToken = (JwtSecurityToken)validatedToken;
      var userID = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

      return userID;
    }
    catch
    {
      return -1;
    }
  }
}