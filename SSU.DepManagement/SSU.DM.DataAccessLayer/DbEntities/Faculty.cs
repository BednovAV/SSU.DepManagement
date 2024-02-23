namespace SSU.DM.DataAccessLayer.DbEntities;

public class Faculty
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string NameDat { get; set; }

    public virtual List<Competence> Competencies { get; set; }
}