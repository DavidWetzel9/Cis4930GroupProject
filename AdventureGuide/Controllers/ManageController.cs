using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdventureGuide.Controllers
{
    
    public class ManageController : Controller
    {
        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            return View();
        }

        
    }
}