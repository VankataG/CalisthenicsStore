using Microsoft.AspNetCore.Identity;

namespace CalisthenicsStore.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
