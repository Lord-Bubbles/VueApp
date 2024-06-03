
namespace VueApp1.Server;

using VueApp1.Server.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VueApp1.Server.Services;
using VueApp1.Server.Authorization;

public class Program
{
  public static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddAutoMapper(typeof(Program));

    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IPerformanceRepository, PerformanceRepository>();
    builder.Services.AddScoped<IJwtUtils, JwtUtils>();


    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(option =>
    {
      option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
      option.AddSecurityDefinition("JWT", new OpenApiSecurityScheme()
      {
        In = ParameterLocation.Header,
        Description = "Please insert JWT token into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
      });
      option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="JWT"
                        }
                    },
                    Array.Empty<string>()
                }
        });
    });

    var app = builder.Build();

    app.UseDefaultFiles();
    app.UseStaticFiles();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors();


    app.UseAuthorization();

    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();

    app.Run();
  }
}
