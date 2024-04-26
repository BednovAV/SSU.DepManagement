using System.Collections;

namespace Models.Request;

public class StudyAssignmentCardData
{
    public string StudyYear { get; set; }
    
    public List<StudyAssignmentTeacherData> Teachers { get; set; }
}

public class StudyAssignmentTeacherData
{
    public string FioWithJobTitle { get; set; }
    
    public IDictionary<string, StudyAssignmentHours> HoursByStudyForm { get; set; }
}

public class StudyAssignmentHours
{
    public double Lectures { get; set; }
    public double Practicals { get; set; }
    public double Laboratory { get; set; }
    public double ControlOfIndependentWork { get; set; }
    public double PreExamConsultation { get; set; }
    public double Exam { get; set; }
    public double Test { get; set; }
    public double PracticeManagement { get; set; }
    public double CourseWork { get; set; }
    public double DiplomaWork { get; set; }
    public double Gac { get; set; }
    public double CheckingTestPaperHours { get; set; }
    public double AspirantManagement { get; set; }
    public double ApplicantManagement { get; set; }
    public double MasterManagement { get; set; }
    public double ExtracurricularActivity { get; set; }
}