using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using VueApp1.Server.Models.Enums;

namespace VueApp1.Server.Models.Entities;

public class User
{
  public int ID { get; set; }

  public string FirstName { get; set; }

  public string LastName { get; set; }

  public int Age { get; set; }

  public string Email { get; set; }

  public DateOnly Birthday { get; set; }

  public Account AccountType { get; set; }

  public string PhoneNum { get; set; } = string.Empty;

  public string ManagerName { get; set; } = string.Empty;

  public string? RefreshToken { get; set; }

  public DateTime RefreshTokenExpires { get; set; }

  [JsonIgnore]
  public string HashPassword { get; set; }

  public virtual ICollection<Performance> Performances { get; set; } = [];

  public void SetToken(string token, DateTime expires)
  {
    RefreshToken = token;
    RefreshTokenExpires = expires;
  }

  public void Logout()
  {
    RefreshToken = null;
  }
}

public class RegisterRequest
{
  [Required]
  public string Email { get; set; }

  [Required]
  public string Password { get; set; }

  [Required]
  public string FirstName { get; set; }

  [Required]
  public string LastName { get; set; }
}

public class LoginRequest
{
  [Required]
  public string Username { get; set; }

  [Required]
  public string Password { get; set; }
}

public class AuthenticateResponse
{
  public string AccessToken { get; set; }

  [JsonIgnore]
  public string RefreshToken { get; set; }

  [JsonIgnore]
  public DateTime RefreshTokenExpiry { get; set; }

  public UserView User { get; set; }
}

public class UpdateRequest
{
  public string? FirstName { get; set; }

  public string? LastName { get; set; }

  public string? Email { get; set; }

  public DateOnly Birthday { get; set; }

  public string? PhoneNum { get; set; }

  public string? ManagerName { get; set; }

  public string? AccountType { get; set; }

  public string? Password { get; set; }
}

public class UserView
{
  public int ID { get; set; }

  public string FirstName { get; set; }

  public string LastName { get; set; }

  public int Age { get; set; }

  public string Email { get; set; }

  public DateOnly Birthday { get; set; }

  public string PhoneNum { get; set; }

  public string ManagerName { get; set; }

  public string AccountType { get; set; }
}

public class UserParameters : QueryParameters
{
  [FromQuery(Name = "managerName")]
  public string? ManagerName { get; set; }

  [FromQuery(Name = "type")]
  public string? Type { get; set; }

  [FromQuery(Name = "name")]
  public string? Name { get; set; }

  [FromQuery(Name = "minAge")]
  public int MinAge { get; set; }

  [FromQuery(Name = "maxAge")]
  public int MaxAge { get; set; }

  [FromQuery(Name = "email")]
  public string? Email { get; set; }
}
