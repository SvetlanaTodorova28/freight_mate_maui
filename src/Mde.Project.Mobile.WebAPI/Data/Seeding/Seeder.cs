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
                CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000011")
            },
            new Product{
                Id = Guid.Parse("00000000-0000-0000-0000-000000000022"),
                Name = "Toaster",
                CategoryId =Guid.Parse("00000000-0000-0000-0000-000000000012")
            },
            new Product{
                Id = Guid.Parse("00000000-0000-0000-0000-000000000023"),
                Name = "Sneakers",
                CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000012")
            },
            new Product{
                Id = Guid.Parse("00000000-0000-0000-0000-000000000024"),
                Name = "Training",
                CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000012")
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
            new{ CargosId = Guid.Parse("00000000-0000-0000-0000-000000000031"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000021") },
            new{ CargosId = Guid.Parse("00000000-0000-0000-0000-000000000031"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000022") },
            new{ CargosId = Guid.Parse("00000000-0000-0000-0000-000000000032"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000021") },
            new{ CargosId = Guid.Parse("00000000-0000-0000-0000-000000000032"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000022") }
        };
        var functions = new List<Function>{
            new Function(){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000081"),
                Name = "Admin"
            },
            new Function(){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000082"),
                Name = "Driver"
            },
            new Function(){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000083"),
                Name = "Consignee"
            },
            new Function(){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000084"),
                Name = "Consignor"
            }
        };
        
              //========================== AppUsers =========================
        IPasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
        
        // admin seeden
        var adminUser = new AppUser(){
            Id = "00000000-0000-0000-0000-100000000000",
            UserName = "Admin@fedex.com",
            FirstName = "Admin",
            Email = "Admin@fedex.com",
            NormalizedEmail = "ADMIN@FEDEX.COM",
            NormalizedUserName = "ADMIN@FEDEX.COM",
            EmailConfirmed = true,
            SecurityStamp = "BABUNAPLANINAVHODCHETERI",
            ConcurrencyStamp = "4b277cc7-bcb0-4d91-8aab-08dc4b606f7a",
            FunctionId = Guid.Parse("00000000-0000-0000-0000-000000000081")
           
        };
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin1234");

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
                FunctionId = Guid.Parse("00000000-0000-0000-0000-000000000082")
               
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
                FunctionId = Guid.Parse("00000000-0000-0000-0000-000000000082")
              
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
                LastName = "Stenis",
                FunctionId = Guid.Parse("00000000-0000-0000-0000-000000000084")
              
                }
            };
        var consignors = new List<AppUser>{
            new(){
                Id = "00000000-0000-0000-0000-500000000000",
                UserName = "s@t.com",
                NormalizedUserName = "S@T.COM",
                Email = "s@t.com",
                NormalizedEmail = "S@T.COM",
                EmailConfirmed = true,
                SecurityStamp = "3DIFFERENT3UNIQUE3STRING3",
                ConcurrencyStamp = "3YET3ANOTHER3UNIQUE3STRING3",
                FirstName = "Sve",
                LastName = "Tod",
                FunctionId = Guid.Parse("00000000-0000-0000-0000-000000000083")

            }
        };
        
        
        //Add admin  role to database
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{
            Id = "00000000-0000-0000-0000-000000000060",
            Name = "Admin",
            NormalizedName = "ADMIN"
        });
        //Add  driver role to database
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{
            Id = "00000000-0000-0000-0000-000000000061",
            Name = "Driver",
            NormalizedName = "DRIVER"
        });
        //Add consignee role to database
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{
            Id = "00000000-0000-0000-0000-000000000063",
            Name = "Consignee",
            NormalizedName = "CONSIGNEE"
        });
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{
            Id = "00000000-0000-0000-0000-000000000064",
            Name = "Consignor",
            NormalizedName = "CONSIGNOR"
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
                UserId = "00000000-0000-0000-0000-300000000000",
                RoleId = "00000000-0000-0000-0000-000000000061"
            },
            new IdentityUserRole<string>{
                UserId = "00000000-0000-0000-0000-200000000000",
                RoleId = "00000000-0000-0000-0000-000000000061"
            },
            //consignees
            new IdentityUserRole<string>{
                UserId = "00000000-0000-0000-0000-400000000000",
                RoleId = "00000000-0000-0000-0000-000000000063"
            },
            //consignors
            new IdentityUserRole<string>{
                UserId = "00000000-0000-0000-0000-500000000000",
                RoleId = "00000000-0000-0000-0000-000000000064"
            }
        );
        
        modelBuilder.Entity<Category>().HasData(categories);
        modelBuilder.Entity<Product>().HasData(products);  
        modelBuilder.Entity<Cargo>().HasData(cargos);
        modelBuilder.Entity<Function>().HasData(functions);
        modelBuilder.Entity<AppUser>().HasData(adminUser);
        drivers.ForEach(driver => {
            driver.PasswordHash = passwordHasher.HashPassword(driver, "Driver1234");
            modelBuilder.Entity<AppUser>().HasData(driver);
        });
        consignees.ForEach(consignee => {
            consignee.PasswordHash = passwordHasher.HashPassword(consignee, "Consignee1234");
            modelBuilder.Entity<AppUser>().HasData(consignee);
        });
        consignors.ForEach(consignor => {
            consignor.PasswordHash = passwordHasher.HashPassword(consignor, "1234");
            modelBuilder.Entity<AppUser>().HasData(consignor);
        });
        modelBuilder.Entity($"{nameof(Cargo)}{nameof(Product)}").HasData(cargosProducts);

    }
}