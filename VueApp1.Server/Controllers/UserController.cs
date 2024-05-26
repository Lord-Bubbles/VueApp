namespace VueApp1.Server.Controllers;

using Microsoft.AspNetCore.Mvc;
using VueApp1.Server.Models;
using VueApp1.Server.Services;
using VueApp1.Server.Authorization;
using System.ComponentModel.DataAnnotations;

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

  [HttpPost("authenticate"), AllowAnonymous]
  public async Task<IActionResult> Authenticate([FromHeader(Name = "Authorization"), Required] string authorization)
  {
    var user = await repository.Login(authorization);
    return Ok(user);
  }

  [HttpPost("register"), AllowAnonymous]
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
