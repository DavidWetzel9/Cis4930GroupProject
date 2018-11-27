using AdventureGuide.Models;
using AdventureGuide.Models.Destinations;
using AdventureGuide.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdventureGuide.Controllers
{
    
    public class ManageController : Controller
    {
        private readonly ManageService _service;
        private readonly UserManager<IdentityUser> _userManager;

        public ManageController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _service = new ManageService(context);
            _userManager = userManager;
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

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> Reviews(int? pageNumber)
        {
            string userName = _userManager.GetUserName(User);
            return View(await _service.GetUserReviews(pageNumber, userName));
        }

        [Authorize(Roles = "User")]
        public ActionResult Delete()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> EditReview(int id)
        {
            return View( await _service.GetReview(id) );
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult EditReview(Review review)
        {
            _service.EditReview(review);
            return RedirectToAction("Reviews", new { pageNumber = 1 });
            //_service.EditReview(review);
            //return RedirectToAction("Reviews", await _service.EditReview(review));
        }
    }
}