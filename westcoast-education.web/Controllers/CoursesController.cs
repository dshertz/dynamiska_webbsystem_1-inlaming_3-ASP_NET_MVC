using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_education.web.Data;

namespace westcoast_education.web.Controllers
{
    [Route("courses")]
    public class CoursesController : Controller
    {
        private readonly WestcoastEducationContext _context;
        public CoursesController(WestcoastEducationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();
            
            return View("Index", courses);
        }
    }
}