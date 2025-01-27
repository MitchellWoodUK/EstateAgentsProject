using EstateAgents.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstateAgents.Controllers
{
    //Only the admin role is authorised to access the controller actions
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        //Inject the database into the controller
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            //Retrieve the properties from the database
            var properties = _context.Properties.ToList();

            return View(properties); //Send the list to the view
        }
    }
}
