﻿using AccessControl.Api.Models.Entity;
using Microsoft.AspNetCore.Identity;

namespace UnitTests.TestHelper
{
    internal static class Helpers
    {
        private static IEnumerable<Lock> GetPreconfiguredLocks()
        {
            var tunnelLock = new Lock()
            {
                LockId = new Guid("10f43841-21db-4ef5-9fd2-d79190bc3b39"),
                Name = "Tunnel Lock",
                Location = "Tunnel Lock Location",
                IsLocked = true,
                AllowUnlocking = true,
            };

            var officeLock = new Lock()
            {
                LockId = new Guid("56c66421-d893-4d27-867d-ae15483f9d76"),
                Name = "Office Lock",
                Location = "Office Lock Location",
                IsLocked = true,
                AllowUnlocking = true,
            };

            var unopenableLock = new Lock()
            {
                LockId = new Guid("e0a686ee-79f5-471f-a07e-f7928ee99304"),
                Name = "Unopenable Lock",
                Location = "Unopenable Lock Location",
                IsLocked = true,
                AllowUnlocking = false,
            };

            return new List<Lock>
            {
                tunnelLock,
                officeLock,
                unopenableLock,
            };
        }
        public static Lock GetOfficeLock()
        {
            return GetPreconfiguredLocks()
                .First(l => l.LockId.ToString().Equals("56c66421-d893-4d27-867d-ae15483f9d76"));
        }

        public static Lock GetTunnelLock()
        {
            return GetPreconfiguredLocks()
                .First(l => l.LockId.ToString().Equals("10f43841-21db-4ef5-9fd2-d79190bc3b39"));
        }

        public static Lock GetUnopenableLock()
        {
            return GetPreconfiguredLocks()
                .First(l => l.LockId.ToString().Equals("e0a686ee-79f5-471f-a07e-f7928ee99304"));
        }

        public static List<Lock> GetAllLocks()
        {
            return GetPreconfiguredLocks().ToList();
        }

        private static IEnumerable<ApplicationUser> GetPreconfiguredUsers()
        {
            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

            ApplicationUser userAdmin = new ApplicationUser()
            {
                Id = "f8419ab0-8fd7-4bb8-898f-cef8006d91a6", // Admin
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
                Id = "a23072bc-9150-429d-9445-cc03f8f19047", // Accessor1
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
                Id = "95e7e4ff-3708-4645-91f0-4f20325869a3", // Accessor2
                UserName = "accessor2",
                NormalizedUserName = "accessor2".ToUpper(),
                Email = "accessor2@email.com",
                NormalizedEmail = "accessor2@email.com".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = "90b1fe54-22c8-4826-975a-96beb4e27ee6",
            };
            var userAccessor2Pass = passwordHasher.HashPassword(userAccessor2, "Accessor2*123");
            userAccessor2.PasswordHash = userAccessor2Pass;

            ApplicationUser userAccessor3 = new ApplicationUser()
            {
                Id = "d0fd01e0-a685-46f3-adec-3e4e0e8786f3", // Accessor3
                UserName = "accessor3",
                NormalizedUserName = "accessor3".ToUpper(),
                Email = "accessor3@email.com",
                NormalizedEmail = "accessor3@email.com".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = "90b1fe54-22c8-4826-975a-96beb4e27ee6",
            };
            var userAccessor3Pass = passwordHasher.HashPassword(userAccessor3, "Accessor3*123");
            userAccessor3.PasswordHash = userAccessor3Pass;

            return new List<ApplicationUser>
            {
                userAdmin,
                userAccessor1,
                userAccessor2,
                userAccessor3,
            };
        }

        public static List<ApplicationUser> GetAllAccessors()
        {
            return GetPreconfiguredUsers().
                Where(a => a.UserName.ToLower().Contains("accessor")).ToList();
        }

        public static ApplicationUser GetAdmin()
        {
            return GetPreconfiguredUsers()
                .First(l => l.Id.Equals("f8419ab0-8fd7-4bb8-898f-cef8006d91a6"));
        }

        public static ApplicationUser GetAccessor1()
        {
            return GetPreconfiguredUsers()
                .First(l => l.Id.Equals("a23072bc-9150-429d-9445-cc03f8f19047"));
        }

        public static ApplicationUser GetAccessor2()
        {
            return GetPreconfiguredUsers()
                .First(l => l.Id.Equals("95e7e4ff-3708-4645-91f0-4f20325869a3"));
        }

        public static ApplicationUser GetAccessor3()
        {
            return GetPreconfiguredUsers()
                .First(l => l.Id.Equals("d0fd01e0-a685-46f3-adec-3e4e0e8786f3"));
        }

