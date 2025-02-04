using Microsoft.AspNetCore.Identity;

namespace EstateAgents.Models
{
    public class AdminDashboardViewModel
    {
        public List<CustomUserModel> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<PropertyModel> Properties { get; set; }
        public Dictionary<string, List<string>> UserRoles { get; set; }
    }
}
