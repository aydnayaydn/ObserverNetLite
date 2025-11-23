using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ObserverNetLite.Data.Mappings;
using ObserverNetLite.Core.Entities;

namespace ObserverNetLite.Data;

public class ObserverNetLiteDbContext(DbContextOptions<ObserverNetLiteDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuPermission> MenuPermissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {   
        // optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));     
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ObserverNetLiteDbContext).Assembly);
        
        // Configure User entity
        UserMapping.OnModelCreating(modelBuilder.Entity<User>());
    }
}
