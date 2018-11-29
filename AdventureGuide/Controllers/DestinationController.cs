using AdventureGuide.Models;
using AdventureGuide.Models.Destinations;
using AdventureGuide.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace AdventureGuide.Controllers
{
    public class DestinationController : Controller
    {
        private readonly DestinationService _service;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DestinationController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _service = new DestinationService(context);
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
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

        public ActionResult CreateDestination(IFormFileCollection pictures, Destination destination)
        {
            destination.UserId = _userManager.GetUserId(User);
            _service.CreateDestination(destination);

            if(pictures.Count > 0)
            { 
                UploadPictures(pictures, destination.Id);
            }

            return RedirectToAction("Details", new { destinationId = destination.Id });
        }

        [HttpPost]
        [Authorize]
        public async Task<PartialViewResult> SubmitReview([FromBody] Review review)
        {
            review.Username = _userManager.GetUserName(User);
            return PartialView("_ReviewDetails", await (_service.AddReview(review)));
        }

        private void UploadPictures(IFormFileCollection pictures, int newDestinationId)
        {
            foreach (IFormFile picture in pictures)
            {
                // generate unique GUID (globally unique identifier) for file to upload (prevent filename collisions)
                string pictureName = "/images/destinations/" + newDestinationId.ToString() + "/" + string.Format(@"{0}", Guid.NewGuid()) + Path.GetExtension(picture.FileName);
                string webRoot = _hostingEnvironment.WebRootPath;
                string path = pictureName;

                Directory.CreateDirectory(webRoot + "/images/destinations/" + newDestinationId.ToString());

                picture.CopyTo(new FileStream(webRoot + path, FileMode.Create));

                ImagePath imagePath = new ImagePath
                {
                    DestinationId = newDestinationId,
                    Path = path
                };

                _service.CreateImagePath(imagePath);
            }
        }
    }
}
 