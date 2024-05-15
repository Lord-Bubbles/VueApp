using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VueApp1.Server.Models;
using VueApp1.Server.Services;

namespace VueApp1.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository repository;

    public UserController(IUserRepository repository)
    {
        this.repository = repository;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromHeader(Name = "Authorization")] string authorization)
    {
        var user = await repository.Login(authorization);
        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> CreateAccount([FromBody] AccountInfo info)
    {
        var user = await repository.CreateAccount(info);
        return CreatedAtAction(nameof(CreateAccount), new { user.ID }, user);
    }

    [HttpGet]
    public IActionResult GetUsers([FromQuery] UserParameters query)
    {
        var (users, count) = repository.GetAll(query);
        return Ok(new { users, count });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await repository.GetByIdAsync(id);
        return Ok(user);
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
