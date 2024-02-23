namespace Models.View;

public class JobTitleViewItem
{
    public long Id { get; set; }

    public string Name { get; set; }
    
    public int LowerBoundHours { get; set; }
    
    public int UpperBoundHours { get; set; }

    public override string ToString()
    {
        return Name;
    }
}