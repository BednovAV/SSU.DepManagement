using Models.Request;

namespace SSU.DM.LogicLayer.Interfaces.Reports;

public interface IDistributionReportDataBuilder
{
    DistributionReportData BuildData(ISet<Guid> appFormIds);
}