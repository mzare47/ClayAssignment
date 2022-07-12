using AccessControl.Api.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Shared.Lib.Models;

namespace AccessControl.Api.Data
{
    public class ClayDbContextSeed
    {
        public static async Task SeedAsync(ClayDbContext clayDbContext, ILogger<ClayDbContextSeed> logger)
        {
            if (!clayDbContext.Users.Any())
            {
                await clayDbContext.Users.AddRangeAsync(GetPreconfiguredUsers());
                await clayDbContext.Roles.AddRangeAsync(GetPreconfiguredRoles());
                await clayDbContext.UserRoles.AddRangeAsync(GetPreconfiguredUserRoles());
                await clayDbContext.SaveChangesAsync();

                logger.LogInformation($"Seed database associated with context {typeof(ClayDbContext).Name}");
            }
        }

        private static IEnumerable<ApplicationUser> GetPreconfiguredUsers()
        {
            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

            ApplicationUser userAdmin = new ApplicationUser()
            {
                Id = "f8419ab0-8fd7-4bb8-898f-cef8006d91a6",
                UserName = "admin",
                NormalizedUserName = "admin".ToUpper(),
                Email = "admin@email.com",
                NormalizedEmail = "admin@email.com".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = "66e195e5-4175-4660-9019-ebc0dc39c802",
            };
            var userAdminPass = passwordHasher.HashPassword(userAdmin, "Admin*123");
            userAdmin.PasswordHash = userAdminPass;

            ApplicationUser userAccessor1 = new ApplicationUser()
            {
                Id = "a23072bc-9150-429d-9445-cc03f8f19047",
                UserName = "accessor1",
                NormalizedUserName = "accessor1".ToUpper(),
                Email = "accessor1@email.com",
                NormalizedEmail = "accessor1@email.com".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = "90b1fe54-22c8-4826-975a-96beb4e27ee6",
            };
            var userAccessor1Pass = passwordHasher.HashPassword(userAccessor1, "Accessor1*123");
            userAccessor1.PasswordHash = userAccessor1Pass;

            ApplicationUser userAccessor2 = new ApplicationUser()
            {
                Id = "95e7e4ff-3708-4645-91f0-4f20325869a3",
                UserName = "accessor2",
                NormalizedUserName = "accessor2".ToUpper(),
                Email = "accessor2@email.com",
                NormalizedEmail = "accessor2@email.com".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = "90b1fe54-22c8-4826-975a-96beb4e27ee6",
            };
            var userAccessor2Pass = passwordHasher.HashPassword(userAccessor2, "Accessor2*123");
            userAccessor2.PasswordHash = userAccessor2Pass;

            return new List<ApplicationUser>
            {
                userAdmin,
                userAccessor1,
                userAccessor2
            };
        }

        private static IEnumerable<IdentityRole> GetPreconfiguredRoles()
        {
            var roleAdmin = new IdentityRole()
            {
                Id = "29f0ee7a-79c5-4b4e-8adf-876c7e0b8014",
                Name = UserRole.Admin,
                NormalizedName = UserRole.Admin.ToUpper(),
                ConcurrencyStamp = "66e195e5-4175-4660-9019-ebc0dc39c802",
            };

            var roleAccessor = new IdentityRole()
            {
                Id = "8cbeaa93-3fb6-4df0-9afd-eb2ddb8f24a6",
                Name = UserRole.Accessor,
                NormalizedName = UserRole.Accessor.ToUpper(),
                ConcurrencyStamp = "90b1fe54-22c8-4826-975a-96beb4e27ee6",
            };

            return new List<IdentityRole>
            {
                roleAdmin,
                roleAccessor
            };
        }

        private static IEnumerable<IdentityUserRole<string>> GetPreconfiguredUserRoles()
        {
            return new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>() {
                    RoleId = "29f0ee7a-79c5-4b4e-8adf-876c7e0b8014",
                    UserId = "f8419ab0-8fd7-4bb8-898f-cef8006d91a6" },
                new IdentityUserRole<string>() {
                    RoleId = "8cbeaa93-3fb6-4df0-9afd-eb2ddb8f24a6",
                    UserId = "a23072bc-9150-429d-9445-cc03f8f19047" },
                new IdentityUserRole<string>() {
                    RoleId = "8cbeaa93-3fb6-4df0-9afd-eb2ddb8f24a6",
                    UserId = "95e7e4ff-3708-4645-91f0-4f20325869a3" }
            };
        }



    }
}
