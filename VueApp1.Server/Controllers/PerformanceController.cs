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
    var (performances, count) = repository.GetAll(query);
    return Ok(new { performances, count });
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetPerformance(int id)
  {
    var performance = await repository.GetByIdAsync(id);
    return Ok(performance);
  }

  [HttpPost]
  public async Task<IActionResult> CreatePerformance([FromBody] Performance performance)
  {
    await repository.CreateAsync(performance);
    return Ok("Performance review successfully created!");
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeletePerformance(int id)
  {
    await repository.DeleteAsync(id);
    return Ok("Performance review successfully deleted!");
  }
}
