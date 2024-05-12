using Models.Request;

namespace SSU.DM.LogicLayer.Interfaces.Reports;

public interface ICalculationOfHoursDataBuilder
{
    CalculationOfHoursData BuildData(ISet<Guid> appFormIds);
}