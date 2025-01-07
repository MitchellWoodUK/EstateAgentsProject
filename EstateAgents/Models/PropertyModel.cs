using System.ComponentModel.DataAnnotations;

namespace EstateAgents.Models
{
    public class PropertyModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Property Title is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage ="Title must be between 5 and 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage ="Property Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Required(ErrorMessage ="Property type is required")]
        public PropertyType PropertyType { get; set; }

        [Required(ErrorMessage ="Listing date is required")]
        public DateTime ListingDate { get; set; }

        [Required(ErrorMessage ="Amount of bedrooms is required")]
        public int AmountofBedrooms { get; set; }
        [Required(ErrorMessage = "Amount of bathrooms is required")]
        public int AmountofBathrooms { get; set; }

        [Required(ErrorMessage = "Property Description is required")]
        [StringLength(500, ErrorMessage = "Description must be below 500 characters")]
        public string Description { get; set; }

        public string Image {  get; set; }

        public double M2 { get; set; }

        public int AmountofParking { get; set; }
    }
}
