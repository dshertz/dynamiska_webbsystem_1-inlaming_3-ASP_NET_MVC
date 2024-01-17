using Microsoft.AspNetCore.Mvc;

namespace westcoast_education.web.Controllers;

    [Route("admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }