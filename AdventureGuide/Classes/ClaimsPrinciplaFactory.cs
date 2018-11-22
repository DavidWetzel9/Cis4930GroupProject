using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AdventureGuide.Classes
{
    public class ClaimsPrinciplaFactory : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>
    {
        public ClaimsPrinciplaFactory(UserManager<IdentityUser> userManager,
                                      RoleManager<IdentityRole> roleManager,
                                      IOptions<IdentityOptions> options)
            :base(userManager, roleManager, options)
        {

        }
    }
}
