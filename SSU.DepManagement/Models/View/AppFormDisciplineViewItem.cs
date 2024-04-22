﻿namespace Models.View;

public class AppFormDisciplineViewItem
{
    public string Name { get; set; }
    
    public int Semester { get; set; }

    public IReadOnlyList<int> Groups { get; set; }
    
    public int TotalHours { get; set; }
    
    public IReadOnlyList<RequestViewItem> Requests { get; set; }
}