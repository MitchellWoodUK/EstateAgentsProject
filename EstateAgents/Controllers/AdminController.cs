using EstateAgents.Data;
using EstateAgents.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<CustomUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext context, UserManager<CustomUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<IActionResult> Index()
        {
            //Getting all relevant data from the database
            var properties = await _context.Properties.ToListAsync();
            var users = await _userManager.Users.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = new Dictionary<string, List<string>>();

            //Populating the dictionary with the user roles data.
            foreach (var user in users)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = userRole.ToList();
            }

            //Create the view model and pass the data into the view:
            var viewModel = new AdminDashboardViewModel
            {
                Users = users,
                Roles = roles,
                Properties = properties,
                UserRoles = userRoles
            };

            return View(viewModel); 
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userID, string roleName)
        {
            //find the user from the ID
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return BadRequest("Invalid User!");
            }
            //Add the role
            await _userManager.AddToRoleAsync(user, roleName);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userID, string roleName)
        {
            //find the user from the ID
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null)
            {
                return BadRequest("Invalid User!");
            }
            //Add the role
            await _userManager.RemoveFromRoleAsync(user, roleName);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            //Validation checks
            //Check if the role name is not null and check if the role doesnt already exist
            if (!string.IsNullOrEmpty(roleName) && !await _roleManager.RoleExistsAsync(roleName))
            {
                //Create role
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            else
            {
                //Error message
                return BadRequest("Role already exists or is empty");
            }
            //refresh the page to show the new role.
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleID)
        {
            //Obtain the role
            var role = await _roleManager.FindByIdAsync(roleID);
            //Delete the role
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }


    }
}
