namespace Models.View;

public class TeacherCapacitiesViewItem
{
    public TeacherViewItem Teacher { get; set; }

    public Dictionary<long, CapacityView> CapacityBySemester { get; set; }
}

public class CapacityView
{
    public CapacityView(int AllocatedHours, int TotalHours)
    {
        this.AllocatedHours = AllocatedHours;
        this.TotalHours = TotalHours;
    }

    public int AllocatedHours { get; set; }
    public int TotalHours { get; set; }
}