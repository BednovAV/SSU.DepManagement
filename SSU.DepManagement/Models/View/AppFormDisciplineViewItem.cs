namespace Models.View;

public class AppFormDisciplineViewItem
{
    
    
    public string Name { get; set; }
    
    public IReadOnlyList<int> Groups { get; set; }
    
    public string Semesters { get; set; }
    
    public double TotalHours { get; set; }
    
    public IReadOnlyList<RequestViewItem> Requests { get; set; }
    
    public string StudyForm { get; set; }
}