using Models.Request;

namespace SSU.DM.LogicLayer.Interfaces.Reports;

public interface IStudyAssignmentReportDataBuilder
{
    StudyAssignmentCardData BuildData(ISet<Guid> appFormIds);
}
