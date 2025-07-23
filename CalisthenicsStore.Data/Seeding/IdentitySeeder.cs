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

            
            ApplicationUser? testUserSeeded =
                await this.userStore.FindByNameAsync(testUserEmail, CancellationToken.None);
            if (testUserSeeded == null)
            {
                ApplicationUser testUser = new ApplicationUser()
                {
                    FirstName = testUserFirstName,
                    LastName = testUserLastName,
                };

                await this.userStore.SetUserNameAsync(testUser, testUserEmail, CancellationToken.None);
                await this.emailStore.SetEmailAsync(testUser, testUserEmail, CancellationToken.None);

                IdentityResult result = await this.userManager.CreateAsync(testUser, testUserPassword);
                if (!result.Succeeded)
                {
                    throw new Exception($"There was an exception while seeding the {UserRoleName} user.");
                }

                result = await this.userManager.AddToRoleAsync(testUser, UserRoleName);
                if (!result.Succeeded)
                {
                    throw new Exception($"There was an exception while assigning to {UserRoleName}.");
                }
            }

            ApplicationUser? adminUserSeeded =
                await this.userStore.FindByNameAsync(adminUserEmail, CancellationToken.None);
            if (adminUserSeeded == null)
            {
                ApplicationUser adminUser = new ApplicationUser()
                {
                    FirstName = adminUserFirstName,
                    LastName = adminUserLastName,
                };

                await this.userStore.SetUserNameAsync(adminUser, adminUserEmail, CancellationToken.None);
                await this.emailStore.SetEmailAsync(adminUser, adminUserEmail, CancellationToken.None);

                IdentityResult result = await this.userManager.CreateAsync(adminUser, adminUserPassword);
                if (!result.Succeeded)
                {
                    throw new Exception($"There was an exception while seeding the {AdminRoleName} user.");
                }

                result = await this.userManager.AddToRoleAsync(adminUser, AdminRoleName);
                if (!result.Succeeded)
                {
                    throw new Exception($"There was an exception while assigning to {AdminRoleName}.");
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
