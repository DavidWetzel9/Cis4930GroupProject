using AdventureGuide.Models;
using AdventureGuide.Models.Destinations;
using AdventureGuide.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AdventureGuide.Controllers
{

    public class ManageController : Controller
    {
        private readonly ManageService _service;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ManageController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _service = new ManageService(context);
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AllReviews()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Users()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpDelete]
        [Authorize(Roles = "User")]
        public async void DeleteAccount()
        {
            string userName = _userManager.GetUserName(User);
            string userId = _userManager.GetUserId(User);

            //var user = await _userManager.FindByIdAsync(userId);
            //var userRoles = await _userManager.GetRolesAsync(user);

            await _signInManager.SignOutAsync();

            //if (userRoles.Count > 0)
            //{
            //    foreach (var role in userRoles)
            //    {
            //        await _userManager.RemoveFromRoleAsync(user, role);
            //    }
            //}

            //await _userManager.DeleteAsync(user);
            //_service.DeleteAccount(userName);
        }



        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> Reviews(int? pageNumber)
        {
            string userName = _userManager.GetUserName(User);
            return View(await _service.GetUserReviews(pageNumber, userName));
        }

        [Authorize(Roles = "User")]
        public ActionResult DeleteReview(int id)
        {
            _service.DeleteReview(id);
            return RedirectToAction("Reviews");
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> EditReview(int id)
        {
            return View(await _service.GetReview(id));
        }

        [Authorize(Roles = "User")]
        public ActionResult EditReview(Review review)
        {
            _service.EditReview(review);
            return RedirectToAction("Reviews", new { pageNumber = 1 });
        }
    }
}