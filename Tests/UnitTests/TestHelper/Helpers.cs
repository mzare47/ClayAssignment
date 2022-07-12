using AccessControl.Api.Models.Entity;

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
    }
}
