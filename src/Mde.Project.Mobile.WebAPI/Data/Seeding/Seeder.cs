using Mde.Project.Mobile.WebAPI.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace Mde.Project.Mobile.WebAPI.Data.Seeding;

public class Seeder{
    public static void Seed(ModelBuilder modelBuilder){
        var categories = new List<Category>{
            new (){ Id = Guid.Parse("00000000-0000-0000-0000-000000000011"), Name = "Electronics" },
            new(){ Id = Guid.Parse("00000000-0000-0000-0000-000000000012"), Name = "Home Appliances" },
            new (){ Id = Guid.Parse("00000000-0000-0000-0000-000000000013"), Name = "Sportswear" }
        };
        var products = new List<Product>{
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000021"),
                Name = "Smartphone",
                CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000011")
            },
            new(){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000022"),
                Name = "Toaster",
                CategoryId =Guid.Parse("00000000-0000-0000-0000-000000000012")
            },
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000023"),
                Name = "Sneakers",
                CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000012")
            },
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000024"),
                Name = "Gaz",
                CategoryId = Guid.Parse("00000000-0000-0000-0000-000000000012")
            },

        };
      
        var functions = new List<AccessLevel>{
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000081"),
                Name = "Admin"
            },
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000082"),
                Name = "Advanced"
            },
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000083"),
                Name = "Basic"
            },
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000084"),
                Name = "ManageUsers"
            },
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000085"),
                Name = "CrudProducts"
            },
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000086"),
                Name = "CrudCustomers"
            },
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000087"),
                Name = "SalesRep"
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
            AccessLevelId = Guid.Parse("00000000-0000-0000-0000-000000000081")
           
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
                AccessLevelId = Guid.Parse("00000000-0000-0000-0000-000000000083")
                
               
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
                AccessLevelId = Guid.Parse("00000000-0000-0000-0000-000000000083")
              
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
                AccessLevelId = Guid.Parse("00000000-0000-0000-0000-000000000082")
              
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
                AccessLevelId = Guid.Parse("00000000-0000-0000-0000-000000000082")

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
            Name = "Advanced",
            NormalizedName = "ADVANCED"
        });
        //Add consignee role to database
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole{
            Id = "00000000-0000-0000-0000-000000000062",
            Name = "BASIC",
            NormalizedName = "BASIC"
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
                RoleId = "00000000-0000-0000-0000-000000000062"
            },
            new IdentityUserRole<string>{
                UserId = "00000000-0000-0000-0000-200000000000",
                RoleId = "00000000-0000-0000-0000-000000000062"
            },
            //consignees
            new IdentityUserRole<string>{
                UserId = "00000000-0000-0000-0000-400000000000",
                RoleId = "00000000-0000-0000-0000-000000000061"
            },
            //consignors
            new IdentityUserRole<string>{
                UserId = "00000000-0000-0000-0000-500000000000",
                RoleId = "00000000-0000-0000-0000-000000000061"
            }
        );
          var cargos = new List<Cargo>{
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000031"),
                Destination = "Milan",
                TotalWeight = 1500.5,
                AppUserId = "00000000-0000-0000-0000-200000000000"
            },
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000032"),
                Destination = "London",
                TotalWeight = 2900.0,
                AppUserId = "00000000-0000-0000-0000-400000000000"
            }
            , new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000033"),
                Destination = "Zeebrugge",
                TotalWeight = 1500.5,
                AppUserId = "00000000-0000-0000-0000-400000000000"
            },
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000034"),
                Destination = "Sofia",
                TotalWeight = 2900.0,
                AppUserId = "00000000-0000-0000-0000-400000000000"
            },
             new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000035"),
                Destination = "Zeebrugge",
                TotalWeight = 500.5,
                IsDangerous = true,
                AppUserId = "00000000-0000-0000-0000-300000000000"
            },
            new (){
                Id = Guid.Parse("00000000-0000-0000-0000-000000000036"),
                Destination = "Berlin",
                TotalWeight = 900.0,
                IsDangerous = true,
                AppUserId = "00000000-0000-0000-0000-300000000000"
            }
        };
        var cargosProducts = new[]{
            new{ CargosId = Guid.Parse("00000000-0000-0000-0000-000000000031"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000021") },
            new{ CargosId = Guid.Parse("00000000-0000-0000-0000-000000000031"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000022") },
            new{ CargosId = Guid.Parse("00000000-0000-0000-0000-000000000032"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000021") },
            new{ CargosId = Guid.Parse("00000000-0000-0000-0000-000000000032"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000022") },
            new{ CargosId = Guid.Parse("00000000-0000-0000-0000-000000000035"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000024") },
            new{ CargosId = Guid.Parse("00000000-0000-0000-0000-000000000036"), ProductsId = Guid.Parse("00000000-0000-0000-0000-000000000024") }
        };
        
        modelBuilder.Entity<Category>().HasData(categories);
        modelBuilder.Entity<Product>().HasData(products);  
        modelBuilder.Entity<Cargo>().HasData(cargos);
        modelBuilder.Entity<AccessLevel>().HasData(functions);
        modelBuilder.Entity<AppUser>().HasData(adminUser);
        
        drivers.ForEach(driver => {
            driver.PasswordHash = passwordHasher.HashPassword(driver, "Basic1234");
            modelBuilder.Entity<AppUser>().HasData(driver);
        });
        consignees.ForEach(consignee => {
            consignee.PasswordHash = passwordHasher.HashPassword(consignee, "Advanced1234");
            modelBuilder.Entity<AppUser>().HasData(consignee);
        });
        consignors.ForEach(consignor => {
            consignor.PasswordHash = passwordHasher.HashPassword(consignor, "1234");
            modelBuilder.Entity<AppUser>().HasData(consignor);
        });
        modelBuilder.Entity($"{nameof(Cargo)}{nameof(Product)}").HasData(cargosProducts);
       

    }
}