namespace Models.Request;

public class DistributionReportData
{
    public List<TeacherData> Teachers { get; set; }
}

public class TeacherData
{
    public string TeacherName { get; set; }
    
    public string StudyYear { get; set; }
    
    public string BudgetForm { get; set; }
    
    public string JobTitle { get; set; }
    
    public string Rate { get; set; }

    public List<StudyFormData> FirstSemester { get; set; }
    
    public List<StudyFormData> SecondSemester { get; set; }
}