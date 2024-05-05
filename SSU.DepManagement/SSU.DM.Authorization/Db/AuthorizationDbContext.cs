using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSU.DM.Authorization.Models;

namespace SSU.DM.Authorization.Db;

public class AuthorizationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options)
        : base(options)
    {
        
    }
}