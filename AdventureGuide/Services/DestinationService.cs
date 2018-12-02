using AdventureGuide.Models;
using AdventureGuide.ViewModels.DestinationViewModels;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdventureGuide.Models.Destinations;
using System.Collections.Generic;

namespace AdventureGuide.Services
{
    public class DestinationService
    {
        private ApplicationDbContext _context;

        public DestinationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DestinationViewModel> GetDestinations(int? pageNumber, string searchString, bool isMapView)
        {
            DestinationViewModel viewModel = new DestinationViewModel();
            viewModel.PageViewModel.PageNumber = pageNumber ?? 1;
            viewModel.PageViewModel.IsMapView = isMapView;
            if (string.IsNullOrEmpty(searchString))
                return await GetDestinationsDefault(viewModel);
            else
                return await GetDestinationsBySearch(viewModel, searchString);
        }

        public async Task<Destination> GetDestinationDetails(int id)
        {
            Destination destination = await _context.Destination.FindAsync(id);
            destination.Keywords = await _context.Keyword.Where(i => i.DestinationId == destination.Id).ToListAsync();
            destination.Reviews = await _context.Review.Where(i => i.DestinationId == destination.Id).ToListAsync();

            await GetImagePaths(destination);

            return destination;
        }

        public void CreateDestination(Destination destination)
        {
            destination.RatingCount = 0;
            destination.RatingSum = 0;
            _context.Destination.Add(destination);
            _context.SaveChanges();
        }

        public void CreateImagePath(ImagePath imagePath)
        {
            _context.ImagePath.Add(imagePath);
            _context.SaveChanges();
        }

        public bool HasReview(int destinationId, string username)
        {
            if (_context.Review.Any(s => s.DestinationId == destinationId && s.Username.Equals(username)))
                return true;
            return false;
        }

        public async Task<List<Review>> AddReview(Review review)
        {
            Destination destination = _context.Destination.Where(i => i.Id == review.DestinationId).First();
            destination.RatingCount += 1;
            destination.RatingSum += review.Rating;
            _context.Review.Add(review);
            _context.SaveChanges();
            List<Review> reviews = await _context.Review.Where(i => i.DestinationId == review.DestinationId).ToListAsync();
            return reviews;
        }

        private async Task<DestinationViewModel> GetDestinationsDefault(DestinationViewModel viewModel)
        {
            viewModel.PageViewModel.TotalCount = await _context.Destination.CountAsync();
            foreach(Destination destination in await _context.Destination.Skip(((viewModel.PageViewModel.PageNumber) - 1) * viewModel.PageViewModel.PageSize).Take(viewModel.PageViewModel.PageSize).ToListAsync())
            {
                await GetImagePaths(destination);

                viewModel.Destinations.Add(destination);
            }
            return viewModel;
        }

        private async Task<DestinationViewModel> GetDestinationsBySearch(DestinationViewModel viewModel, string searchString)
        {
            if (Enum.TryParse(searchString, out DestinationKeyword keyword))
            {
                List<int> destinationIds = await _context.Keyword.Where(i => i.KeywordEnum == keyword).Select(i => i.DestinationId).ToListAsync();
                List<Destination> destinations = new List<Destination>();
                viewModel.PageViewModel.TotalCount = destinationIds.Count();
                foreach (int id in destinationIds)
                {
                    destinations.Add(await _context.Destination.Where(s => s.Id == id).FirstAsync());
                }
                foreach (Destination destination in destinations.Skip(((viewModel.PageViewModel.PageNumber) - 1) * viewModel.PageViewModel.PageSize).Take(viewModel.PageViewModel.PageSize))
                {
                    await GetImagePaths(destination);

                    viewModel.Destinations.Add(destination);
                }
            }
            else
            {
                List<Destination> destinationList = await _context.Destination.Where(s => s.Name.Contains(searchString) || s.City.Contains(searchString) || s.State.Contains(searchString)).ToListAsync();
                viewModel.PageViewModel.TotalCount = destinationList.Count();
                foreach (Destination destination in destinationList.Skip(((viewModel.PageViewModel.PageNumber) - 1) * viewModel.PageViewModel.PageSize).Take(viewModel.PageViewModel.PageSize))
                {
                    await GetImagePaths(destination);

                    viewModel.Destinations.Add(destination);
                }
            }
            return viewModel;
        }

        public async Task<List<ImagePath>> GetNewImages(int destinationId)
        {
            List<ImagePath> images = await _context.ImagePath.Where(i => i.DestinationId == destinationId).ToListAsync();

            if (!images.Any())  // check if destination has image associated with it, if not, use placeholder image
            {
                string defaultDestinationImagePath = "/images/defaultDestinationImage.png";

                ImagePath defaultImage = new ImagePath
                {
                    DestinationId = destinationId,
                    Id = 0,
                    Path = defaultDestinationImagePath
                };

                images.Add(defaultImage);
            }

            return images;
        }

        private async Task GetImagePaths(Destination destination)
        {
            destination.ImagePaths = await _context.ImagePath.Where(i => i.DestinationId == destination.Id).ToListAsync();

            if (!destination.ImagePaths.Any())  // check if destination has image associated with it, if not, use placeholder image
            {
                string defaultDestinationImagePath = "/images/defaultDestinationImage.png";

                ImagePath defaultImage = new ImagePath
                {
                    DestinationId = destination.Id,
                    Id = 0,
                    Path = defaultDestinationImagePath
                };

                destination.ImagePaths.Add(defaultImage);
            }
        }
    }
}
