using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AdventureGuide.Classes
{
    public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>
    {
        public ClaimsPrincipalFactory(UserManager<IdentityUser> userManager,
                                      RoleManager<IdentityRole> roleManager,
                                      IOptions<IdentityOptions> options)
            :base(userManager, roleManager, options)
        {

        }
    }
}
