using System.Text;

namespace Models.View;

public class TeacherViewItem
{
    public long Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string MiddleName { get; set; }
    
    public string LastName { get; set; }
    
    public float? Rate { get; set; }
    
    public JobTitleViewItem JobTitle { get; set; }
    
    public override string ToString()
    {
        if (Id == -1)
        {
            return "-";
        }
        
        var builder = new StringBuilder(LastName);
        
        var firstName = FirstName[..1];
        if (firstName.Any())
        {
            builder.Append($" {firstName}.");
        }
        
        var middleName = MiddleName[..1];
        if (middleName.Any())
        {
            builder.Append($" {middleName}.");
        }

        return builder.ToString();
    }
}