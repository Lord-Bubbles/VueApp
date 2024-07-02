namespace VueApp1.Server.Controllers;

using Microsoft.AspNetCore.Mvc;
using VueApp1.Server.Models.Entities;
using VueApp1.Server.Services;
using VueApp1.Server.Authorization;


[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PerformanceController(IPerformanceRepository repository) : ControllerBase
{
  private readonly IPerformanceRepository repository = repository;

  [HttpGet]
  public IActionResult GetPerformances([FromQuery] PerformanceParameters query)
  {
    var authUser = (User)HttpContext.Items["User"];
    if (query.UserID != authUser.ID)
    {
      return Unauthorized();
    }
    if (query.Page <= 0 || query.Limit <= 0)
    {
      return BadRequest("page and limit query parameters not valid!");
    }
    var (performances, count) = repository.GetAll(query);
    return Ok(new { performances, count });
  }

  [HttpGet("{id}")]
  // Not implemented
  public async Task<IActionResult> GetPerformance(int id)
  {
    var performance = await repository.GetByIdAsync(id);
    return Ok(performance);
  }

  [HttpPost]
  public async Task<IActionResult> CreatePerformance([FromBody] Performance performance)
  {
    var authUser = (User)HttpContext.Items["User"];
    if (authUser.ID != performance.UserID)
    {
      return Unauthorized();
    }
    await repository.CreateAsync(performance);
    return Ok("Performance review successfully created!");
  }

  [HttpDelete("{id}")]
  // Not implemented
  public async Task<IActionResult> DeletePerformance(int id)
  {
    await repository.DeleteAsync(id);
    return Ok("Performance review successfully deleted!");
  }
}
