using AdventureGuide.ViewModels.DestinationViewModels; //For the PageViewModel
using System.Collections.Generic;

namespace AdventureGuide.ViewModels.ManageViewModels
{
    public class UserViewModel
    {
        public List<Microsoft.AspNetCore.Identity.IdentityUser> Users { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public UserViewModel()
        {
            Users = new List<Microsoft.AspNetCore.Identity.IdentityUser>();
            PageViewModel = new PageViewModel();
        }
    }
}
