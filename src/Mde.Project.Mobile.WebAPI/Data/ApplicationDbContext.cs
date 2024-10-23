using Mde.Project.Mobile.WebAPI.Data.Seeding;
using Mde.Project.Mobile.WebAPI.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mde.Project.Mobile.WebAPI.Data;

public class ApplicationDbContext:IdentityDbContext<AppUser>{
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products{ get; set; }
    public DbSet<Cargo> Cargos { get; set; }
    public DbSet<AccessLevel> AccessLevels { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        
        modelBuilder.Entity<AppUser>()
            .HasOne(p => p.AccessLevel) // Assuming ApplicationUser has a navigation property AccessLevel
            .WithMany() // Assuming AccessLevel does not have a navigation collection property in ApplicationUser
            .HasForeignKey(p => p.AccessLevelId)
            .OnDelete(DeleteBehavior.SetNull); // Set 
        Seeder.Seed(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

}