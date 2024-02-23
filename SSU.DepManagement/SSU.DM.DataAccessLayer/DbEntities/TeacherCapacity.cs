using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SSU.DM.DataAccessLayer.DbEntities;

[EntityTypeConfiguration(typeof(TeacherCapacityConfiguration))]
public class TeacherCapacity
{
    public long TeacherId { get; set; }
    public virtual Teacher Teacher { get; set; }

    public long SemesterId { get; set; }
    public virtual Semester Semester { get; set; }
    
    
    public int Hours { get; set; }
}

internal class TeacherCapacityConfiguration : IEntityTypeConfiguration<TeacherCapacity>
{
    public void Configure(EntityTypeBuilder<TeacherCapacity> builder)
    {
        builder.HasKey(capacity => new { capacity.TeacherId, capacity.SemesterId});
        builder.HasOne(capacity => capacity.Teacher)
            .WithMany(teacher => teacher.Capacities)
            .HasForeignKey(capacity => capacity.TeacherId)
            .IsRequired();
        builder.HasOne(capacity => capacity.Semester)
            .WithMany(semester => semester.Capacities)
            .HasForeignKey(capacity => capacity.SemesterId)
            .IsRequired();
    }
}