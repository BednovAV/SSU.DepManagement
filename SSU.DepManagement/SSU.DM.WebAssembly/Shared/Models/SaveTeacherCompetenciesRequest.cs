using Models.Request;
using Models.View;

namespace SSU.DM.WebAssembly.Shared.Models;

public class SaveTeacherCompetenciesRequest
{
    public long TeacherId { get; set; }

    public List<CompetenceShortInfo> Competencies { get; set; }
    
    public List<PriorityItem> Priorities { get; set; }
}