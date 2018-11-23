using AdventureGuide.Models;
using AdventureGuide.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdventureGuide.Models.Destinations;

namespace AdventureGuide.Services
{
    public class ManageService
    {
        private ApplicationDbContext _context;

        public ManageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewViewModel> GetUserReviews(int? pageNumber, string userName)
        {
            ReviewViewModel viewModel = new ReviewViewModel();
            var userReviews = await _context.Review.Where(s => s.Username.Contains(userName)).ToListAsync();
            viewModel.PageViewModel.TotalCount = await _context.Review.CountAsync();
            viewModel.PageViewModel.PageNumber = (pageNumber ?? 1);
            foreach(Review review in userReviews.Skip(((viewModel.PageViewModel.PageNumber) - 1) * viewModel.PageViewModel.PageSize).Take(viewModel.PageViewModel.PageSize))
            {
                review.DestinationName = _context.Destination.Where(i => i.Id == review.DestinationId).First().Name;
                viewModel.Reviews.Add(review);
            }
            return viewModel;
        }
    }
}