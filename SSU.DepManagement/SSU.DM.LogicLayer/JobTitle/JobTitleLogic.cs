using Models.View;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.LogicLayer.Interfaces.JobTitle;

namespace SSU.DM.LogicLayer.JobTitle;

public class JobTitleLogic : IJobTitleLogic
{
    private readonly IJobTitleDao _jobTitleDao;

    public JobTitleLogic(IJobTitleDao jobTitleDao)
    {
        _jobTitleDao = jobTitleDao;
    }

    public IReadOnlyList<JobTitleViewItem> GetAll()
    {
        return _jobTitleDao.GetAll().Select(jobTitle => jobTitle.ToViewItem()).ToList();
    }
    
    public void Add(JobTitleViewItem jobTitle)
    {
        ValidateBounds(jobTitle);

        _jobTitleDao.Add(new DataAccessLayer.DbEntities.JobTitle
        {
            Name = jobTitle.Name,
            LowerBoundHours = jobTitle.LowerBoundHours,
            UpperBoundHours = jobTitle.UpperBoundHours,
        });
    }

    public void Update(JobTitleViewItem jobTitle)
    {
        ValidateBounds(jobTitle);
        
        _jobTitleDao.Update(new DataAccessLayer.DbEntities.JobTitle
        {
            Id = jobTitle.Id,
            Name = jobTitle.Name,
            LowerBoundHours = jobTitle.LowerBoundHours,
            UpperBoundHours = jobTitle.UpperBoundHours,
        });
    }

    public void Delete(long id)
    {
        _jobTitleDao.DeleteById(id);
    }

    private static void ValidateBounds(JobTitleViewItem jobTitle)
    {
        if (jobTitle.UpperBoundHours < 0 || jobTitle.LowerBoundHours < 0)
        {
            throw new InvalidOperationException();
        }
    }
}