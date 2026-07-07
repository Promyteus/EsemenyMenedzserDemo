using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EsemenyMenedzser.DAL.Seed
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedTechnicalUserAsync(IServiceProvider serviceProvider)
        {
            // Get the UserManager from DI.
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string techEmail = "admin@admin.local";

            var letezoUser = await userManager.FindByEmailAsync(techEmail);

            if (letezoUser == null)
            {
                var techUser = new IdentityUser
                {
                    UserName = techEmail,
                    Email = techEmail,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                string techPassword = "Asdf123.";

                var result = await userManager.CreateAsync(techUser, techPassword);

                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to seed technical user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
