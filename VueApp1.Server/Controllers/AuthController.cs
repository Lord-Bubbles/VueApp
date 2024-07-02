using Microsoft.AspNetCore.Mvc;
using VueApp1.Server.Authorization;
using VueApp1.Server.Models.Entities;
using VueApp1.Server.Services;

namespace VueApp1.Server.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController(IUserRepository userRepository) : ControllerBase
  {
    private readonly IUserRepository userRepository = userRepository;

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequest info)
    {
      var response = await userRepository.LoginAsync(info);
      SetCookies(response);
      return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateAccount([FromBody] RegisterRequest info)
    {
      await userRepository.CreateUserAsync(info);
      return Ok("Account successfully created!");
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> CreateRefreshToken()
    {
      var cookie = Request.Cookies["refreshToken"];
      var user = await userRepository.GetByTokenAsync(cookie);

      if (user == null || user.RefreshTokenExpires < DateTime.UtcNow)
      {
        // If refresh token is not valid or has expired, remove from database and clear cookie
        if (user != null)
        {
          userRepository.Logout(user);
        }
        Response.Cookies.Delete("refreshToken");
        return Unauthorized();
      }
      var response = userRepository.GenerateTokens(user);
      SetCookies(response);
      return Ok(response);
    }

    [HttpDelete("logout"), Authorize]
    public IActionResult Logout()
    {
      var user = (User)HttpContext.Items["User"];
      if (user != null)
      {
        userRepository.Logout(user);
      }
      Response.Cookies.Delete("refreshToken");
      return Ok("Successfully logged out!");
    }

    private void SetCookies(AuthenticateResponse response)
    {
      var cookieOptions = new CookieOptions()
      {
        HttpOnly = true,
        Expires = response.RefreshTokenExpiry,
        SameSite = SameSiteMode.Strict,
        Secure = true
      };
      Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);
    }
  }
}
