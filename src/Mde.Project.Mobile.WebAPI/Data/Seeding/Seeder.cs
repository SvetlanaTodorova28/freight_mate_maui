using Mde.Project.Mobile.WebAPI.Entities;

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
            new Product{
                Id = Guid.Parse("00000000-0000-0000-0000-000000000024"),
                Name = "Training",
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
        var functions = new List<Function>{
            new Function(){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000081"),
                Name = GlobalConstants.AdminUserName
            },
            new Function(){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000082"),
                Name = GlobalConstants.Driver
            },
            new Function(){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000083"),
                Name = GlobalConstants.Consignee
            },
            new Function(){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000084"),
                Name = GlobalConstants.Consignor
            }
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
            FunctionId = functions[0].Id
           
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
                FunctionId = functions[1].Id
               
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
                FunctionId = functions[1].Id
              
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
                FunctionId = functions[2].Id
              
                }
            };
        
        //Add admin  role to database
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{
            Id = "00000000-0000-0000-0000-000000000060",
            Name = "Admin",
            NormalizedName = "Admin".ToUpper()
        });
        //Add  driver role to database
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{
            Id = "00000000-0000-0000-0000-000000000061",
            Name = "Driver",
            NormalizedName = "Driver".ToUpper()
        });
        //Add consignee role to database
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{
            Id = "00000000-0000-0000-0000-000000000063",
            Name = "Consignee",
            NormalizedName = "Consignee".ToUpper()
        });
        //Link roles to users
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            //admin
            new IdentityUserRole<string>{
                UserId = "00000000-0000-0000-0000-100000000000",
                RoleId = "00000000-0000-0000-0000-000000000060"
            },
            //drivers
            new IdentityUserRole<string>{
                UserId = drivers[0].Id,
                RoleId = "00000000-0000-0000-0000-000000000061"
            },
            new IdentityUserRole<string>{
                UserId = drivers[1].Id,
                RoleId = "00000000-0000-0000-0000-000000000061"
            },
            //consignees
            new IdentityUserRole<string>{
                UserId = consignees[0].Id,
                RoleId = "00000000-0000-0000-0000-000000000063"
            }
        );
        
        modelBuilder.Entity<Category>().HasData(categories);
        modelBuilder.Entity<Product>().HasData(products);  
        modelBuilder.Entity<Cargo>().HasData(cargos);
        modelBuilder.Entity<Function>().HasData(functions);
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