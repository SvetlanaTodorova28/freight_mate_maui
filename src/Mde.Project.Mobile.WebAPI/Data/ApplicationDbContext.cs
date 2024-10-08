using Mde.Project.Mobile.WebAPI.Data.Seeding;
using Mde.Project.Mobile.WebAPI.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mde.Project.Mobile.WebAPI.Data;

public class ApplicationDbContext:IdentityDbContext<AppUser>{
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products{ get; set; }
    public DbSet<Cargo> Cargos { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}


   


    protected override void OnModelCreating(ModelBuilder modelBuilder){
        
        Seeder.Seed(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

}