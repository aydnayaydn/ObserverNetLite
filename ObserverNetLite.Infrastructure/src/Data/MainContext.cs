using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ObserverNetLite.Data.Mappings;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Data;

public class ObserverNetLiteDbContext(DbContextOptions<ObserverNetLiteDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {   
        // optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));     
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure your entity mappings here
        UserMapping.OnModelCreating(modelBuilder.Entity<User>());
    }
}
