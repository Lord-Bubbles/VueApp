using Microsoft.EntityFrameworkCore;
using VueApp1.Server.Models;

namespace VueApp1.Server.Helpers;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<Performance> Performances { get; set; }

}