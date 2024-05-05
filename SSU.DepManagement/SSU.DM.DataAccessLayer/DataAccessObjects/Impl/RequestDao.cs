using Microsoft.EntityFrameworkCore;
using Models.Request;
using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.DbEntities;

namespace SSU.DM.DataAccessLayer.DataAccessObjects.Impl;

public class RequestDao : BaseDao<Request, int>, IRequestDao
{
    public RequestDao(ApplicationContext ctx) : base(ctx)
    {
    }

    protected override DbSet<Request> SelectDbSet(ApplicationContext db)
        => db.Requests;
    
    protected override int SelectKey(Request entity)
        => entity.Id;

    /*public void AddRange(IEnumerable<ParsedRequest> requestItems, Guid applicationFormId)
    {
        var disciplines = UseContext(db => db.Disciplines.ToList());
        UseContext(db => db.Requests
            .AddRange(requestItems.Select(x =>
            {
                var discipline = disciplines.FirstOrDefault(d =>
                    d.Name.Equals(x.NameDiscipline, StringComparison.InvariantCultureIgnoreCase));
                if (discipline == null)
                {
                    db.Disciplines.Add(new Discipline { Name = x.NameDiscipline });
                    db.SaveChanges();
                    disciplines = db.Disciplines.ToList();
                    discipline = disciplines.First(d =>
                        d.Name.Equals(x.NameDiscipline, StringComparison.InvariantCultureIgnoreCase));
                }
                
                return Request.FromModel(x, applicationFormId, discipline.Id);
            })));
    }*/

    public void AddRange(IEnumerable<RequestSaveItem> requestItems, Guid applicationFormId)
    {
        UseContext(db => db.Requests
            .AddRange(requestItems.Select(item => new Request
            {
                Direction = item.Direction,
                Semester = item.Semester,
                BudgetCount = item.BudgetCount,
                CommercialCount = item.CommercialCount,
                GroupNumber = item.GroupNumber,
                SubgroupNumber = item.SubgroupNumber,
                GroupForm = item.GroupForm,
                LessonHours = item.LessonHours,
                IndependentWorkHours = item.IndependentWorkHours,
                ControlOfIndependentWork = item.ControlOfIndependentWork,
                PreExamConsultation = item.PreExamConsultation,
                ExamHours = item.ExamHours,
                TestHours = item.TestHours,
                PracticeManagement = item.PracticeManagement,
                CourseWork = item.CourseWork,
                DiplomaWork = item.DiplomaWork,
                Gac = item.Gac,
                HasTestPaper = item.HasTestPaper,
                AspirantManagement = item.AspirantManagement,
                ApplicantManagement = item.ApplicantManagement,
                ExtracurricularActivity = item.ExtracurricularActivity,
                MasterManagement = item.MasterManagement,
                Reporting = item.Reporting,
                Note = item.Note,
                ApplicationFormId = applicationFormId,
                DisciplineId = item.DisciplineId,
                LessonForm = item.LessonForm,
                StudyForm = item.StudyForm,
            })));
    }

    public void SetTeacherId(int requestId, long? teacherId)
    {
        var entity = GetById(requestId);
        entity.TeacherId = teacherId;
        UseContext(ctx =>
        {
            ctx.Requests.Update(entity);
        });
    }
}