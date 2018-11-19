using AdventureGuide.Models;
using AdventureGuide.Models.Destinations;
using AdventureGuide.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public ActionResult Create()
        {
            return View(new Destination());
        }
    }
}