using AdventureGuide.Models.Destinations;
using AdventureGuide.ViewModels.DestinationViewModels;
using System.Collections.Generic;

namespace AdventureGuide.ViewModels.ManageViewModels
{
    public class DestinationViewModel
    {
        public List<Destination> Destinations { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public DestinationViewModel()
        {
            Destinations = new List<Destination>();
            PageViewModel = new PageViewModel();
        }

    }
}
