using CalisthenicsStore.ViewModels.Order;

namespace CalisthenicsStore.ViewModels.ApplicationUser
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<ProfileOrderViewModel> Orders { get; set; } = Enumerable.Empty<ProfileOrderViewModel>();
    }
}
