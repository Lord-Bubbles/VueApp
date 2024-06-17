namespace VueApp1.Server.Controllers;

using Microsoft.AspNetCore.Mvc;
using VueApp1.Server.Models.Entities;
using VueApp1.Server.Services;
using VueApp1.Server.Authorization;
using AutoMapper;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController(IUserRepository repository, IMapper mapper) : ControllerBase
{
    private readonly IUserRepository repository = repository;

    private readonly IMapper mapper = mapper;

    [HttpGet]
    public IActionResult GetUsers([FromQuery] UserParameters query)
    {
        var authUser = (User)HttpContext.Items["User"];
        // Only Admins are allowed to access all users' infos
        if (authUser.AccountType != Models.Enums.Account.Admin)
        {
            return Unauthorized();
        }
        if (query.Page <= 0 || query.Limit <= 0)
        {
            return BadRequest("Error: invalid page and limit parameters!");
        }
        var (users, count) = repository.GetAll(query);
        return Ok(new { users, count });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var authUser = (User)HttpContext.Items["User"];
        // Only Admins are allowed to access others' infos. Else user only allowed to access own info
        if (authUser.ID != id && authUser.AccountType != Models.Enums.Account.Admin)
        {
            return Unauthorized();
        }
        var user = await repository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(mapper.Map<UserView>(user));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateRequest user)
    {
        var authUser = (User)HttpContext.Items["User"];
        // Only Admins are allowed to change other people's user info. Else user only allowed to change own info
        if (authUser.ID != id && authUser.AccountType != Models.Enums.Account.Admin)
        {
            return Unauthorized();
        }
        var updatedUser = await repository.UpdateAsync(id, user);
        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var authUser = (User)HttpContext.Items["User"];
        // Only Admins are allowed to delete users
        if (authUser.AccountType != Models.Enums.Account.Admin)
        {
            return Unauthorized();
        }
        await repository.DeleteAsync(id);
        return Ok("User successfully deleted!");
    }
}
