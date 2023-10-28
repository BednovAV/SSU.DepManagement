namespace SSU.DM.LogicLayer.Interfaces.Reports;

public interface ICalculationOfHoursBuilder
{
    byte[] BuildReport(ISet<Guid> appFormIds);
}