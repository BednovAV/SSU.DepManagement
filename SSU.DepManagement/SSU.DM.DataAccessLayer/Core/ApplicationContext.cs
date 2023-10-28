using Microsoft.EntityFrameworkCore;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.Core;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<SavedFile> SavedFiles { get; set; } = null!;
    
    public DbSet<ApplicationForm> ApplicationForms { get; set; } = null!;

    public DbSet<Faculty> Faculties { get; set; } = null!;

    public DbSet<Request> Requests { get; set; } = null!;
}
