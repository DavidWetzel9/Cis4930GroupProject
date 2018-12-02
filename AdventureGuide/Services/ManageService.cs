using AdventureGuide.Models;
using AdventureGuide.ViewModels.DestinationViewModels;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdventureGuide.Models.Destinations;
using System.Collections.Generic;
using System;
using AdventureGuide.ViewModels.ManageViewModels;

namespace AdventureGuide.Services
{
    public class ManageService
    {
        private ApplicationDbContext _context;

        public ManageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserViewModel> GetAllUsers(int? pageNumber)
        {
            UserViewModel viewModel = new UserViewModel();
            var adminRole = await _context.Roles.Where(s => s.Name.Equals("Admin")).FirstAsync();
            var allUsers = await _context.Users.OrderBy(s => s.UserName).ToListAsync();
            var userRoles = await _context.UserRoles.ToListAsync();

            var admins = userRoles.Where(s => s.RoleId.Equals(adminRole.Id)).ToList();

            viewModel.PageViewModel.TotalCount = allUsers.Count() - admins.Count();
            viewModel.PageViewModel.PageNumber = (pageNumber ?? 1);

            for(int i = 0; i < allUsers.Count(); i++)
            {
                Microsoft.AspNetCore.Identity.IdentityUser user = allUsers.ElementAt(i);
                var role = userRoles.Where(s => s.UserId.Equals(user.Id)).First();
                if (role.RoleId.Equals(adminRole.Id))
                {
                    allUsers.Remove(user);
                    i--;
                }
            }

            int maxI = (viewModel.PageViewModel.PageNumber - 1) * viewModel.PageViewModel.PageSize;
            if(allUsers.Count() - maxI > viewModel.PageViewModel.PageSize)
            {
                maxI += viewModel.PageViewModel.PageSize;
            }
            else
            {
                maxI += allUsers.Count() - maxI;
            }

            for (int i = (viewModel.PageViewModel.PageNumber - 1) * viewModel.PageViewModel.PageSize; i < maxI; i++)
            {
                Microsoft.AspNetCore.Identity.IdentityUser user = allUsers.ElementAt(i);
                var role = userRoles.Where(s => s.UserId.Equals(user.Id)).First();                
                viewModel.Users.Add(user);
            }
            //foreach(Microsoft.AspNetCore.Identity.IdentityUser user in allUsers.Skip(((viewModel.PageViewModel.PageNumber) - 1) * viewModel.PageViewModel.PageSize).Take(viewModel.PageViewModel.PageSize))
            //{
            //    var roles = userRoles.Where(s => s.UserId.Equals(user.Id)).ToList();
            //    bool isAdmin = false;
            //    foreach(var r in roles)
            //    {
            //        if (r.RoleId.Equals(adminRole.Id))
            //        {
            //            isAdmin = true;
            //        }
            //    }
            //    if (!isAdmin)
            //    {
            //        viewModel.Users.Add(user);
            //    }
            //}
            return viewModel;
        }

        public void DeleteAccount(string id, string userName)
        {
            var userRolesList = _context.UserRoles.Where(s => s.UserId == id).ToList();
            foreach(Microsoft.AspNetCore.Identity.IdentityUserRole<string> role in userRolesList)
            {
                _context.UserRoles.Remove(role);
            }
            var user = _context.Users.Where(s => s.Id == id);

            var reviewList = _context.Review.Where(s => s.Username == userName).ToList();
            foreach(Review r in reviewList)
            {
                _context.Review.Remove(r);
            }

            foreach(Microsoft.AspNetCore.Identity.IdentityUser u in user)
            {
                _context.Users.Remove(u);
            }
            _context.SaveChanges();
        }

        public async Task<ReviewViewModel> GetAllReviews(int? pageNumber)
        {
            ReviewViewModel viewModel = new ReviewViewModel();
            var allReviews = await _context.Review.OrderBy(s => s.Username).ToListAsync();
            viewModel.PageViewModel.TotalCount = await _context.Review.CountAsync();
            viewModel.PageViewModel.PageNumber = (pageNumber ?? 1);
            foreach (Review review in allReviews.Skip(((viewModel.PageViewModel.PageNumber) - 1) * viewModel.PageViewModel.PageSize).Take(viewModel.PageViewModel.PageSize))
            {
                review.DestinationName = _context.Destination.Where(i => i.Id == review.DestinationId).First().Name;
                viewModel.Reviews.Add(review);
            }
            return viewModel;
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

        public async Task<Review> GetReview(int id)
        {
            Review review = await _context.Review.Where(s => s.Id == id).FirstAsync();
            return review;
        }

        public void EditReview(Review review)
        {
            Review r = _context.Review.Where(s => s.Id == review.Id).First();
            Destination destination = _context.Destination.Where(s => s.Id == r.DestinationId).First();
            destination.RatingSum -= r.Rating;
            destination.RatingSum += review.Rating;
            r.Comment = review.Comment;
            r.Rating = review.Rating;
            _context.SaveChanges();
        }

        public void DeleteReview(int id)
        {
            Review review = _context.Review.Where(s => s.Id == id).First();
            Destination destination = _context.Destination.Where(s => s.Id == review.DestinationId).First();
            destination.RatingCount -= 1;
            destination.RatingSum -= review.Rating;
            _context.Review.Remove(review);
            _context.SaveChanges();
        }
    }
}