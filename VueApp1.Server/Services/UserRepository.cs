namespace VueApp1.Server.Services;

using Microsoft.EntityFrameworkCore;
using VueApp1.Server.Helpers;
using VueApp1.Server.Models;
using BCrypt.Net;
using System.Text;
using VueApp1.Server.Authorization;
using AutoMapper;

public interface IUserRepository
{
    (IEnumerable<UserView>, int) GetAll(UserParameters query);
    Task<UserView> GetByIdAsync(int id);
    Task UpdateAsync(int id, UpdateRequest entity);
    Task DeleteAsync(int id);
    Task<User> CreateAccount(AccountInfo info);
    Task<UserView> Login(string authorization);
}

public class UserRepository : IUserRepository
{
    protected readonly AppDbContext context;

    protected readonly IJwtUtils jwtUtils;

    protected readonly IMapper mapper;

    public UserRepository(AppDbContext context, IJwtUtils jwtUtils, IMapper mapper)
    {
        this.context = context;
        this.jwtUtils = jwtUtils;
        this.mapper = mapper;
    }

    public async Task DeleteAsync(int id)
    {
        var user = await context.Users.FindAsync(id) ?? throw new Exception("User not found!");
        context.Users.Remove(user);
        context.SaveChanges();
    }

    public (IEnumerable<UserView>, int) GetAll(UserParameters query)
    {
        if (query.Page <= 0 || query.Limit <= 0)
        {
            throw new Exception("Error: page and limit query parameters not valid!");
        }
        var filter = context.Users.Select(u => mapper.Map<UserView>(u));
        if (!string.IsNullOrEmpty(query.Type))
        {
            filter = filter.Where(u => u.AccountType == query.Type);
        }
        if (!string.IsNullOrEmpty(query.Name))
        {
            filter = filter.Where(u => u.FirstName.Contains(query.Name, StringComparison.CurrentCultureIgnoreCase) || u.LastName.Contains(query.Name, StringComparison.CurrentCultureIgnoreCase));
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
        return (filter.Skip((query.Page - 1) * query.Limit).Take(query.Limit).ToList(), filter.Count());
    }

    public async Task<UserView> GetByIdAsync(int id)
    {
        var user = await context.Users.Select(u => mapper.Map<UserView>(u)).FirstOrDefaultAsync(u => u.ID == id);
        return user ?? throw new Exception("User not found!");
    }

    public async Task UpdateAsync(int id, UpdateRequest entity)
    {
        var user = await context.Users.FindAsync(id) ?? throw new Exception("User not found!");

        if (!string.IsNullOrEmpty(entity.Password))
        {
            user.HashPassword = BCrypt.HashPassword(entity.Password, BCrypt.GenerateSalt(13), true);
        }

        mapper.Map(entity, user);
        context.Users.Update(user);
        context.SaveChanges();
    }

    public async Task<User> CreateAccount(AccountInfo info)
    {
        var found = await context.Users.FirstOrDefaultAsync(x => x.Email == info.Username);
        if (found != null)
        {
            throw new Exception("A user with this username already exists! Please log in instead.");
        }
        string salt = BCrypt.GenerateSalt(13);
        string hashPassword = BCrypt.HashPassword(info.Password, salt, true);
        var account = new User
        {
            Email = info.Username,
            HashPassword = hashPassword,
            AccountID = 2
        };
        context.Users.Add(account);
        context.SaveChanges();
        return account;
    }

    public async Task<UserView> Login(string authorization)
    {
        if (authorization == null)
        {
            throw new Exception("Authorization header not found!");
        }
        string[] temp = authorization.Trim().Split(" ");
        if (temp.Length != 2 || !temp[0].Equals("Basic"))
        {
            throw new Exception("Authorization header must be in the form Basic base64(username:password)!");
        }
        var buffer = new byte[temp[1].Length];
        if (!Convert.TryFromBase64String(temp[1], buffer, out _))
        {
            throw new Exception("Username and password must be base64(username:password)!");
        }
        byte[] data = Convert.FromBase64String(temp[1]);
        string[] decodedString = Encoding.UTF8.GetString(data).Split(":");
        if (decodedString.Length != 2)
        {
            throw new Exception("Base 64 encoded string needs to be of the form username:password");
        }
        string username = decodedString[0];
        string password = decodedString[1];
        var found = await context.Users.SingleOrDefaultAsync(m => m.Email == username);
        if (found == null || !BCrypt.Verify(password, found.HashPassword, true))
        {
            throw new Exception("The password is incorrect!");
        }
        return mapper.Map<UserView>(found);
    }
}