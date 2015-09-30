using Microsoft.AspNet.Identity.EntityFramework;

namespace SE344.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Name on Facebook (full name)
        /// </summary>
        public string Name { get; set; }
    }
}
