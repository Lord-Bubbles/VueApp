using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace VueApp1.Server.Models;

public class User
{
  public int ID { get; set; }

  public string FirstName { get; set; } = "";

  public string LastName { get; set; } = "";

  public int Age { get; set; }

  public string Email { get; set; } = "";

  public DateTime Birthday { get; set; }

  public string PhoneNum { get; set; } = "";

  public string ManagerName { get; set; } = "";

  [JsonIgnore]
  public string HashPassword { get; set; } = "";

  public int AccountID { get; set; }

  public virtual Account Account { get; set; }

  public virtual ICollection<Performance> Performances { get; set; }
}

public class AccountInfo
{
  [Required]
  public string Username { get; set; }

  [Required]
  public string Password { get; set; }
}

public class AuthenticateResponse
{
  public int ID { get; set; }

  public string FirstName { get; set; }

  public string LastName { get; set; }

  public string Email { get; set; }

  public string Token { get; set; }
}

public class UpdateRequest : UserView
{
  public string Password { get; set; }
}

public class UserView
{
  public int ID { get; }

  public string FirstName { get; set; }

  public string LastName { get; set; }

  public int Age { get; set; }

  public string Email { get; set; }

  public DateTime Birthday { get; set; }

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



