using Models.Request;

namespace Models.View;

public class RequestViewItem
{
    public int Id { get; set; }

    public string Direction { get; set; }
    
    public string GroupNumber { get; set; }

    public double TotalHours { get; set; }

    public string LessonForm { get; set; }
    
    public TeacherViewItem Teacher { get; set; }

    public ISet<long> AvailableTeacherIds { get; set; }
}