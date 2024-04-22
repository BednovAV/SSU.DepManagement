namespace Models.View;

public class TeacherCapacitiesViewItem
{
    public TeacherViewItem Teacher { get; set; }
    
    public Dictionary<long, double> CapacityBySemester { get; set; }
}