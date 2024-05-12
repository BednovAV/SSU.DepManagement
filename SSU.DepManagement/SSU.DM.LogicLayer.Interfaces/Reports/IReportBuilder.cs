using Models.Request;

namespace SSU.DM.LogicLayer.Interfaces.Reports;

public interface IReportBuilder
{
    byte[] BuildReport(GenerateReportsRequest request);
}