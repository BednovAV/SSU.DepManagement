using Models.Enums;

namespace SSU.DM.LogicLayer.Interfaces.Discipline;

public interface IDisciplineLogic
{
    long GetOrCreateDisciplineId(string name);
}