        private static IEnumerable<Access> GetPreconfiguredAccesses()
        {
            var accessor1ToOfficeLock = new Access()
            {
                AccessId = new Guid("c930b5c9-a13f-445d-8602-a528cd683fee"),
                AccessorId = "a23072bc-9150-429d-9445-cc03f8f19047", //Accessor1
                LockId = new Guid("56c66421-d893-4d27-867d-ae15483f9d76"), //Office Lock
                IsSuccessful = true,
            };

            var accessor2ToTunnelLock = new Access()
            {
                AccessId = new Guid("c2cb4d78-fb15-44cb-8816-d079edcda676"),
                LockId = new Guid("56c66421-d893-4d27-867d-ae15483f9d76"), //Office Lock
                AccessorId = "95e7e4ff-3708-4645-91f0-4f20325869a3", //Accessor2
                IsSuccessful = false,
                Reason = $"You are not allowed to open Office Lock"
            };

            var accessor3ToTunnelLock = new Access()
            {
                AccessId = new Guid("4812c902-cb04-418c-ab34-11595e203056"),
                LockId = new Guid("10f43841-21db-4ef5-9fd2-d79190bc3b39"), //Tunnel Lock
                AccessorId = "d0fd01e0-a685-46f3-adec-3e4e0e8786f3", //Accessor3
                Reason = "You are not allowed to open any locks"
            };

            var accessor2ToUnopenableLock = new Access()
            {
                AccessId = new Guid("ef7536bc-4234-4f99-ae14-8887c9fa25de"),
                LockId = new Guid("e0a686ee-79f5-471f-a07e-f7928ee99304"), //Unopenable Lock
                AccessorId = "95e7e4ff-3708-4645-91f0-4f20325869a3", //Accessor2
                Reason = "Unopenable Lock is not Unlockable"
            };

            return new List<Access>()
            {
                accessor1ToOfficeLock,
                accessor2ToTunnelLock,
                accessor3ToTunnelLock,
                accessor2ToUnopenableLock,
            };
        }

        public static List<Access> GetAllAccesses()
        {
            return GetPreconfiguredAccesses().ToList();
        }

        private static IEnumerable<LockAccessor> GetPreconfiguredLocksAccessors()
        {
            var accessor1ToOfficeLock = new LockAccessor()
            {
                LockAccessorId = new Guid("67359ea47c394630ac7f7fc723205ded"),
                LockId = new Guid("56c66421-d893-4d27-867d-ae15483f9d76"), //Office Lock
                AccessorId = "a23072bc-9150-429d-9445-cc03f8f19047", //Accessor1
            };

            var accessor1ToTunnelLock = new LockAccessor()
            {
                LockAccessorId = new Guid("1202bd25-36c3-476d-b0e9-7913db980068"),
                LockId = new Guid("10f43841-21db-4ef5-9fd2-d79190bc3b39"), //Tunnel Lock
                AccessorId = "a23072bc-9150-429d-9445-cc03f8f19047" //Accessor1
            };

            var accessor2ToTunnelLock = new LockAccessor()
            {
                LockAccessorId = new Guid("e009c4d6-18eb-4a3b-a4c6-af7046057705"),
                LockId = new Guid("10f43841-21db-4ef5-9fd2-d79190bc3b39"), //Tunnel Lock
                AccessorId = "95e7e4ff-3708-4645-91f0-4f20325869a3" //Accessor2
            };

            var accessor2ToUnopenableLock = new LockAccessor()
            {
                LockAccessorId = new Guid("ef7536bc-4234-4f99-ae14-8887c9fa25de"),
                LockId = new Guid("e0a686ee-79f5-471f-a07e-f7928ee99304"), //Unopenable Lock
                AccessorId = "95e7e4ff-3708-4645-91f0-4f20325869a3" //Accessor2
            };

            return new List<LockAccessor>
            {
                accessor1ToOfficeLock,
                accessor1ToTunnelLock,
                accessor2ToTunnelLock,
                accessor2ToUnopenableLock
            };
        }

        public static List<LockAccessor> GetAccessorLocksAccessors(string accessorId)
        {
            return GetPreconfiguredLocksAccessors()
                                .Where(x => x.AccessorId.Equals(accessorId))
                                .ToList();
        }

        public static List<LockAccessor> GetLockLocksAccessors(string lockId)
        {
            return GetPreconfiguredLocksAccessors()
                                .Where(x => x.LockId.ToString().Equals(lockId))
                                .ToList();
        }

        public static List<LockAccessor> GetAllLocksAccessors()
        {
            return GetPreconfiguredLocksAccessors().ToList();
        }

        public static List<ApplicationUser> GetLockAccessors(string lockId)
        {
            var accessorIds = GetPreconfiguredLocksAccessors()
                                .Where(x => x.LockId.ToString().Equals(lockId))
                                .Select(x => x.AccessorId)
                                .ToList();
            return GetAllAccessors().Where(x => accessorIds.Contains(x.Id)).ToList();
        }

        public static List<Lock> GetAccessorLocks(string accessorId)
        {
            var lockIds = GetPreconfiguredLocksAccessors()
                                .Where(x => x.AccessorId.Equals(accessorId))
                                .Select(x => x.LockId.ToString())
                                .ToList();
            return GetAllLocks().Where(x => lockIds.Contains(x.LockId.ToString())).ToList();
        }
    }
}
