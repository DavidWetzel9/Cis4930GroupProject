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
using System.Linq;

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
        public async Task<ActionResult> Index(int? pageNumber, string currentFilter, bool isMapView = false)
        {
            if (!string.IsNullOrEmpty(currentFilter))
                ViewData["CurrentFilter"] = currentFilter;
            return View(await _service.GetDestinations(pageNumber, currentFilter, isMapView));
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
            foreach (var keyword in Enum.GetNames(typeof(DestinationKeyword)))
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

            if (pictures.Count > 0)
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
            return PartialView("_ReviewDetails", await _service.AddReview(review));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> SubmitImages(int newDestinationId) {
            IFormFileCollection pictures = Request.Form.Files;

            UploadPictures(pictures, newDestinationId);

            return Json(await _service.GetNewImages(newDestinationId));
        }

        private void UploadPictures(IFormFileCollection pictures, int newDestinationId)
        {
            string[] supportedTypes = { ".jpg", ".jpeg", ".png" };

            foreach (IFormFile picture in pictures)
            {
                string fileExtension = Path.GetExtension(picture.FileName);

                if(supportedTypes.Contains(fileExtension))
                {
                    string imagesDirectoryPath = "/images/destinations";
                    string webRootPath = _hostingEnvironment.WebRootPath;

                    // generate unique GUID (globally unique identifier) for file to upload (prevent filename collisions)
                    string picturePath = string.Format(@"{0}/{1}/{2}{3}", imagesDirectoryPath, newDestinationId.ToString(), Guid.NewGuid(), fileExtension);
                        
                    string serverSavePath = string.Format(@"{0}{1}/{2}", webRootPath, imagesDirectoryPath, newDestinationId.ToString());
                    Directory.CreateDirectory(serverSavePath);

                    picture.CopyTo(new FileStream(webRootPath + picturePath, FileMode.Create));

                    ImagePath imagePathEntry = new ImagePath
                    {
                        DestinationId = newDestinationId,
                        Path = picturePath
                    };

                    _service.CreateImagePath(imagePathEntry);
                }
            }
        }
    }
}
 