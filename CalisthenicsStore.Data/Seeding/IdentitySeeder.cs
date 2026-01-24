using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Seeding.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using static CalisthenicsStore.Common.RolesConstants;

namespace CalisthenicsStore.Data.Seeding
{
    public class IdentitySeeder : IIdentitySeeder
    {
        private readonly string[] DefaultRoles = [AdminRoleName, UserRoleName];

        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;
        private readonly IUserEmailStore<ApplicationUser> emailStore;

        public IdentitySeeder(RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager, 
            IUserStore<ApplicationUser> userStore )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.userStore = userStore;
            this.emailStore = GetEmailStore();
        }
        public async Task SeedIdentityAsync()
        {
            await SeedRolesAsync();
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            foreach (string defaultRole in DefaultRoles)
            {
                bool roleExists = await this.roleManager.RoleExistsAsync(defaultRole);

                if (!roleExists)
                {
                    IdentityRole<Guid> newRole = new IdentityRole<Guid>(defaultRole);

                    IdentityResult result = await this.roleManager.CreateAsync(newRole);

                    if (!result.Succeeded)
                    {
                        throw new Exception($"There was an exception while creating ${defaultRole} role.");
                    }
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            string testUserFirstName = "Test";
            string testUserLastName = "User";
            string testUserEmail = "testUser@abv.bg";
            string testUserPassword = "Test123";

            string adminUserFirstName = "Admin";
            string adminUserLastName = "User";
            string adminUserEmail = "adminUser@abv.bg";
            string adminUserPassword = "Admin123";

            // ---- Test user ----
            var testUserSeeded = await userManager.FindByEmailAsync(testUserEmail);
            if (testUserSeeded == null)
            {
                var testUser = new ApplicationUser
                {
                    FirstName = testUserFirstName,
                    LastName = testUserLastName,
                    UserName = testUserEmail,
                    Email = testUserEmail
                };

                var result = await userManager.CreateAsync(testUser, testUserPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Seeding {UserRoleName} user failed: {errors}");
                }

                result = await userManager.AddToRoleAsync(testUser, UserRoleName);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Assigning {UserRoleName} role failed: {errors}");
                }
            }

            // ---- Admin user ----
            var adminUserSeeded = await userManager.FindByEmailAsync(adminUserEmail);
            if (adminUserSeeded == null)
            {
                var adminUser = new ApplicationUser
                {
                    FirstName = adminUserFirstName,
                    LastName = adminUserLastName,
                    UserName = adminUserEmail,
                    Email = adminUserEmail
                };

                var result = await userManager.CreateAsync(adminUser, adminUserPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Seeding {AdminRoleName} user failed: {errors}");
                }

                result = await userManager.AddToRoleAsync(adminUser, AdminRoleName);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Assigning {AdminRoleName} role failed: {errors}");
                }
            }
        }


        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)userStore;
        }
    }
}
