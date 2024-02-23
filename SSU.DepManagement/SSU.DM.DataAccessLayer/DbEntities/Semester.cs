using Models.View;

namespace SSU.DM.DataAccessLayer.DbEntities;

public class Semester
{
    public long Id { get; set; }

    public string Name { get; set; }
    
    public virtual List<TeacherCapacity> Capacities { get; set; }

    public SemesterViewItem ToViewItem()
    {
        return new SemesterViewItem
        {
            Id = Id,
            Name = Name
        };
    }
}