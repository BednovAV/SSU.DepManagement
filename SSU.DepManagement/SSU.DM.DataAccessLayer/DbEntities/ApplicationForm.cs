using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SSU.DM.DataAccessLayer.DbEntities;

public class ApplicationForm
{
    public Guid ApplicationFormId { get; set; }

    public DateTimeOffset DateTimeCreated { get; set; }

    public string FileKey { get; set; }
    public virtual SavedFile File { get; set; }

    public int? FacultyId { get; set; }
    public virtual Faculty Faculty { get; set; }
    
    public virtual List<Request> Requests { get; set; }
}

internal class ApplicationFormConfiguration : IEntityTypeConfiguration<ApplicationForm>
{
    public void Configure(EntityTypeBuilder<ApplicationForm> builder)
    {
        builder.HasKey(x => x.ApplicationFormId);
        builder.Property(x => x.ApplicationFormId).ValueGeneratedNever();
        builder
            .HasOne(x => x.File)
            .WithOne()
            .HasForeignKey<ApplicationForm>(x => x.FileKey)
            .IsRequired();
        builder.HasOne(x => x.Faculty)
            .WithOne()
            .HasForeignKey<ApplicationForm>(x => x.FacultyId);
    }
}