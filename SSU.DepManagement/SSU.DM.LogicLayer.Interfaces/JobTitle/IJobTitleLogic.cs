using Models.View;

namespace SSU.DM.LogicLayer.Interfaces.JobTitle;

public interface IJobTitleLogic
{
    IReadOnlyList<JobTitleViewItem> GetAll();
    
    void Add(JobTitleViewItem jobTitle);
    
    void Update(JobTitleViewItem jobTitle);
    
    void Delete(long id);
}