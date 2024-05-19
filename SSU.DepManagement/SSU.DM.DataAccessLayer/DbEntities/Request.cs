using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Extensions;
using Models.Request;

namespace SSU.DM.DataAccessLayer.DbEntities;

[EntityTypeConfiguration(typeof(RequestConfiguration))]
public class Request
{
    public int Id { get; set; }
    
    public List<string> Direction { get; set; }

    public List<int> Semester { get; set; }

    public int YearSemester => Semester.Any()
        ? Semester[0] % 2 == 0 ? 2 : 1
        : 1;
    
    public List<int> BudgetCount { get; set; }
    
    public List<int> CommercialCount { get; set; }
    
    public List<int> GroupNumber { get; set; }

    public int[][] GroupNumbersByDirection => GroupNumber
        .GroupBy(x => x / 10)
        .Select(x => x.ToArray())
        .ToArray();
    
    public int? SubgroupNumber { get; set; }

    public string GroupNumberString => SubgroupNumber.HasValue
        ? $"{GroupNumber[0]}({SubgroupNumber.Value})"
        : string.Join(", ", GroupNumber);

    public string GroupForm { get; set; }
    
    public int LessonHours { get; init; }

    public List<int> IndependentWorkHours { get; set; }
    
    public List<double> ControlOfIndependentWork { get; set; }

    public List<double> CheckingTestPaperHours { get; set; }

    public List<double?> PreExamConsultation { get; set; }
    
    public List<double?> TestHours { get; set; }
    
    public List<double?> ExamHours { get; set; }
    
    public double PracticeManagement { get; set; }
    
    public double CourseWork { get; set; }
    
    public double DiplomaWork { get; set; }
    
    public double Gac { get; set; }
    
    public double AspirantManagement { get; set; }
    
    public double ApplicantManagement { get; set; }
    
    public double ExtracurricularActivity { get; set; }
    
    public double MasterManagement { get; set; }
    
    public int Other { get; set; }

    public List<ReportingForm> Reporting { get; set; }
    
    public LessonForm? LessonForm { get; init; }
    
    public StudyForm StudyForm { get; init; }

    public string Note { get; set; }

    public Guid ApplicationFormId { get; set; }
    public virtual ApplicationForm ApplicationForm { get; set; }

    public long? TeacherId { get; set; }
    public virtual Teacher Teacher { get; set; }

    public long DisciplineId { get; set; }
    public virtual Discipline Discipline { get; set; }

    public double TotalHours => Math.Round(
        LessonHours
        + (ControlOfIndependentWork?.Sum() ?? 0d)
        + (PreExamConsultation?.SumWhereNotNull() ?? 0d)
        + (TestHours?.SumWhereNotNull() ?? 0d)
        + (ExamHours?.SumWhereNotNull() ?? 0d)
        + PracticeManagement
        + CourseWork
        + DiplomaWork
        + Gac
        + (CheckingTestPaperHours?.Sum() ?? 0d)
        + AspirantManagement
        + ApplicantManagement
        + ExtracurricularActivity
        + MasterManagement
        + Other,
            1);

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
        builder.HasOne(r => r.Teacher)
            .WithMany(t => t.Requests)
            .HasForeignKey(r => r.TeacherId);
        builder.HasOne(r => r.Discipline)
            .WithMany(t => t.Requests)
            .HasForeignKey(r => r.DisciplineId);
    }
}