using EstateAgents.Data;
using EstateAgents.Migrations;
using EstateAgents.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            //Retrieve the properties from the database
            var properties = _context.Properties.ToList();

            return View(properties); //Send the list to the view
        }

        [Authorize (Roles= "Admin, Staff")]
        //Get: Property/Create
        public IActionResult Create()
        {
            //Gets the view
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PropertyModel property, IFormFile imageFile)
        {
            //Check if the image has been sent to the backend
            if (imageFile != null && imageFile.Length > 0)
            {
                //Define the path to save the image in
                var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");

                if (!Directory.Exists(imagesDirectory))
                {
                    //If it does not already exist, create it!
                    Directory.CreateDirectory(imagesDirectory);
                }
                //Creating the filepath to save in the database
                var filePath = Path.Combine(imagesDirectory, imageFile.FileName);


                try
                {
                    //save the uploaded image to the server
                    using(var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                    //Set the image file name into the PropertyModel
                    property.Image = imageFile.FileName;

                    //Add the listing date
                    property.ListingDate = DateTime.Now;

                    _context.Properties.Add(property);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("Image", "An error occured while saving the image. Please try again!");
                }
            }
            else
            {
                ModelState.AddModelError("Image", "An error occured while saving the image. Please try again!");
            }
            return View(property);
        }


        [Authorize]
        public IActionResult Details(int id)
        {
            //Fetch the property by the given Id.
            var property = _context.Properties.Find(id);

            if (property == null)
            {
                return NotFound();
            }
            else
            {
                return View(property);
            }
        }


        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            //Find the property from id and then remove it from the database.
            var property = _context.Properties.Find(id);
            _context.Properties.Remove(property);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin,Staff")]
        //Get: Property/Edit/id
        public IActionResult Edit(int id)
        {
            var property = _context.Properties.Find(id);
            return View(property);
        }


        //GET: Property/Search
        public async Task<IActionResult> Search(string search, int? minPrice, int? maxPrice, int? bedrooms, int? bathrooms)
        {
            //Get all the properties so that we can search through them
            var properties = _context.Properties.AsQueryable();
            //If the search criteria has been entered
            if (!string.IsNullOrEmpty(search))
            {
                //Filter out the properties based on the users search criteria
                properties = properties.Where(p =>
                    p.Title.ToLower().Contains(search.ToLower()) ||
                    p.Address.ToLower().Contains(search.ToLower()) ||
                    p.Description.ToLower().Contains(search.ToLower()) 
                    );
            }
            //Run individual searches for the filters
            if (minPrice.HasValue)
            {
                properties = properties.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                properties = properties.Where(p => p.Price <= maxPrice.Value);
            }
            if (bedrooms.HasValue)
            {
                properties = properties.Where(p => p.AmountofBedrooms == bedrooms.Value);
            }
            if (bathrooms.HasValue)
            {
                properties = properties.Where(p => p.AmountofBathrooms == bathrooms.Value);
            }

            return View(await properties.ToListAsync());
        }




    }
}
