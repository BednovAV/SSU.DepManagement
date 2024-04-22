namespace SSU.DM.WebAssembly.Shared.Models;

public class UpdateTeacherCapacityRequest
{
    public long TeacherId { get; set; }
    
    public Dictionary<long, float> TotalHoursBySemester { get; set; }
}