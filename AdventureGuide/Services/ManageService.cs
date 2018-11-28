﻿using AdventureGuide.Models;
using AdventureGuide.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdventureGuide.Models.Destinations;
using System.Collections.Generic;
using System;

namespace AdventureGuide.Services
{
    public class ManageService
    {
        private ApplicationDbContext _context;

        public ManageService(ApplicationDbContext context)
        {
            _context = context;
        }

        //TODO: Complete deleting account
        public void DeleteAccount(int id)
        {
            var userRolesList = _context.UserRoles.Where(s => Int32.Parse(s.UserId) == id);
            var user = _context.Users.Where(s => Int32.Parse(s.Id) == id);
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
            r.Comment = review.Comment;
            r.Rating = review.Rating;
            _context.SaveChanges();
        }

        public void DeleteReview(int id)
        {
            Review review = _context.Review.Where(s => s.Id == id).First();
            _context.Review.Remove(review);
            _context.SaveChanges();
        }
    }
}