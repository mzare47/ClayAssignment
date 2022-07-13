using Shared.Lib.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        public static string getUserIdFromToken(string tokenStr)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenStr);
            var uid = token.Claims.First(c => c.Type.Equals("uid")).Value;
            return uid;
        }

        public static List<Claim> getClaimsFromToken(string tokenStr)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenStr);
            return token.Claims.ToList();
        }
    }
}
