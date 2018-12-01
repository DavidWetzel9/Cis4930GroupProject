using AdventureGuide.Models.Destinations;
using System.Collections.Generic;

namespace AdventureGuide.ViewModels.DestinationViewModels
{
    public class ReviewViewModel
    {
        public List<Review> Reviews { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public ReviewViewModel()
        {
            Reviews = new List<Review>();
            PageViewModel = new PageViewModel();
        }
    }
}
