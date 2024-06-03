namespace VueApp1.Server.Authorization;

using VueApp1.Server.Services;

public class JwtMiddleware(RequestDelegate next)
{
  private readonly RequestDelegate _next = next;

  public async Task Invoke(HttpContext context, IUserRepository userRepository, IJwtUtils jwtUtils)
  {
    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
    var userID = jwtUtils.ValidateAccessToken(token);
    if (userID > 0)
    {
      context.Items["User"] = await userRepository.GetByIdAsync(userID);
    }

    await _next(context);
  }
}
