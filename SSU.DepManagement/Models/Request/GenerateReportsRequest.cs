namespace Models.Request;

public class GenerateReportsRequest
{
    public HashSet<Guid> AppFormIds { get; set; }

    public bool CalculationOfHours { get; set; }
    public bool DistributionReport { get; set; }
    public bool StudyAssignmentCard { get; set; }
}