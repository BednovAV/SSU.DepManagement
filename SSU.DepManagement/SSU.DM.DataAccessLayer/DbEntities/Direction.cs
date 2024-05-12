using Models.Request;
using SSU.DM.DataAccessLayer.DbEntities.Enums;

namespace SSU.DM.DataAccessLayer.DbEntities;

public class Direction
{
    public long Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public int FacultyId { get; set; }
    public virtual Faculty Faculty { get; set; }

    public StudyForm StudyForm { get; set; }

    public StudyLevel StudyLevel { get; set; }
}