using Models.View;

namespace SSU.DM.LogicLayer.Interfaces.Semesters;

public interface ISemesterLogic
{
    IReadOnlyList<SemesterViewItem> GetAll();
}