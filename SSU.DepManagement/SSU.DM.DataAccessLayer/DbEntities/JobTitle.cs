using Models.View;

namespace SSU.DM.DataAccessLayer.DbEntities;

public class JobTitle
{
    public long Id { get; set; }

    public string Name { get; set; }
    
    public int LowerBoundHours { get; set; }
    
    public int UpperBoundHours { get; set; }

    public virtual List<Teacher> Teachers { get; set; }

    public JobTitleViewItem ToViewItem()
    {
        return new JobTitleViewItem
        {
            Id = Id,
            Name = Name,
            LowerBoundHours = LowerBoundHours,
            UpperBoundHours = UpperBoundHours
        };
    }
}