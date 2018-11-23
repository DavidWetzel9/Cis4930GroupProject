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

        public async Task<ReviewViewModel> GetUserReviews(int? pageNumber)
        {
            ReviewViewModel viewModel = new ReviewViewModel();
            viewModel.PageViewModel.TotalCount = await _context.Review.CountAsync();
            viewModel.PageViewModel.PageNumber = (pageNumber ?? 1);
            foreach(Review review in await _context.Review.Skip(((viewModel.PageViewModel.PageNumber) - 1) * viewModel.PageViewModel.PageSize).Take(viewModel.PageViewModel.PageSize).ToListAsync())
            {
                viewModel.Reviews.Add(review);
            }
            return viewModel;
        }
         
    }
}