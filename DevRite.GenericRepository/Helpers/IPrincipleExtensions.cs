using System.Security.Claims;
using System.Security.Principal;

namespace DevRite.GenericRepository.Helpers
{
    internal static class IPrincipleExtensions
    {
        public static string GetClaimValue(this IPrincipal user, string claimType)
        {
            var thisUser = user as ClaimsPrincipal;
            var claim = thisUser?.FindFirst(claimType);
            return claim == null ? "" : claim.Value;
        }
    }
}
