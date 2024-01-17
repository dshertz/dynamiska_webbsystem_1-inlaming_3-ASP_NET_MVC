using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_education.web.Data;

namespace westcoast_education.web.Controllers
{
    [Route("persons")]
    public class PersonsController : Controller
    {
        private readonly WestcoastEducationContext _context;
        public PersonsController(WestcoastEducationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var persons = await _context.Persons.ToListAsync();
            
            return View("Index", persons);
        }
    }
}