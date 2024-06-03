using Microsoft.EntityFrameworkCore;
using VueApp1.Server.Models.Entities;

namespace VueApp1.Server.Helpers;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.UseSerialColumns();
  }

  public DbSet<User> Users { get; set; }

  public DbSet<Performance> Performances { get; set; }
}