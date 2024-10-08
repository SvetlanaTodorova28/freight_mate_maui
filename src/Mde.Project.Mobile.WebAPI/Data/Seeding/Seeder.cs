using Mde.Project.Mobile.WebAPI.Entities;
using Mde.Project.Mobile.WebAPI.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace Mde.Project.Mobile.WebAPI.Data.Seeding;

public class Seeder{
    public static void Seed(ModelBuilder modelBuilder){
        var categories = new List<Category>{
            new Category{ Id = Guid.Parse("00000000-0000-0000-0000-000000000011"), Name = "Electronics" },
            new Category{ Id = Guid.Parse("00000000-0000-0000-0000-000000000012"), Name = "Home Appliances" },
            new Category{ Id = Guid.Parse("00000000-0000-0000-0000-000000000013"), Name = "Sportswear" }
        };
        var products = new List<Product>{
            new Product{
                Id = Guid.Parse("00000000-0000-0000-0000-000000000021"),
                Name = "Smartphone",
                CategoryId = categories[0].Id
            },
            new Product{
                Id = Guid.Parse("00000000-0000-0000-0000-000000000022"),
                Name = "Toaster",
                CategoryId = categories[1].Id
            },
            new Product{
                Id = Guid.Parse("00000000-0000-0000-0000-000000000023"),
                Name = "Sneakers",
                CategoryId = categories[1].Id
            },

        };
        var cargos = new List<Cargo>{
            new Cargo{
                Id = Guid.Parse("00000000-0000-0000-0000-000000000031"),
                Destination = "Milan",
                TotalWeight = 1500.5,
            },
            new Cargo{
                Id = Guid.Parse("00000000-0000-0000-0000-000000000032"),
                Destination = "London",
                TotalWeight = 2900.0,
            }
        };
        var cargosProducts = new[]{
            new{ CargosId = cargos[0].Id, ProductsId = products[0].Id },
            new{ CargosId = cargos[0].Id, ProductsId = products[1].Id },
            new{ CargosId = cargos[1].Id, ProductsId = products[1].Id },
            new{ CargosId = cargos[1].Id, ProductsId = products[2].Id }
        };
        
              //========================== AppUsers =========================
        IPasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
        
        // admin seeden
        var adminUser = new AppUser(){
            Id = GlobalConstants.AdminId,
            UserName = GlobalConstants.AdminUserName,
            FirstName = "Admin",
            Email = GlobalConstants.AdminUserName,
            NormalizedEmail = GlobalConstants.AdminUserName.ToUpper(),
            NormalizedUserName = GlobalConstants.AdminUserName.ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = "BABUNAPLANINAVHODCHETERI",
            ConcurrencyStamp = "4b277cc7-bcb0-4d91-8aab-08dc4b606f7a",
            AccessLevelType = AccessLevelType.Admin,
        };
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, GlobalConstants.AdminUserPassword);

        // driver seeden
        var drivers = new List<AppUser>{
            new(){
                Id = "00000000-0000-0000-0000-200000000000",
                UserName = "tom@gmail.com",
                NormalizedUserName = "TOM@GMAIL.COM",
                Email = "tom@gmail.com",
                NormalizedEmail = "TOM@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = "1DIFFERENT1UNIQUE1STRING1",
                ConcurrencyStamp = "1YET1ANOTHER1UNIQUE1STRING1", 
                FirstName = "Tom",
                LastName = "Calme",
                AccessLevelType = AccessLevelType.Basic
               
            },
            new(){
                Id = "00000000-0000-0000-0000-300000000000",
                UserName = "sarah@gmail.com",
                NormalizedUserName = "SARAH@GMAIL.COM",
                Email = "sarah@gmail.com",
                NormalizedEmail = "SARAH@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = "2DIFFERENT2UNIQUE2STRING2",
                ConcurrencyStamp = "2YET2ANOTHER2UNIQUE2STRING2", 
                FirstName = "Sarah",
                LastName = "Vrout",
                AccessLevelType = AccessLevelType.Basic
              
            }
        };
        var consignees = new List<AppUser>{
            new(){
                Id = "00000000-0000-0000-0000-400000000000",
                UserName = "milka@speedy.gr",
                NormalizedUserName = "MILKA@SPEEDY.GR",
                Email = "milka@speedy.gr",
                NormalizedEmail = "MILKA@SPEEDY.GR",
                EmailConfirmed = true,
                SecurityStamp = "3DIFFERENT3UNIQUE3STRING3",
                ConcurrencyStamp = "3YET3ANOTHER3UNIQUE3STRING3", 
                FirstName = "Milka",
                LastName = "Stanis",
                AccessLevelType = AccessLevelType.Advanced
              
                }
            };
        
        //Add admin  role to database
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{
            Id = GlobalConstants.AdminRoleId,
            Name = GlobalConstants.AdminRoleName,
            NormalizedName = GlobalConstants.AdminRoleName.ToUpper()
        });
        //Add  driver role to database
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{
            Id = GlobalConstants.DriverRoleId,
            Name = GlobalConstants.DriverRoleName,
            NormalizedName = GlobalConstants.DriverRoleName.ToUpper()
        });
        //Add consignee role to database
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{
            Id = GlobalConstants.ConsigneeRoleId,
            Name = GlobalConstants.ConsigneeRoleName,
            NormalizedName = GlobalConstants.ConsigneeRoleName.ToUpper()
        });
        //Link roles to users
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            //admin
            new IdentityUserRole<string>{
                UserId = GlobalConstants.AdminId,
                RoleId = GlobalConstants.AdminRoleId
            },
            //drivers
            new IdentityUserRole<string>{
                UserId = drivers[0].Id,
                RoleId = GlobalConstants.DriverRoleId
            },
            new IdentityUserRole<string>{
                UserId = drivers[1].Id,
                RoleId = GlobalConstants.DriverRoleId
            },
            //consignees
            new IdentityUserRole<string>{
                UserId = consignees[0].Id,
                RoleId = GlobalConstants.ConsigneeRoleId
            }
        );
        
        modelBuilder.Entity<Category>().HasData(categories);
        modelBuilder.Entity<Product>().HasData(products);  
        modelBuilder.Entity<Cargo>().HasData(cargos);
        modelBuilder.Entity<AppUser>().HasData(adminUser);
        drivers.ForEach(driver => {
            driver.PasswordHash = passwordHasher.HashPassword(driver, GlobalConstants.DriverPassword);
            modelBuilder.Entity<AppUser>().HasData(driver);
        });
        consignees.ForEach(consignee => {
            consignee.PasswordHash = passwordHasher.HashPassword(consignee, GlobalConstants.ConsigneePassword);
            modelBuilder.Entity<AppUser>().HasData(consignee);
        });
        modelBuilder.Entity($"{nameof(Cargo)}{nameof(Product)}").HasData(cargosProducts);

    }
}