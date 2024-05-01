namespace SSU.DM.LogicLayer.Interfaces.Reports;

public interface IStudyAssignmentReportBuilder
{
    byte[] BuildReport(ISet<Guid> appFormIds);
}
