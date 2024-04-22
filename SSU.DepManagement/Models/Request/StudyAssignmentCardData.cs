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
    public int Lectures { get; set; }
    public int Practicals { get; set; }
    public int Laboratory { get; set; }
    public int ControlOfIndependentWork { get; set; }
    public int PreExamConsultation { get; set; }
    public int Exam { get; set; }
    public int PracticeManagement { get; set; }
    public int CourseWork { get; set; }
    public int DiplomaWork { get; set; }
    public int Gac { get; set; }
    public int CheckingTestPaperHours { get; set; }
    public int AspirantManagement { get; set; }
    public int ApplicantManagement { get; set; }
    public int MasterManagement { get; set; }
    public int ExtracurricularActivity { get; set; }
}