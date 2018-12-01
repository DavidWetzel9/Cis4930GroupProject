using AdventureGuide.Models.Destinations;
using System.Collections.Generic;

namespace AdventureGuide.ViewModels.DestinationViewModels
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
