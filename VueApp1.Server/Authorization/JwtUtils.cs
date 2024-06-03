using VueApp1.Server.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace VueApp1.Server.Authorization;

public interface IJwtUtils
{
  public string GenerateAccessToken(User user);
  public int ValidateAccessToken(string token);
  public string GenerateRefreshToken();
}

public class JwtUtils : IJwtUtils
{
  public static readonly byte[] secret = RandomNumberGenerator.GetBytes(64);

  public string GenerateAccessToken(User user)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var claims = new Dictionary<string, object> {
      {"id", Convert.ToString(user.ID)},
      {JwtRegisteredClaimNames.Name, user.FirstName},
      {JwtRegisteredClaimNames.FamilyName, user.LastName},
      {JwtRegisteredClaimNames.Email, user.Email}
    };
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Claims = claims,
      Expires = DateTime.UtcNow.AddHours(1),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha512Signature),
      IssuedAt = DateTime.UtcNow
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

  public int ValidateAccessToken(string token)
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

  public string GenerateRefreshToken()
  {
    return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
  }
}
