using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.Request;

namespace SSU.DM.DataAccessLayer.DbEntities;

[EntityTypeConfiguration(typeof(СompetenceConfiguration))]
public class Competence
{
    public long TeacherId { get; set; }
    public virtual Teacher Teacher { get; set; }
    
    public long DisciplineId { get; set; }
    public virtual Discipline Discipline { get; set; }
    
    public int FacultyId { get; set; }
    public virtual Faculty Faculty { get; set; }

    public LessonForm LessonForm { get; set; }
}

internal class СompetenceConfiguration : IEntityTypeConfiguration<Competence>
{
    public void Configure(EntityTypeBuilder<Competence> builder)
    {
        builder.HasKey(competence => new
        {
            competence.TeacherId,
            competence.DisciplineId,
            competence.FacultyId,
            competence.LessonForm
        });
        builder.HasOne(c => c.Teacher)
            .WithMany(t => t.Сompetencies)
            .HasForeignKey(c => c.TeacherId)
            .IsRequired();
        builder.HasOne(c => c.Discipline)
            .WithMany(d => d.Competencies)
            .HasForeignKey(c => c.DisciplineId)
            .IsRequired();
        builder.HasOne(c => c.Faculty)
            .WithMany(f => f.Competencies)
            .HasForeignKey(r => r.FacultyId)
            .IsRequired();
    }
}