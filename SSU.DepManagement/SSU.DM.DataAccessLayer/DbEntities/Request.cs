using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Request;

namespace SSU.DM.DataAccessLayer.DbEntities;

[EntityTypeConfiguration(typeof(RequestConfiguration))]
public class Request
{
    public int Id { get; set; }
    
    public string NameDiscipline { get; set; }

    public string Direction { get; set; }

    public int Semester { get; set; }
    
    public int BudgetCount { get; set; }
    
    public int CommercialCount { get; set; }
    
    public string GroupNumber { get; set; }

    public string GroupForm { get; set; }

    public int TotalHours { get; set; }

    public int LectureHours { get; set; }

    public int PracticalHours { get; set; }

    public int LaboratoryHours { get; set; }

    public int IndependentWorkHours { get; set; }

    public ReportingForm Reporting { get; set; }

    public string Note { get; set; }

    public Guid ApplicationFormId { get; set; }
    public virtual ApplicationForm ApplicationForm { get; set; }
    
    public static Request FromModel(RequestItem model, Guid applicationFormId)
    {
        return new()
        {
            NameDiscipline = model.NameDiscipline,
            Direction = model.Direction,
            Semester = model.Semester,
            BudgetCount = model.BudgetCount,
            CommercialCount = model.CommercialCount,
            GroupNumber = model.GroupNumber,
            GroupForm = model.GroupForm,
            TotalHours = model.TotalHours,
            LectureHours = model.LectureHours,
            PracticalHours = model.PracticalHours,
            LaboratoryHours = model.LaboratoryHours,
            IndependentWorkHours = model.IndependentWorkHours,
            Reporting = model.Reporting,
            Note = model.Note,
            ApplicationFormId = applicationFormId
        };
    }
}

internal class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedOnAdd();
        builder.HasOne(r => r.ApplicationForm)
            .WithMany(a => a.Requests)
            .HasForeignKey(r => r.ApplicationFormId)
            .IsRequired();
    }
}