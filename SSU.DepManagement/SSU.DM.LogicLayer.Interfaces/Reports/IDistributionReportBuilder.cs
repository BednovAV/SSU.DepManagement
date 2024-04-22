namespace SSU.DM.LogicLayer.Interfaces.Reports;

public interface IDistributionReportBuilder
{
    byte[] BuildReport(ISet<Guid> appFormIds);
}