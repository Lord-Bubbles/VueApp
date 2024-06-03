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
        await repository.UpdateAsync(id, user);
        return Ok("User successfully updated!");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await repository.DeleteAsync(id);
        return Ok("User successfully deleted!");
    }
}
