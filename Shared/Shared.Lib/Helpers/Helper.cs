using Shared.Lib.Models;

namespace Shared.Lib.Helpers
{
    public static class Helper
    {
        public static LockStatus getLockStatus(int lockStatus)
        {
            switch (lockStatus)
            {
                case 0: return LockStatus.Lock;
                case 1: return LockStatus.Unlock;
                default: return LockStatus.Undefined;
            }
        }
    }
}
