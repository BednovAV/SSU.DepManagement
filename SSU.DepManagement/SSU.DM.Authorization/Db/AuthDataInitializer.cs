using Microsoft.AspNetCore.Identity;

namespace SSU.DM.Authorization.Db;

public class AuthDataInitializer
{
    public static async Task InitialiseAsync(RoleManager<IdentityRole<Guid>> roleManager, List<string> allRoles)
    {
        foreach (var roleName in allRoles)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
    }
}