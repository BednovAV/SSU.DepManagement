using Models.Enums;

namespace SSU.DM.DataAccessLayer.DbEntities;

public class Discipline
{
    public long Id { get; set; }

    public string Name { get; set; }
    
    //public DisciplineType Type { get; set; }

    public virtual List<Request> Requests { get; set; }
    
    public virtual List<Competence> Competencies { get; set; }
}