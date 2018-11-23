using AdventureGuide.Models;
using AdventureGuide.Models.Destinations;
using AdventureGuide.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureGuide.Controllers
{
    public class DestinationController : Controller
    {
        private readonly DestinationService _service;
        private readonly UserManager<IdentityUser> _userManager;

        public DestinationController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _service = new DestinationService(context);
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int? pageNumber, string currentFilter)
        {
            if (!String.IsNullOrEmpty(currentFilter))
                ViewData["CurrentFilter"] = currentFilter;
            return View(await _service.GetDestinations(pageNumber, currentFilter));
        }

        [HttpGet]
        public async Task<ActionResult> Details(int destinationId)
        {
            return View(await _service.GetDestinationDetails(destinationId));
        }

        [HttpGet]
        public JsonResult KeywordAutoComplete()
        {
            List<string> keywords = new List<string>();
            foreach(var keyword in Enum.GetNames(typeof(DestinationKeyword)))
            {
                keywords.Add(keyword);
            }
            return Json(keywords);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View(new Destination());
        }

        public ActionResult CreateDestination(Destination destination)
        {
            destination.UserId = _userManager.GetUserId(User);
            _service.CreateDestination(destination);
            return RedirectToAction("Details", new { destinationId = destination.Id });
        }

        [HttpPost]
        [Authorize]
        public async Task<PartialViewResult> SubmitReview([FromBody] Review review)
        {
            review.Username = _userManager.GetUserName(User);
            return PartialView("_ReviewDetails", await (_service.AddReview(review)));
        }
    }
}