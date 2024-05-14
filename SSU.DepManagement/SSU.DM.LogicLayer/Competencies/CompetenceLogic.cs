using Models.Request;
using Models.View;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DbEntities;
using SSU.DM.LogicLayer.Interfaces.Competencies;
using SSU.DM.WebAssembly.Shared.Models;

namespace SSU.DM.LogicLayer.Competencies;

public class CompetenceLogic : ICompetenceLogic
{
    private readonly ICompetenceDao _competenceDao;
    private readonly IFacultyDao _facultyDao;
    private readonly IDisciplineDao _disciplineDao;

    public CompetenceLogic(
        ICompetenceDao competenceDao,
        IFacultyDao facultyDao,
        IDisciplineDao disciplineDao)
    {
        _competenceDao = competenceDao;
        _facultyDao = facultyDao;
        _disciplineDao = disciplineDao;
    }

    public IDictionary<LessonForm, IReadOnlyList<TeacherCompetenciesViewItem>> GetTeacherCompetencies(
        long teacherId)
    {
        var faculties = _facultyDao.GetAll();
        var disciplines = _disciplineDao.GetAll();

        var teacherCompetencies = _competenceDao.GetTeacherCompetencies(teacherId);

        return Enum.GetValues<LessonForm>()
            .ToDictionary(
                lessonForm => lessonForm,
                lessonForm => BuildTeacherCompetencies(disciplines, faculties, teacherCompetencies, lessonForm));
    }

    private static IReadOnlyList<TeacherCompetenciesViewItem> BuildTeacherCompetencies(
        IReadOnlyList<DataAccessLayer.DbEntities.Discipline> disciplines,
        IReadOnlyList<Faculty> faculties,
        ISet<CompetenceShortInfo> teacherCompetencies,
        LessonForm lessonForm)
    {
        var formResult = new List<TeacherCompetenciesViewItem>();
        foreach (var discipline in disciplines)
        {
            var priority = teacherCompetencies.FirstOrDefault(x =>
                                   x.DisciplineId == discipline.Id && x.LessonForm == lessonForm)
                ?.Priority
                ?? 0;
            var disciplineItem = new TeacherCompetenciesViewItem
            {
                DisciplineId = discipline.Id,
                DisciplineName = discipline.Name,
                Faculties = new List<FacultyDisciplineViewItem>(),
                Priority = priority
            };
            foreach (var faculty in faculties)
            {
                disciplineItem.Faculties.Add(new FacultyDisciplineViewItem
                {
                    Competence = new CompetenceShortInfo(discipline.Id, faculty.Id, lessonForm, priority),
                    FacultyName = faculty.Name,
                    Checked = teacherCompetencies.Contains(new CompetenceShortInfo(discipline.Id, faculty.Id, lessonForm, priority))
                });
            }

            disciplineItem.Checked = disciplineItem.Faculties.All(x => x.Checked);
            formResult.Add(disciplineItem);
        }

        return formResult;
    }

    public void SaveTeacherCompetencies(
        long teacherId,
        IReadOnlyList<CompetenceShortInfo> competencies,
        List<PriorityItem> priorities)
    {
        _competenceDao.SetForTeacher(teacherId, competencies, priorities);
    }
}