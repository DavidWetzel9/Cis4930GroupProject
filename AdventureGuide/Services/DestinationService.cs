using AdventureGuide.Models;
using AdventureGuide.ViewModels;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdventureGuide.Models.Destinations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace AdventureGuide.Services
{
    public class DestinationService
    {
        private ApplicationDbContext _context;

        public DestinationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DestinationViewModel> GetDestinations(int? pageNumber, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
                return await GetDestinationsDefault(pageNumber);
            else
                return await GetDestinationsBySearch(pageNumber, searchString);
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

        private async Task<DestinationViewModel> GetDestinationsDefault(int? pageNumber)
        {
            DestinationViewModel viewModel = new DestinationViewModel();
            viewModel.PageViewModel.TotalCount = await _context.Destination.CountAsync();
            viewModel.PageViewModel.PageNumber = (pageNumber ?? 1);
            foreach(Destination destination in await _context.Destination.Skip(((viewModel.PageViewModel.PageNumber) - 1) * viewModel.PageViewModel.PageSize).Take(viewModel.PageViewModel.PageSize).ToListAsync())
            {
                await GetImagePaths(destination);

                viewModel.Destinations.Add(destination);
            }
            return viewModel;
        }

        private async Task<DestinationViewModel> GetDestinationsBySearch(int? pageNumber, string searchString)
        {
            DestinationViewModel viewModel = new DestinationViewModel();
            DestinationKeyword keyword;
            if (Enum.TryParse<DestinationKeyword>(searchString, out keyword))
            {
                List<Keyword> keywords = _context.Keyword.Where(i => i.KeywordString.Equals(searchString)).ToList();
                List<Destination> destinations = new List<Destination>();
                viewModel.PageViewModel.TotalCount = destinations.Count();
                viewModel.PageViewModel.PageNumber = (pageNumber ?? 1);
                foreach (Keyword key in keywords)
                {
                    destinations.Add(_context.Destination.Where(s => s.Id == key.DestinationId).First());
                }
                foreach(Destination destination in destinations.Skip(((viewModel.PageViewModel.PageNumber) - 1) * viewModel.PageViewModel.PageSize).Take(viewModel.PageViewModel.PageSize))
                {
                    await GetImagePaths(destination);

                    viewModel.Destinations.Add(destination);
                }
            }
            else
            {
                var destinationList = await _context.Destination.Where(s => s.Name.Contains(searchString)).ToListAsync();
                viewModel.PageViewModel.TotalCount = destinationList.Count();
                viewModel.PageViewModel.PageNumber = (pageNumber ?? 1);
                foreach (Destination destination in destinationList.Skip(((viewModel.PageViewModel.PageNumber) - 1) * viewModel.PageViewModel.PageSize).Take(viewModel.PageViewModel.PageSize))
                {
                    await GetImagePaths(destination);

                    viewModel.Destinations.Add(destination);
                }
            }
            return viewModel;
        }

        private async Task GetImagePaths(Destination destination)
        {
            destination.ImagePaths = await _context.ImagePath.Where(i => i.DestinationId == destination.Id).ToListAsync();

            if (!destination.ImagePaths.Any())  // check if destination has image associated with it, if not, use placeholder image
            {
                String defaultDestinationImagePath = "/images/defaultDestinationImage.png";

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
