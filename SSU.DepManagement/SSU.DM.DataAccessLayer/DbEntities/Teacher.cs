﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Request;
using Models.View;

namespace SSU.DM.DataAccessLayer.DbEntities;

[EntityTypeConfiguration(typeof(TeacherConfiguration))]
public class Teacher
{
    public long Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string MiddleName { get; set; }
    
    public string LastName { get; set; }

    public long? JobTitleId { get; set; }
    public virtual JobTitle? JobTitle { get; set; }

    public float? Rate { get; set; }

    public TeacherRateBounds Bounds => new()
    {
        Lower = Math.Round((JobTitle?.LowerBoundHours ?? 0) * (Rate ?? 0), 1),
        Upper = Math.Round((JobTitle?.UpperBoundHours ?? 0) * (Rate ?? 0), 1),
    };

    public virtual List<Request> Requests { get; set; }
    
    public virtual List<Competence> Сompetencies { get; set; }

    public TeacherViewItem ToViewItem()
        => new()
        {
            Id = Id,
            FirstName = FirstName,
            MiddleName = MiddleName,
            LastName = LastName,
            Rate = Rate,
            Bounds = Bounds,
            JobTitle = JobTitle?.ToViewItem(),
        };
}

internal class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasKey(teacher => teacher.Id);
        builder.HasOne(teacher => teacher.JobTitle)
            .WithMany(jobTitle => jobTitle.Teachers)
            .HasForeignKey(teacher => teacher.JobTitleId);
    }
}