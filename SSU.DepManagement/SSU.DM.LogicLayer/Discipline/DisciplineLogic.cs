using Models.Enums;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.LogicLayer.Interfaces.Discipline;

namespace SSU.DM.LogicLayer.Discipline;

public class DisciplineLogic : IDisciplineLogic
{
    private readonly IDisciplineDao _disciplineDao;

    public DisciplineLogic(IDisciplineDao disciplineDao)
    {
        _disciplineDao = disciplineDao;
    }

    public long GetOrCreateDisciplineId(string name)
    {
        var disciplineId = _disciplineDao
            .GetAll(discipline =>
                discipline.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            .FirstOrDefault()?.Id;
        
        if (!disciplineId.HasValue)
        {
            disciplineId = _disciplineDao.Add(new DataAccessLayer.DbEntities.Discipline
            {
                Name = name,
            });
        }

        return disciplineId.Value;
    }
}