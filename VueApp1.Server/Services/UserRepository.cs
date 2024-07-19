namespace VueApp1.Server.Services;

using Microsoft.EntityFrameworkCore;
using VueApp1.Server.Helpers;
using VueApp1.Server.Models.Entities;
using BCrypt.Net;
using VueApp1.Server.Authorization;
using AutoMapper;
using System.Data;
using VueApp1.Server.Models.Enums;

public interface IUserRepository
{
  (IEnumerable<UserView>, int) GetAll(UserParameters query);
  Task<User?> GetByIdAsync(int id);
  Task<User> UpdateAsync(int id, UpdateRequest entity);
  Task DeleteAsync(int id);
  Task CreateUserAsync(RegisterRequest info);
  Task<AuthenticateResponse> LoginAsync(LoginRequest user);
  Task<User?> GetByTokenAsync(string token);
  void Logout(User user);
  AuthenticateResponse GenerateTokens(User user);
}

public class UserRepository(AppDbContext context, IJwtUtils jwtUtils, IMapper mapper) : IUserRepository
{
  protected readonly AppDbContext context = context;

  protected readonly IJwtUtils jwtUtils = jwtUtils;

  protected readonly IMapper mapper = mapper;

  public async Task DeleteAsync(int id)
  {
    var user = await context.Users.FindAsync(id);
    context.Users.Remove(user);
    context.SaveChanges();
  }

  public (IEnumerable<UserView>, int) GetAll(UserParameters query)
  {
    var filter = context.Users.AsQueryable();
    if (!string.IsNullOrEmpty(query.Type))
    {
      var accountType = (Account)Enum.Parse(typeof(Account), query.Type);
      filter = filter.Where(u => u.AccountType == accountType);
    }

    if (query.MinAge > 0 && query.MinAge <= query.MaxAge)
    {
      filter = filter.Where(u => u.Age >= query.MinAge);
    }
    if (query.MaxAge > 0 && query.MaxAge >= query.MinAge)
    {
      filter = filter.Where(u => u.Age <= query.MaxAge);
    }
    if (!string.IsNullOrEmpty(query.ManagerName))
    {
      filter = filter.Where(u => u.ManagerName == query.ManagerName);
    }
    if (!string.IsNullOrEmpty(query.Name))
    {
      filter = filter.Where(u => (u.FirstName + " " + u.LastName).ToLower().Contains(query.Name.ToLower()));
    }
    return (filter.Skip((query.Page - 1) * query.Limit).Take(query.Limit).ToList().Select(mapper.Map<UserView>), filter.Count());
  }

  public async Task<User?> GetByIdAsync(int id)
  {
    return await context.Users.FindAsync(id);
  }

  public async Task<User> UpdateAsync(int id, UpdateRequest entity)
  {
    var user = await GetByIdAsync(id) ?? throw new KeyNotFoundException("User doesn't exist!");

    if (!string.IsNullOrEmpty(entity.Password))
    {
      user.HashPassword = BCrypt.HashPassword(entity.Password, BCrypt.GenerateSalt(13), true);
    }

    if (!string.IsNullOrEmpty(entity.AccountType))
    {
      user.AccountType = (Account)Enum.Parse(typeof(Account), entity.AccountType);
    }
    var today = DateTime.Today;
    user.Age = today.Year - entity.Birthday.Year;
    if (today.Month < entity.Birthday.Month || (today.Month == entity.Birthday.Month && today.Day < entity.Birthday.Day))
    {
      user.Age -= 1;
    }
    mapper.Map(entity, user);
    context.SaveChanges();
    return user;
  }

  public async Task CreateUserAsync(RegisterRequest info)
  {
    var found = await context.Users.FirstOrDefaultAsync(u => u.Email == info.Email);
    if (found != null)
    {
      throw new ApplicationException("User already exists!");
    }
    string salt = BCrypt.GenerateSalt(13);
    string hashPassword = BCrypt.HashPassword(info.Password, salt, true);
    var user = mapper.Map<User>(info);
    user.HashPassword = hashPassword;
    user.AccountType = Account.Admin;
    context.Users.Add(user);
    context.SaveChanges();
  }

  public async Task<AuthenticateResponse> LoginAsync(LoginRequest login)
  {
    var found = await context.Users.SingleOrDefaultAsync(u => u.Email == login.Username);
    if (found == null || !BCrypt.Verify(login.Password, found.HashPassword, true))
    {
      throw new ApplicationException("Username or password incorrect!");
    }
    return GenerateTokens(found);
  }

  public async Task<User?> GetByTokenAsync(string token)
  {
    return await context.Users.SingleOrDefaultAsync(u => u.RefreshToken == token);
  }

  public void Logout(User user)
  {
    user.Logout();
    context.SaveChanges();
  }

  public AuthenticateResponse GenerateTokens(User user)
  {
    user.SetToken(jwtUtils.GenerateRefreshToken(), DateTime.UtcNow.AddDays(2));
    context.SaveChanges();
    return new AuthenticateResponse()
    {
      AccessToken = jwtUtils.GenerateAccessToken(user),
      RefreshToken = user.RefreshToken,
      RefreshTokenExpiry = user.RefreshTokenExpires,
      User = mapper.Map<UserView>(user)
    };
  }
}
