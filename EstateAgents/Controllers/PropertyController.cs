using EstateAgents.Data;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgents.Controllers
{
    public class PropertyController : Controller
    {
        //Inject the database into the controller
        private readonly ApplicationDbContext _context;

        public PropertyController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        //Get: Property/Create
        public IActionResult Create()
        {
            //Gets the view
            return View();
        }



    }
}
